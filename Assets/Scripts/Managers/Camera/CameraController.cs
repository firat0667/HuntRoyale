using Firat0667.CaseLib.Patterns;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : FoundationSingleton<CameraController>, IFoundationSingleton
{
    public bool Initialized { get; set ; }

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
