using Unity.VisualScripting;
using UnityEngine;


namespace Subsystems.CoreComponents
{
    public abstract class AttackCore : CoreComponent
    {
        protected int currentDamage;
        protected IAttackContext context;
        public virtual void Prepare(int damage,Subsystem subsystem)
        {
            currentDamage = damage;
            Initialize(subsystem);
            context = subsystem as IAttackContext;
            if (context == null)
            {
                Debug.LogError($"{GetType().Name} requires IAttackContext");
            }
        }

        public abstract void OnAttackHit();
    }

}
