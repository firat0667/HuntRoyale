using UnityEngine;

namespace Subsystems.CoreComponents
{
    public class MovementInputCore : CoreComponent
    {
        public Vector3 InputVector { get; private set; }

        private ICharacterInputProvider m_provider;

        protected override void Awake()
        {
            base.Awake();
            m_provider = transform.root.GetComponentInChildren<ICharacterInputProvider>();
        }

        public override void LogicUpdate()
        {
            InputVector = m_provider != null ? m_provider.MoveWorld : Vector3.zero;
        }
    }
}
