using Godot;

public class WaitState : IState
{
    private Enemy enemy;
    private float waitTime;
    private RandomNumberGenerator rng = new();

    public WaitState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        waitTime = rng.RandfRange(1f, 3f);
        enemy.Velocity = Vector3.Zero;
    }

    public void Exit() { }

    public void Update(double delta)
    {
        waitTime -= (float)delta;

        if (waitTime <= 0)
        {
            enemy.FSM.ChangeState(enemy.PatrolState);
        }
    }

    public void PhysicsUpdate(double delta)
    {
        enemy.Velocity = Vector3.Zero;
    }
}
