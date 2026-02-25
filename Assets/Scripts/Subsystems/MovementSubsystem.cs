using Subsystems.CoreComponents;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Subsystems
{
    public class MovementSubsystem : Subsystem
    {
        [SerializeField] private Transform m_visualRoot;

        private MovementPhysicsCore m_physics;
        private Vector3 m_moveDir;

        private Dictionary<object, float> m_speedMultipliers = new();

        public Vector3 Velocity => m_physics.Velocity;
        public float MaxSpeed;
        public float MoveAttackSpeedMult => StatsComponent.MoveAttackSpeedMult;
        public float CurrentSpeed { get; private set; }


        protected virtual void OnEnable()
        {
            ClearAllSpeedMultipliers();
        }

        public float Speed01
        {
            get
            {
                float max = Mathf.Max(StatsComponent.MoveSpeed, 0.01f);
                return Mathf.Clamp01(Velocity.magnitude / max);
            }
        }
        public void ClearAllSpeedMultipliers()
        {
            m_speedMultipliers.Clear();
            MaxSpeed = StatsComponent.MoveSpeed;
            CurrentSpeed = StatsComponent.MoveSpeed;
        }
        protected override void Awake()
        {
            base.Awake();
            GetCoreComponent(ref m_physics);
        }

        public void SetMoveDirection(Vector3 dir)
        {
            m_moveDir = dir;
        }

        public void Stop()
        {
            m_moveDir = Vector3.zero;
        }

        public override void LogicUpdate()
        {
            float finalMultiplier = 1f;

            foreach (var mult in m_speedMultipliers.Values)
                finalMultiplier *= mult;

            float speed = StatsComponent.MoveSpeed * finalMultiplier;

            CurrentSpeed = speed;
            m_physics.Move(m_moveDir, speed);
        }
        public void AddSpeedMultiplier(object source, float multiplier)
        {
            m_speedMultipliers[source] = multiplier;
        }

        public void RemoveSpeedMultiplier(object source)
        {
            if (m_speedMultipliers.ContainsKey(source))
                m_speedMultipliers.Remove(source);
        }
        public void RotateTowards(Vector3 dir)
        {
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.001f) return;

            Quaternion target = Quaternion.LookRotation(dir.normalized);
            float t = 1f - Mathf.Exp(-StatsComponent.RotationSpeed * Time.deltaTime);

            m_visualRoot.rotation = Quaternion.Slerp(m_visualRoot.rotation, target, t);
        }
    }


}
