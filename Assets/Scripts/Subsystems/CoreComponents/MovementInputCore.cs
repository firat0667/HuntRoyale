using UnityEngine;
using UnityEngine.InputSystem;

public class MovementInputCore : CoreComponent
{
    public Vector3 InputVector { get; set; }

    private PlayerInputCore m_playerInput;

    public void SetDirection(Vector3 dir)
    {
        InputVector = dir;
    }
    private void Awake()
    {
        Debug.Log("MovementInputCore Awake — Parent = " + transform.parent.name);
        base.Awake();
        m_playerInput = transform.root.GetComponentInChildren<PlayerInputCore>();
    }
    public override void LogicUpdate()
    {
        if (m_playerInput != null)
        {
            Vector2 move = m_playerInput.MoveInput;
            InputVector = new Vector3(move.x, 0f, move.y);
        }
        else
        {
            InputVector = Vector3.zero;
        }
    }

}
