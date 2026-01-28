using UnityEngine;


namespace Subsystems.CoreComponents
{
    public abstract class AttackCore : CoreComponent
    {
        protected int currentDamage;

        public virtual void Prepare(int damage)
        {
            currentDamage = damage;
        }

        public abstract void OnAttackHit();
    }

}
