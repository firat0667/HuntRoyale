using Combat.Effects;
using Firat0667.WesternRoyaleLib.Key;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Subsystems
{
    public class EffectSubsystem : Subsystem
    {
        private readonly List<StatusEffect> m_activeEffects = new();

        public IReadOnlyList<StatusEffect> ActiveEffects => m_activeEffects;

        public BasicSignal<StatusEffect> OnEffectAdded = new BasicSignal<StatusEffect>();
        public BasicSignal<StatusEffect> OnEffectRemoved = new BasicSignal<StatusEffect>();


        public void AddEffect(StatusEffect effect)
        {
            var existing = m_activeEffects
                .FirstOrDefault(e => e.GetType() == effect.GetType());

            if (existing != null)
            {
                existing.Refresh(effect);
                return;
            }

            m_activeEffects.Add(effect);
            OnEffectAdded.Emit(effect);
        }
        public void RemoveEffect(StatusEffect effect) 
        {
            if (m_activeEffects.Contains(effect))
            {
                effect.ForceExpire();
                m_activeEffects.Remove(effect);
                OnEffectRemoved.Emit(effect);
            }
        }
        public override void LogicUpdate()
        {
            if (m_activeEffects.Count == 0)
                return;
            for(int i= m_activeEffects.Count - 1; i >= 0; i--)
            {
               var effect = m_activeEffects[i];
                effect.Tick(Time.deltaTime);

                if(effect.IsFinished)
                {
                    effect.ForceExpire();
                    m_activeEffects.RemoveAt(i);
                    OnEffectRemoved.Emit(effect);
                }
            }
        }
    }
}
