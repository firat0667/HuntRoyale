using Combat.Effects;
using Subsystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EffectUIController : MonoBehaviour
{
    [SerializeField] private Transform m_iconParent;
    [SerializeField] private GameObject m_iconPrefab;

    private Dictionary<StatusEffect, GameObject> m_activeIcons = new();
    private EffectSubsystem m_effectSubsystem;

    private void Awake()
    {
        m_effectSubsystem = GetComponentInParent<EffectSubsystem>();
    }

    private void OnEnable()
    {
        if (m_effectSubsystem == null) return;

        m_effectSubsystem.OnEffectAdded.Connect(AddIcon);
        m_effectSubsystem.OnEffectRemoved.Connect(RemoveIcon);
    }

    private void OnDisable()
    {
        if (m_effectSubsystem == null) return;

        m_effectSubsystem.OnEffectAdded.Disconnect(AddIcon);
        m_effectSubsystem.OnEffectRemoved.Disconnect(RemoveIcon);
    }

    void AddIcon(StatusEffect effect)
    {
        if (effect.Source == null || effect.Source.icon == null)
            return;

        var go = Instantiate(m_iconPrefab, m_iconParent);
        go.GetComponent<Image>().sprite = effect.Source.icon;

        m_activeIcons.Add(effect, go);
    }

    void RemoveIcon(StatusEffect effect)
    {
        if (!m_activeIcons.TryGetValue(effect, out var go))
            return;

        Destroy(go);
        m_activeIcons.Remove(effect);
    }
}