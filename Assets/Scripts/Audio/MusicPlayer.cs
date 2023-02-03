using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public List<AudioClip> m_SceneOST;
    private AudioClip m_CurrentAudioClip;
    private AudioSource m_MyMusicSource;
    int audioIndex = 0;
    private void Start()
    {
        m_CurrentAudioClip = m_SceneOST[audioIndex];
        AudioManager.instance.m_MyMusicSource.clip = m_CurrentAudioClip;
        AudioManager.instance.m_MyMusicSource.Play();
    }
    private void Update()
    {
        Debug.Log(audioIndex);
        if (!AudioManager.instance.m_MyMusicSource.isPlaying)
        {
            ChangeAudioClip();
        }
        if(AudioManager.instance.m_MyMusicSource.time >= m_CurrentAudioClip.length - 10f)
        {
            AudioManager.instance.FadeOut();
            
        }
    }
    public void SetCurrentMusicClip(string name)
    {
        m_CurrentAudioClip = AudioManager.instance.m_SoundsDictionary[name];
    }

    void ChangeAudioClip()
    {
        if (m_SceneOST[audioIndex++] != null)
        {
            audioIndex++;
            m_CurrentAudioClip = m_SceneOST[audioIndex];
            
        } 
        else
        {
            audioIndex = 0; 
            m_CurrentAudioClip = m_SceneOST[audioIndex];
        }
        m_MyMusicSource.volume = 0;
        AudioManager.instance.FadeIn();
        AudioManager.instance.m_MyMusicSource.clip = m_CurrentAudioClip;
        AudioManager.instance.m_MyMusicSource.Play();
    }
}
