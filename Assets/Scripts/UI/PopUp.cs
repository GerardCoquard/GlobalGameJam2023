using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{

    public GameObject popUp;
    public static PopUp instance;
    bool alreadyActivated;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this; 
        }
    }
    void Start()
    {
        popUp.SetActive(false);

    }

    public bool PopUpState()
    {
        return popUp.activeSelf;
    }
    void Update()
    {
        if(InputManager.GetAction("Cancel").WasPressedThisFrame()) ClosePopUp();
    }
    public void OpenPopUp()
    {
        if (!alreadyActivated)
        {
            alreadyActivated = true;
            popUp.SetActive(true);
            InputManager.ChangeActionMap("UI");
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
        }
       
    }
    public void ClosePopUp()
    {
        popUp.SetActive(false);
        InputManager.ChangeActionMap("Player");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;

    }
  
}
