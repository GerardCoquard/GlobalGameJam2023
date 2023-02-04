using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string gameSceneName;
    public GameObject optionsObject;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 1f;
        InputManager.ChangeActionMap("UI");
    }
    public void Options()
    {
        AudioManager.instance.PlaySound("Botones_Menu", 0.5f);
        optionsObject.SetActive(true);
    }
    public void Play()
    {
        AudioManager.instance.PlaySound("Botones_Menu", 0.5f);
        SceneManager.LoadScene(gameSceneName);
    }
    public void Exit()
    {
        AudioManager.instance.PlaySound("Botones_Menu", 0.5f);
        Application.Quit();
    }
}
