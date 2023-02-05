using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    private void OnEnable() {
        InterfaceManager.instance.Pause();
        InputManager.AddInputAction("Cancel",InputType.Started,Close);
        InputManager.AddInputAction("Submit",InputType.Started,Close);
    }
    private void OnDisable() {
        InputManager.RemoveInputAction("Cancel",InputType.Started,Close);
        InputManager.RemoveInputAction("Submit",InputType.Started,Close);
        InterfaceManager.instance.activeInterface = false;
        InterfaceManager.instance.Resume();
    }
    void Close()
    {
        gameObject.SetActive(false);
    }
  
}
