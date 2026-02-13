using Subsystems.CoreComponents;
using UnityEngine;

public class PlayerInputProvider : MonoBehaviour, ICharacterInputProvider
{
    [SerializeField] private PlayerInputCore _input;
public Vector3 MoveWorld
{
    get
    {
        Vector2 input = _input.MoveInput;

        if (input.sqrMagnitude < 0.01f)
            return Vector3.zero;

        Transform cam = Camera.main.transform;

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * input.y + right * input.x;
        return move.normalized;
    }
}

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
