using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Options : MonoBehaviour
{   
    public AudioMixer volMixer; 
    public float multiplier;
    public Slider musicSlider;
    public Slider sfxSlider;

    public void OnEnable()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
        float vol2 = Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume"))*multiplier;
        volMixer.SetFloat("MusicVolume", vol2);

        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        float vol3 = Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume"))*multiplier;
        volMixer.SetFloat("SFXVolume", vol3);

        InputManager.AddInputAction("Canceled",InputType.Started,Back);
    }
    private void OnDisable() {
        
        InputManager.RemoveInputAction("Canceled",InputType.Started,Back);
    }
    public void SetVolumeMusic(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        float vol2 = Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume"))*multiplier;
        volMixer.SetFloat("MusicVolume", vol2);
    }
    public void SetVolumeSFX(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        float vol3 = Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume"))*multiplier;
        volMixer.SetFloat("SFXVolume", vol3);
    }
    void SetToggle(int num, Toggle toggle)
    {
        if(num == 0)
        {
            toggle.isOn = false;
        }
        else
        {
            toggle.isOn = true;
        }
    }

    void CursorModeOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Back()
    {
        gameObject.SetActive(false);
    }
}

