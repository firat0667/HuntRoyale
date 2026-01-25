using Subsystems.CoreComponents;
using UnityEngine;

public class PlayerInputProvider : MonoBehaviour, ICharacterInputProvider
{
    [SerializeField] private PlayerInputCore _input;

    public Vector3 MoveWorld =>
        new Vector3(_input.MoveInput.x, 0f, _input.MoveInput.y);

    public Vector3 AimWorld
    {
        get
        {
            Vector3 move = MoveWorld;
            if (move.sqrMagnitude > 0.01f)
                return move.normalized;

            return transform.forward;
        }
    }

    public bool AttackPressed { get; private set; }

    private void Awake()
    {
        if (_input == null)
            _input = GetComponent<PlayerInputCore>();
    }

    public void SetAttack(bool value)
    {
        AttackPressed = value;
    }
}
