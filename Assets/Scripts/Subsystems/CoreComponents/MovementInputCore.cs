using UnityEngine;

public class MovementInputCore : CoreComponent
{
    public Vector3 InputVector { get; private set; }

    private ICharacterInputProvider m_provider;

    protected override void Awake()
    {
        base.Awake();
        m_provider = transform.root.GetComponentInChildren<ICharacterInputProvider>();

        if (m_provider == null)
            Debug.LogError($"[MovementInputCore] No InputProvider on {transform.root.name}");
    }

    public override void LogicUpdate()
    {
        InputVector = m_provider != null ? m_provider.MoveWorld : Vector3.zero;
    }
}
