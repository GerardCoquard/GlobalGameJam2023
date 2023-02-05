using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField] float gravity;
    [SerializeField] float speed;
    [SerializeField] float speedMultiplier = 1.5f;
    [SerializeField] float jumpSpeed = 10.0f;
    [SerializeField] float coyoteTime = 0.0f;
    [SerializeField] float maxVerticalVelocity;
    public string audioJump;
    public string audioStep;
    [SerializeField] List<AudioClip> jumps = new List<AudioClip>();
    [SerializeField] List<AudioClip> steps = new List<AudioClip>();
    CharacterController controller;
    public Transform spawnPoint;
    [SerializeField] Vector3 velocity;
    bool onGround = true;
    float timeOnAir;
    bool motion;

    [Header("Audio")]
    [SerializeField] float maxTimeBetweenSteps;
    float currentTime;
    private void Start() {
        controller = GetComponent<CharacterController>();
        motion = true;
        spawnPoint.SetParent(null);
    }
    private void Update() {
        SetGravity();
        if(motion)Move();
        Jump();
    }
    void SetGravity()
    {
        velocity.y -= gravity * Time.deltaTime;
        velocity.y = Mathf.Clamp(velocity.y,-maxVerticalVelocity,maxVerticalVelocity);
    }
    public void Move()
    {
        Vector2 move = InputManager.GetAction("Move").ReadValue<Vector2>();
        bool sprinting = InputManager.GetAction("Sprint").ReadValue<float>() == 1f;
        Vector3 movement = (move.y * transform.forward) + (move.x * transform.right);
        movement = new Vector3(movement.x,velocity.y,movement.z);
        CollisionFlags collisionFlags = controller.Move(movement * (sprinting ? speed*speedMultiplier : speed) * Time.deltaTime);
        CheckCollision(collisionFlags);
       
        if(move != Vector2.zero && onGround)
        {
            currentTime += Time.deltaTime;
            
            if(currentTime>= maxTimeBetweenSteps)
            {
                AudioManager.instance.PlaySoundOneShot(audioStep,steps[Random.Range(0,steps.Count)].name,1);
                currentTime = 0;
            }
        }
    }
    void Jump()
    {
        if (InputManager.GetAction("Jump").WasPressedThisFrame() && onGround)
        {
            if(velocity.y<=0)AudioManager.instance.PlaySoundOneShot(audioJump,jumps[Random.Range(0,jumps.Count)].name,1);
            velocity.y = jumpSpeed;
        }
    }
    void CheckCollision(CollisionFlags collisionFlag)
    {
        if ((collisionFlag & CollisionFlags.Above) != 0 && velocity.y > 0.0f)
        {
            velocity.y = 0.0f;
        }

        if ((collisionFlag & CollisionFlags.Below) != 0)
        {
            velocity.y = 0.0f;
            timeOnAir = 0.0f;
            onGround = true;
        }
        else
        {
            timeOnAir += Time.deltaTime;
            if (timeOnAir > coyoteTime)
            onGround = false;
        }
    }
    public void StopMotion(Vector3 direction, float cooldown,float force)
    {
        StartCoroutine(ActiveMotion(direction,cooldown,force));
    }
    IEnumerator ActiveMotion(Vector3 direction,float cooldown,float force)
    {
        float time = 0;
        motion = false;
        while(time < cooldown)
        {
            time+= Time.deltaTime;
            Vector3 desiredDirection = new Vector3(direction.x,velocity.y,direction.z);
            controller.Move(desiredDirection*force*Time.deltaTime);
            yield return null;
        }
        motion = true;
    }
    public CharacterController GetCharacterController()
    {
        return controller;
    }
}
