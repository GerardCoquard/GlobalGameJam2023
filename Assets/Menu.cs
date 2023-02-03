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
        InputManager.ChangeActionMap("UI");
    }
    public void Options()
    {
        optionsObject.SetActive(true);
    }
    public void Play()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
