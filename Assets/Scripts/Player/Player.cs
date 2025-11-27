using UnityEngine;

public class Player : MonoBehaviour
{
    public MovementSubsystem Movement { get; private set; }
    public AimSubsystem Aim { get; private set; }
    public HealthSubsystem Health { get; private set; }
    public AttackSubsystem Attack { get; private set; }
    public ScoreSubsystem Score{ get; private set; }

    private void Awake()
    {
        Movement = GetComponentInChildren<MovementSubsystem>();
        Aim = GetComponentInChildren<AimSubsystem>();
        Health = GetComponentInChildren<HealthSubsystem>();
        Attack = GetComponentInChildren<AttackSubsystem>();
        Score = GetComponentInChildren<ScoreSubsystem>();

        //if(Health!= null)
            //Health.OnplayerDied+=OnPlayerDied;
    }
    private void OnPlayerDied()
    {
        EventManager.Instance.Trigger(EventTags.EVENT_PLAYER_DIED);
    }
}
