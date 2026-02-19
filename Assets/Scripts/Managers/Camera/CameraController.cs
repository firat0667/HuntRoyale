using Firat0667.WesternRoyaleLib.Patterns;
using Unity.Cinemachine;
using UnityEngine;

namespace Managers.Camera
{
    public class CameraController : FoundationSingleton<CameraController>, IFoundationSingleton
    {
        public bool Initialized { get; set; }

        [SerializeField] private CinemachineCamera m_playerCamera;

        void Start()
        {

        }

        void Update()
        {

        }
        public void SetCamera(Transform target)
        {
            m_playerCamera.Follow = target;
            m_playerCamera.LookAt = target;
        }
    }
}
