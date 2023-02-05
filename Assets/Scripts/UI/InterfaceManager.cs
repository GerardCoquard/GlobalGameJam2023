using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;
    public GameObject pauseMenuObject;
    public GameObject popUpObject;
    public Image popUpImage;
    public bool activeInterface;
    private void Awake() {
        instance = this;
    }
    void Start()
    {
        Resume();
    }
    private void OnEnable() {
        InputManager.AddInputAction("PlayerCancel",InputType.Started,OpenPauseMenu);
    }
    private void OnDisable() {
        InputManager.RemoveInputAction("PlayerCancel",InputType.Started,OpenPauseMenu);
    }

    void OpenPauseMenu()
    {
        if(activeInterface) return;
        pauseMenuObject.SetActive(true);
        Pause();
    }
    public void OpenPopUp(Sprite image)
    {
        if(activeInterface) return;
        activeInterface = true;
        popUpImage.sprite = image;
        popUpObject.SetActive(true);
    }
    public void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        InputManager.ChangeActionMap("UI");
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InputManager.ChangeActionMap("Player");
        Time.timeScale = 1f;
    }
}
