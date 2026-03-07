
using Managers.Audio;
using UnityEngine;

namespace Game.Audio
{
    public class AudioController : MonoBehaviour
    {
        [Header("BGM Audio Settings")]
        public AudioClip BGMClip;
        public float BGMVolume = 1.0f;

        private void Start()
        {
            PlayBackGroundClip();
        }


        public void PlayBackGroundClip()
        {
            AudioManager.Instance.PlaySound(
                AudioType.BGM,
                BGMClip,
                true,
                BGMVolume
            );
        }
    }



    [System.Serializable]
    public class AudioSound
    {
        public AudioClip Clip;
        [Range(0f, 1f)] public float Volume = 1f;
        [Range(0.5f, 2f)] public float PitchMin = 0.9f;
        [Range(0.5f, 2f)] public float PitchMax = 1.1f;
    }

}
