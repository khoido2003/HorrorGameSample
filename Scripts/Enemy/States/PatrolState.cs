using Godot;
using Godot.Collections;

public class PatrolState : IState
{
    private Enemy enemy;
    private int index = -1;
    private RandomNumberGenerator rng = new();

    public PatrolState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        PickNextPoint();
    }

    public void Exit() { }

    public void Update(double delta)
    {
        DetectPlayer();
    }

    public void PhysicsUpdate(double delta)
    {
        enemy.MoveAlongPath();

        if (enemy.NavAgent.IsNavigationFinished())
        {
            enemy.FSM.ChangeState(enemy.WaitState);
        }
    }

    private void PickNextPoint()
    {
        if (enemy.PatrolPoints.Count == 0)
        {
            return;
        }

        int next;
        do
        {
            next = rng.RandiRange(0, enemy.PatrolPoints.Count - 1);
        } while (enemy.PatrolPoints.Count > 1 && next == index);

        index = next;
        enemy.MoveTo(enemy.PatrolPoints[index].GlobalPosition);
    }

    private void DetectPlayer()
    {
        if (!HasPlayer())
        {
            return;
        }

        if (!IsPlayerInRange())
        {
            return;
        }

        if (!IsPlayerInFront())
        {
            return;
        }

        if (!HasLineOfSight())
        {
            return;
        }

        enemy.FSM.ChangeState(enemy.ChaseState);
    }

    private bool HasPlayer()
    {
        return enemy.Player != null;
    }

    private bool IsPlayerInRange()
    {
        float distance = enemy.GlobalPosition.DistanceTo(enemy.Player.GlobalPosition);
        return distance <= 20f;
    }

    private bool IsPlayerInFront()
    {
        Vector3 toPlayer = (enemy.Player.GlobalPosition - enemy.GlobalPosition).Normalized();
        Vector3 forward = -enemy.GlobalTransform.Basis.Z;

        float dot = forward.Dot(toPlayer);

        return dot >= 0.7f;
    }

    private bool HasLineOfSight()
    {
        PhysicsDirectSpaceState3D space = enemy.GetWorld3D().DirectSpaceState;

        PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(
            enemy.GlobalPosition,
            enemy.Player.GlobalPosition
        );

        Dictionary result = space.IntersectRay(query);

        if (result.Count == 0)
        {
            return false;
        }

        Node collider = result["collider"].As<Node>();

        return collider == enemy.Player || collider.GetParent() == enemy.Player;
    }
}
