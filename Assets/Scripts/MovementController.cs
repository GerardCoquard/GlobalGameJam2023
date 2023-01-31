using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    public float gravity;
    public CharacterController controller;
    public float speed;
    public float speedMultiplier = 1.5f;
    bool onGround = true;
    public float jumpSpeed = 10.0f;
    private float timeOnAir;
    public float coyoteTime = 0.0f;
    Vector3 velocity;
    private void Update() {
        SetGravity();
        Move();
        Jump();
    }
    void SetGravity()
    {
        velocity.y -= gravity * Time.deltaTime;
        CollisionFlags collisionFlags = controller.Move(velocity * Time.deltaTime);
        CheckCollision(collisionFlags);
    }
    public void Move()
    {
        Vector2 move = InputManager.GetAction("Move").ReadValue<Vector2>();
        bool sprinting = InputManager.GetAction("Sprint").ReadValue<float>() == 1f;
        Vector3 movement = (move.y * transform.forward) + (move.x * transform.right);
        CollisionFlags collisionFlags = controller.Move(movement * (sprinting ? speed*speedMultiplier : speed) * Time.deltaTime);
        CheckCollision(collisionFlags);
    }
    void Jump()
    {
        if (InputManager.GetAction("Jump").ReadValue<float>() == 1f && onGround)
        {
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
}
