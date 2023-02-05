using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject optionsObject;
    private void OnEnable() {
        InputManager.AddInputAction("Cancel",InputType.Started,Close);
    }
    private void OnDisable() {
        InputManager.RemoveInputAction("Cancel",InputType.Started,Close);
    }
    public void Options()
    {
        AudioManager.instance.PlaySoundOneShot("Botones_Menu","Botones_Menu", 0.5f,false);
        gameObject.SetActive(false);
        optionsObject.SetActive(true);
    }
    public void ExitMenu()
    {
        AudioManager.instance.PlaySoundOneShot("Botones_Menu","Botones_Menu", 0.5f,false);
        SceneManager.LoadScene("Menu");
    }
    public void ResetPlayer()
    {
        AudioManager.instance.PlaySoundOneShot("Botones_Menu","Botones_Menu", 0.5f,false);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player==null) return;
        MovementController playerController = player.GetComponent<MovementController>();
        playerController.GetCharacterController().enabled = false;
        player.transform.position = playerController.spawnPoint.position;
        player.transform.rotation = playerController.spawnPoint.rotation;
        playerController.GetCharacterController().enabled = true;
    }
    public void Close()
    {
        AudioManager.instance.PlaySoundOneShot("Botones_Menu","Botones_Menu", 0.5f,false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InputManager.ChangeActionMap("Player");
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
