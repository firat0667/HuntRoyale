using Game.Audio;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Character Audio")]
public class CharacterAudioProfile : ScriptableObject
{
    [Header("Combat")]
    public AudioSound Attack;
    public AudioSound Death;

    [Header("Special")]
    public AudioSound Spawn;

    public AudioType AudioType = AudioType.SFX;
}
