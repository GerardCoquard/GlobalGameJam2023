using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    public Dictionary<string, AudioClip> m_SoundsDictionary;
    private AudioSource m_MyAudioSource;
    public AudioSource m_MyMusicSource;

    public AudioMixer m_MyAudioMixer;

    MusicPlayer m_MyPlayer;

    public float m_FadeSpeed;

    float multiplier = 30;

    bool m_IsChanging;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }
    private void Start()
    {
        FillDictionary();
        m_MyAudioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        m_MyMusicSource = GameObject.Find("MusicSource").GetComponent<AudioSource>();
        m_MyPlayer = m_MyMusicSource.GetComponent<MusicPlayer>();

        float vol2 = Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * multiplier;
        m_MyAudioMixer.SetFloat("MusicVolume", vol2);

        float vol3 = Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * multiplier;
        m_MyAudioMixer.SetFloat("SFXVolume", vol3);
    }

    private void Update()
    {
       
        
    }
    void FillDictionary()
    {
        m_SoundsDictionary = new Dictionary<string, AudioClip>();
        AudioClip[] l_clips = Resources.LoadAll<AudioClip>("Audios/");

        foreach (AudioClip clip in l_clips)
        {
            m_SoundsDictionary.Add(clip.name, clip);
        }
    }
    public void PlaySound(string soundName, float volume)
    {
        if (m_SoundsDictionary.ContainsKey(soundName))
        {
            m_MyAudioSource.volume = volume;
            m_MyAudioSource.outputAudioMixerGroup = m_MyAudioMixer.FindMatchingGroups("SFX")[0];
            m_MyAudioSource.PlayOneShot(m_SoundsDictionary[soundName]);
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }
    public void PlaySound(string soundName, float volume, bool loop)
    {
        if (m_SoundsDictionary.ContainsKey(soundName))
        {

            m_MyAudioSource.volume = volume;
            m_MyAudioSource.loop = loop;
            m_MyAudioSource.outputAudioMixerGroup = m_MyAudioMixer.FindMatchingGroups("SFX")[0];
            m_MyAudioSource.clip = m_SoundsDictionary[soundName];
            m_MyAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }

    public void StopSoundLoop(string soundName)
    {
        if (m_SoundsDictionary.ContainsKey(soundName))
        {
           
            m_MyAudioSource.loop = false;
            m_MyAudioSource.Stop();
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }
    public void PlayMusic(AudioClip _clip, float volume, bool loop)
    {
        m_MyMusicSource.outputAudioMixerGroup = m_MyAudioMixer.FindMatchingGroups("Music")[0];
        m_MyMusicSource.loop = loop;
     

    }

    public void PlayRandomSound(List<AudioClip> clips)
    {
        m_MyAudioSource.outputAudioMixerGroup = m_MyAudioMixer.FindMatchingGroups("SFX")[0];
        m_MyAudioSource.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Count)]);
    }

    public void PlayRandomSoundWithTime(List<AudioClip> clips, float time, float maxTime)
    {
        m_MyAudioSource.outputAudioMixerGroup = m_MyAudioMixer.FindMatchingGroups("SFX")[0];
        if(time >= maxTime)
        {
            m_MyAudioSource.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Count)]);
        }
       
    }
    public void FadeIn()
    {
        StartCoroutine(PlayMusicFadeIn());
    }

    public void FadeOut()
    {
        if (!m_IsChanging)
        {
            StartCoroutine(PlayMusicFadeOut());
        }
        
    }

    IEnumerator PlayMusicFadeIn()
    {
        m_MyPlayer.ChangeAudioClip();
        while (m_MyMusicSource.volume < 1)
        {
            m_MyMusicSource.volume += m_FadeSpeed * Time.deltaTime;
            yield return null;

        }
        m_IsChanging = false;
    }
    IEnumerator PlayMusicFadeOut()
    {
        m_IsChanging = true;
        while (m_MyMusicSource.volume > 0)
        {
            m_MyMusicSource.volume -= m_FadeSpeed * Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(PlayMusicFadeIn());
    }

    public void PlayAudioAtPosition(string soundName, Vector3 spawnPosition, float minDistance, float maxDistance)
    {
        if (m_SoundsDictionary.ContainsKey(soundName))
        {
            AudioSource l_audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
            l_audioSource.clip = m_SoundsDictionary[soundName];
            l_audioSource.spatialBlend = 1;
            l_audioSource.minDistance = minDistance;
            l_audioSource.maxDistance = maxDistance;
            l_audioSource.transform.position = spawnPosition;
            l_audioSource.outputAudioMixerGroup = m_MyAudioMixer.FindMatchingGroups("SFX")[0];
            l_audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }

}
