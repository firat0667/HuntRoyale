using System;
using System.Collections.Generic;
using UnityEngine;
using Firat0667.CaseLib.Patterns;
public enum AudioType
{
    BGM,  
    SFX,  
    UI,  
    Voice 
}
public class AudioManager : FoundationSingleton<AudioManager>, IFoundationSingleton
{
    private Dictionary<string, AudioSource> _audioSources = new();
    public bool Initialized { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    private void Awake()
    {
        if (!Initialized)
        {
            CreateAudioSources();
            Initialized = true;
        }
    }

    /// <summary>
    /// Creates and configures audio sources for different sound types.
    /// </summary>
    private void CreateAudioSources()
    {
        foreach (AudioType type in Enum.GetValues(typeof(AudioType)))
        {
            GameObject audioObj = new GameObject($"AudioSource_{type}");
            audioObj.transform.SetParent(transform);

            AudioSource source = audioObj.AddComponent<AudioSource>();
            source.playOnAwake = false;

            _audioSources[type.ToString()] = source;
        }
    }

    /// <summary>
    /// Plays a given sound clip.
    /// </summary>
    public void PlaySound(AudioType type, AudioClip clip, bool loop = false, float volume = 1f)
    {
        if (_audioSources.TryGetValue(type.ToString(), out AudioSource source))
        {
            source.clip = clip;
            source.loop = loop;
            source.volume = volume;
            source.Play();
        }
    }

    /// <summary>
    /// Stops playing a sound.
    /// </summary>
    public void StopSound(AudioType type)
    {
        if (_audioSources.TryGetValue(type.ToString(), out AudioSource source))
        {
            source.Stop();
        }
    }

    /// <summary>
    /// Adjusts the volume of a specific sound type.
    /// </summary>
    public void SetVolume(AudioType type, float volume)
    {
        if (_audioSources.TryGetValue(type.ToString(), out AudioSource source))
        {
            source.volume = volume;
        }
    }


}

/*
 AudioClip jumpSound; // Inspector'dan atayacaks?n

void Jump()
{
    AudioManager.Instance.PlaySound(AudioType.SFX, jumpSound);
*/

