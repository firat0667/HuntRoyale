using UnityEngine;

public class ScoreSubsystem : Subsystem
{
    private ScoreCore m_core;
    public int Score => m_core.Score;
    void Start()
    {
        GetCoreComponent(ref m_core);
    }

    public void Add(int amount)
    {
        m_core.Add(amount);
    }
}
