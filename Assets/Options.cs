using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class Options : MonoBehaviour
{   
    public AudioMixer volMixer; 
    public float multiplier;
    public Slider musicSlider;
    public Slider sfxSlider;

    public void OnEnable()
    {
        SetSliders();
        EventSystem.current.SetSelectedGameObject(musicSlider.gameObject);
        InputManager.AddInputAction("Cancel",InputType.Started,Back);
    }
    private void OnDisable() {
        
        InputManager.RemoveInputAction("Cancel",InputType.Started,Back);
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
    void SetSliders()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
        float vol2 = Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume"))*multiplier;
        volMixer.SetFloat("MusicVolume", vol2);

        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        float vol3 = Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume"))*multiplier;
        volMixer.SetFloat("SFXVolume", vol3);
    }
    public void Back()
    {
        gameObject.SetActive(false);
    }
}

