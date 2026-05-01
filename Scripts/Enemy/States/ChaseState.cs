using Godot;

public class ChaseState : IState
{
    private Enemy enemy;
    private float timer;
    private float maxChaseTime = 10f;

    public ChaseState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        timer = 0f;
        enemy.PlayAnimation("walk");
    }

    public void Exit() { }

    public void Update(double delta)
    {
        if (enemy.Player == null)
        {
            enemy.FSM.ChangeState(enemy.PatrolState);
            return;
        }

        float dist = enemy.GlobalPosition.DistanceTo(enemy.Player.GlobalPosition);

        if (dist > 15f)
        {
            timer += (float)delta;

            if (timer > maxChaseTime)
                enemy.FSM.ChangeState(enemy.PatrolState);
        }
        else
        {
            timer = 0f;
        }

        enemy.MoveTo(enemy.Player.GlobalPosition);
    }

    public void PhysicsUpdate(double delta)
    {
        enemy.MoveAlongPath();
    }
}
