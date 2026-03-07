using Managers.Audio;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
   [SerializeField] private CharacterAudioProfile m_profile;

    public void PlayAttack()
    {
        if (m_profile == null || m_profile.Attack.Clip == null)
            return;

        var a = m_profile.Attack;

        AudioManager.Instance.PlayOneShot(
            m_profile.AudioType,
            a.Clip,
            a.Volume,
            a.PitchMin,
            a.PitchMax
        );
    }

    public void PlayDeath()
    {
        if (m_profile == null || m_profile.Death.Clip == null)
            return;

        var a = m_profile.Death;

        AudioManager.Instance.PlayOneShot(
            m_profile.AudioType,
            a.Clip,
            a.Volume,
            a.PitchMin,
            a.PitchMax
        );
    }

    public void PlaySpawn()
    {
        if (m_profile == null || m_profile.Spawn.Clip == null)
            return;

        var a = m_profile.Spawn;

        AudioManager.Instance.PlayOneShot(
            m_profile.AudioType,
            a.Clip,
            a.Volume,
            a.PitchMin,
            a.PitchMax
        );
    }
}
