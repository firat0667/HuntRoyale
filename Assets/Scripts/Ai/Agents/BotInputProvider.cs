using UnityEngine;

public class BotInputProvider : MonoBehaviour, ICharacterInputProvider
{
    public Vector3 MoveWorld { get; private set; }
    public Vector3 AimWorld { get; private set; }
    public bool AttackPressed { get; private set; }

    public void SetMove(Vector3 dir) => MoveWorld = dir;
    public void SetAim(Vector3 dir) => AimWorld = dir;
    public void SetAttack(bool v) => AttackPressed = v;
}
