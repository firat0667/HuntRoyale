using System;
using System.Collections.Generic;
using UnityEngine;
using Firat0667.WesternRoyaleLib.Patterns;
public enum AudioType
{
    BGM,
    Player,
    Enemy,
    SFX,
    UI,
    Voice
}

namespace Managers.Audio
{
    public class AudioManager : FoundationSingleton<AudioManager>, IFoundationSingleton
    {
        private Dictionary<string, AudioSource> _audioSources = new();
        public bool Initialized { get; set; }

        private void Awake()
        {
            if (!Initialized)
            {
                CreateAudioSources();
                Initialized = true;
            }
        }

        /// Creates and configures audio sources for different sound types.
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

        /// Plays a given sound clip.
        public void PlaySound(AudioType type, AudioClip clip, bool loop = false, float volume = 1f, float pitchMin = 1f, float pitchMax = 1f)
        {
            if (_audioSources.TryGetValue(type.ToString(), out AudioSource source))
            {
                source.clip = clip;
                source.loop = loop;
                source.volume = volume;
                source.Play();
            }
        }
        public void PlayOneShot3D(
         AudioClip clip,
         Vector3 position,
         float volume = 1f,
         float pitchMin = 1f,
         float pitchMax = 1f)
        {
            if (clip == null) return;

            GameObject obj = new GameObject("3DAudio");
            obj.transform.position = position;

            AudioSource source = obj.AddComponent<AudioSource>();

            source.clip = clip;
            source.volume = volume;
            source.pitch = UnityEngine.Random.Range(pitchMin, pitchMax);

            source.spatialBlend = 1f;
            source.rolloffMode = AudioRolloffMode.Logarithmic;
            source.dopplerLevel = 0f;

            source.minDistance = 3f;
            source.maxDistance = 25f;

            source.Play();

            Destroy(obj, clip.length);
        }
        /// Stops playing a sound.
        public void StopSound(AudioType type)
        {
            if (_audioSources.TryGetValue(type.ToString(), out AudioSource source))
            {
                source.Stop();
            }
        }
        public void PlayOneShot(AudioType type, AudioClip clip, float volume = 1f, float pitchMin = 1f, float pitchMax = 1f)
        {
            if (_audioSources.TryGetValue(type.ToString(), out AudioSource source))
            {
                if (clip == null)
                {
                    Debug.LogWarning($"[AudioManager] PlayOneShot null clip for {type}");
                    return;
                }
                source.pitch = UnityEngine.Random.Range(pitchMin, pitchMax);
                source.PlayOneShot(clip, volume);
            }
        }



        /// Adjusts the volume of a specific sound type.
        public void SetVolume(AudioType type, float volume)
        {
            if (_audioSources.TryGetValue(type.ToString(), out AudioSource source))
            {
                source.volume = volume;
            }
        }
        public void SetMasterVolume(float volume)
        {
            foreach (var source in _audioSources.Values)
            {
                source.volume = volume;
            }
        }

    }

    /*
     AudioClip jumpSound; 

    void Jump()
    {
        AudioManager.Instance.PlaySound(AudioType.SFX, jumpSound);
    */
}
