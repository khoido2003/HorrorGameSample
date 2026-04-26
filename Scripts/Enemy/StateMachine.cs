using Godot;

public class StateMachine
{
    public IState CurrentState { get; private set; }

    public void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }

    public void Update(double delta)
    {
        CurrentState?.Update(delta);
    }

    public void PhysicsUpdate(double delta)
    {
        CurrentState?.PhysicsUpdate(delta);
    }
}
