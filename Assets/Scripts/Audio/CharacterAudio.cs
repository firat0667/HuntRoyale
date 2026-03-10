using Managers.Audio;
using UnityEngine;
using Game.Audio;

public class CharacterAudio : MonoBehaviour
{
    [SerializeField] private CharacterAudioProfile m_profile;

    public void PlayAttack() => Play(m_profile?.Attack);
    public void PlayDeath() => Play(m_profile?.Death);
    public void PlaySpawn() => Play(m_profile?.Spawn);

    private void Play(AudioSound sound)
    {
        if (sound == null || sound.Clip == null)
            return;

        AudioManager.Instance.PlayOneShot3D(
            sound.Clip,
            transform.position,
            sound.Volume,
            sound.PitchMin,
            sound.PitchMax
        );
    }
}
