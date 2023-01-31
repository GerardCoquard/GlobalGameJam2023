using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform pitchController;
    public float m_YawRotationSpeed;
    public float m_PitchRotationSpeed;

    public float m_MinPitch;
    public float m_MaxPitch;
    
    public bool yawInverted;
    public bool pitchInverted;
    float m_Yaw;
    float m_Pitch;
    private void OnEnable() {
        InputManager.AddInputAction("Look",UpdateCamera);
    }
    private void OnDisable() {
        InputManager.RemoveInputAction("Look",UpdateCamera);
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        m_Yaw = transform.rotation.y;
        m_Pitch = pitchController.localRotation.x;
    }
    void UpdateCamera(InputAction.CallbackContext context)
    {
        Vector2 mousePos = context.ReadValue<Vector2>();

        m_Yaw += m_YawRotationSpeed * mousePos.x * Time.fixedDeltaTime * (yawInverted ? -1f : 1f);
        m_Pitch += m_PitchRotationSpeed * mousePos.y * Time.fixedDeltaTime * (pitchInverted ? -1f : 1f);
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        pitchController.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);
    }
}
