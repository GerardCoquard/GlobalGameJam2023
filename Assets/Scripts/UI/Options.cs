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
    public Image musicFill;
    public Image sfxFill;
    public GameObject pauseMenu;

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
        musicFill.fillAmount = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        float vol2 = Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume",0.1f))*multiplier;
        volMixer.SetFloat("MusicVolume", vol2);
    }
    public void SetVolumeSFX(float volume)
    {
        sfxFill.fillAmount = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        float vol3 = Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume",0.1f))*multiplier;
        volMixer.SetFloat("SFXVolume", vol3);
    }
    void SetSliders()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.1f);
        musicFill.fillAmount = musicSlider.value;
        float vol2 = Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume"))*multiplier;
        volMixer.SetFloat("MusicVolume", vol2);
        
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.1f);
        sfxFill.fillAmount = sfxSlider.value;
        float vol3 = Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume"))*multiplier;
        volMixer.SetFloat("SFXVolume", vol3);
    }
    public void Back()
    {
        if(pauseMenu!=null) pauseMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}

