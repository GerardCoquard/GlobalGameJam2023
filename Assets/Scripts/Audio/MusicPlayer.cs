using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public List<AudioClip> m_SceneOST;
    private AudioClip m_CurrentAudioClip;
    int audioIndex = 0;
    private void Start()
    {
        m_CurrentAudioClip = m_SceneOST[audioIndex];
        AudioManager.instance.m_MyMusicSource.clip = m_CurrentAudioClip;
        AudioManager.instance.m_MyMusicSource.Play();
    }
    private void Update()
    {
  

        if(AudioManager.instance.m_MyMusicSource.time >= m_CurrentAudioClip.length - (m_CurrentAudioClip.length * 1/4))
        {
            StopAllCoroutines();
            AudioManager.instance.FadeOut();     
        }
    }
    public void SetCurrentMusicClip(string name)
    {
        m_CurrentAudioClip = AudioManager.instance.m_SoundsDictionary[name];
    }

    public void ChangeAudioClip()
    {
        
        if (audioIndex+1 < m_SceneOST.Count)
        {
            audioIndex++;
            m_CurrentAudioClip = m_SceneOST[audioIndex];
            
        } 
        else
        {
            audioIndex = 0; 
            m_CurrentAudioClip = m_SceneOST[audioIndex];
        }
        AudioManager.instance.m_MyMusicSource.volume = 0;
        AudioManager.instance.m_MyMusicSource.clip = m_CurrentAudioClip;
        AudioManager.instance.m_MyMusicSource.Play();
    }
}
