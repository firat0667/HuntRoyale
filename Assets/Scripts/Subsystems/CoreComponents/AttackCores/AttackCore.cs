using Unity.VisualScripting;
using UnityEngine;


namespace Subsystems.CoreComponents
{
    public abstract class AttackCore : CoreComponent
    {
        protected int currentDamage;

        public virtual void Prepare(int damage,Subsystem subsystem)
        {
            currentDamage = damage;
            Initialize(subsystem);
        }

        public abstract void OnAttackHit();
    }

}
