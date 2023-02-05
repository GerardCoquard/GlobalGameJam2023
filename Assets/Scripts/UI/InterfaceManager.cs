using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    public GameObject pauseMenuObject;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        InputManager.ChangeActionMap("Player");
    }
    private void OnEnable() {
        InputManager.AddInputAction("PlayerCancel",InputType.Started,OpenPauseMenu);
    }
    private void OnDisable() {
        InputManager.RemoveInputAction("PlayerCancel",InputType.Started,OpenPauseMenu);
    }

    void OpenPauseMenu()
    {
        if (PopUp.instance.PopUpState()) return;
        pauseMenuObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        InputManager.ChangeActionMap("UI");
        Time.timeScale = 0f;
    }
}
