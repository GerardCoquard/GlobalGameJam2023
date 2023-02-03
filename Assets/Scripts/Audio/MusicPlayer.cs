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
        AudioManager.instance.m_MyMusicSource.PlayOneShot(m_CurrentAudioClip);
    }
    private void Update()
    {
        if (!AudioManager.instance.m_MyMusicSource.isPlaying)
        {
            ChangeAudioClip();
        }
    }
    public void SetCurrentMusicClip(string name)
    {
        m_CurrentAudioClip = AudioManager.instance.m_SoundsDictionary[name];
    }

    void ChangeAudioClip()
    {
        if (m_SceneOST[audioIndex++] != null) m_CurrentAudioClip = m_SceneOST[audioIndex++];
        else { audioIndex = 0; m_CurrentAudioClip = m_SceneOST[audioIndex++]; }
        AudioManager.instance.PlayMusic(m_CurrentAudioClip, 1);
    }
}
