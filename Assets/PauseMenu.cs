using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject optionsObject;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InputManager.ChangeActionMap("Player");
    }
    private void OnEnable() {
        InputManager.AddInputAction("Look",InputType.Started,Options);
    }
    private void OnDisable() {
        InputManager.RemoveInputAction("Look",InputType.Started,Options);
    }
    void Options()
    {

    }
}
