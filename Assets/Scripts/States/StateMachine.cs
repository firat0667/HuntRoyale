using UnityEngine;

public class StateMachine
{
    public IState CurrentState { get; private set; }
    public IState PreviousState { get; private set; }

    private bool _isTransitioning = false;

    public void ChangeState(IState newState)
    {
        if (_isTransitioning || newState == CurrentState)
            return;

        _isTransitioning = true;

  
        CurrentState?.Exit();
        PreviousState = CurrentState;

        
        CurrentState = newState;
        CurrentState.Enter();

        _isTransitioning = false;
    }

    public void LogicUpdate()
    {
        if (_isTransitioning)
            return;

        CurrentState?.LogicUpdate();
    }

    public void PhysicsUpdate()
    {
        if (_isTransitioning)
            return;

        CurrentState?.PhysicsUpdate();
    }

    public void RevertToPreviousState()
    {
        if (PreviousState != null)
            ChangeState(PreviousState);
    }
}
