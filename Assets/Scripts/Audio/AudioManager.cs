using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    public float transitionDuration;
    public List<UnityEngine.AudioClip> musicPlaylist;
    int currentIndex;
    public static AudioManager instance;
    public Dictionary<string, AudioClip> m_SoundsDictionary;
    public AudioSource m_MyMusicSource;
    public AudioMixer m_MyAudioMixer;
    float multiplier = 30;
    bool transitioning;
    Dictionary<string,AudioSource> instancedAudioSources =  new Dictionary<string, AudioSource>();
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

        float vol2 = Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume",0.1f)) * multiplier;
        m_MyAudioMixer.SetFloat("MusicVolume", vol2);

        float vol3 = Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume",0.1f)) * multiplier;
        m_MyAudioMixer.SetFloat("SFXVolume", vol3);

        m_MyMusicSource.clip = musicPlaylist[currentIndex];
        m_MyMusicSource.Play();
    }
    private void Update() {
        if(m_MyMusicSource.time >= m_MyMusicSource.clip.length - (transitionDuration/2) && !transitioning)
        {
            StartCoroutine(Transition());
        }
    }
    IEnumerator Transition()
    {
        transitioning = true;
        float time = 0;
        while(time < transitionDuration/2)
        {
            m_MyMusicSource.volume = Mathf.Lerp(1,0,time/transitionDuration/2);
            time+=Time.unscaledDeltaTime;
            yield return null;
        }

        m_MyMusicSource.volume = 0;
        m_MyMusicSource.Stop();
        currentIndex = currentIndex+1 < musicPlaylist.Count ? currentIndex+1 : 0;
        m_MyMusicSource.clip = musicPlaylist[currentIndex];
        m_MyMusicSource.Play();

        time = 0;
        while(time < transitionDuration/2)
        {
            m_MyMusicSource.volume = Mathf.Lerp(0,1,time/transitionDuration/2);
            time+=Time.unscaledDeltaTime;
            yield return null;
        }
        m_MyMusicSource.volume = 1;
        transitioning = false;
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
    public void PlaySound(string soundName, float volume,bool loop)
    {
        if(instancedAudioSources.ContainsKey(soundName))
        {
            if(!instancedAudioSources[soundName].isPlaying) instancedAudioSources[soundName].Play();
            return;
        }
        if (m_SoundsDictionary.ContainsKey(soundName))
        {
            AudioSource audioSource = CreateInstanceAudioSource(soundName,volume,loop);
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }
    public void PlaySound(string soundName, string clipName, float volume,bool loop)
    {
        if(!m_SoundsDictionary.ContainsKey(clipName)) return;
        if(instancedAudioSources.ContainsKey(soundName))
        {
            if(instancedAudioSources[soundName].clip != m_SoundsDictionary[clipName]) instancedAudioSources[soundName].clip = m_SoundsDictionary[clipName];
            if(!instancedAudioSources[soundName].isPlaying) instancedAudioSources[soundName].Play();
            return;
        }

        AudioSource l_audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
        l_audioSource.clip = m_SoundsDictionary[clipName];
        l_audioSource.spatialBlend = 0;
        l_audioSource.volume = volume;
        l_audioSource.loop = loop;
        l_audioSource.outputAudioMixerGroup = m_MyAudioMixer.FindMatchingGroups("SFX")[0];
        instancedAudioSources.Add(soundName,l_audioSource);
        l_audioSource.Play();
    }
    public void PlaySoundOneShot(string soundName, string clipName,float volume)
    {
        if(!m_SoundsDictionary.ContainsKey(clipName)) return;
        if(instancedAudioSources.ContainsKey(soundName))
        {
            if(m_SoundsDictionary.ContainsKey(clipName)) instancedAudioSources[soundName].PlayOneShot(m_SoundsDictionary[clipName]);
            return;
        }
        else
        {
            AudioSource l_audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
            l_audioSource.spatialBlend = 0;
            l_audioSource.volume = volume;
            l_audioSource.loop = false;
            l_audioSource.outputAudioMixerGroup = m_MyAudioMixer.FindMatchingGroups("SFX")[0];
            instancedAudioSources.Add(soundName,l_audioSource);
            l_audioSource.PlayOneShot(m_SoundsDictionary[clipName]);
        }
    }
    public void StopSound(string soundName)
    {
        if (instancedAudioSources.ContainsKey(soundName))
        {
           instancedAudioSources[soundName].Stop();
        }
    }
    public void PlaySoundOneShotAtPosition(string soundName, float volume ,Vector3 spawnPosition, float minDistance, float maxDistance)
    {
        if (m_SoundsDictionary.ContainsKey(soundName))
        {
            AudioSource l_audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
            l_audioSource.clip = m_SoundsDictionary[soundName];
            l_audioSource.spatialBlend = 1;
            l_audioSource.loop = false;
            l_audioSource.volume = volume;
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
    AudioSource CreateInstanceAudioSource(string soundName, float volume, bool loop)
    {
        AudioSource l_audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
        l_audioSource.clip = m_SoundsDictionary[soundName];
        l_audioSource.spatialBlend = 0;
        l_audioSource.volume = volume;
        l_audioSource.loop = loop;
        l_audioSource.outputAudioMixerGroup = m_MyAudioMixer.FindMatchingGroups("SFX")[0];
        instancedAudioSources.Add(soundName,l_audioSource);
        return l_audioSource;
    }

}
