using System;
using Godot;

public partial class Enemy : CharacterBody3D
{
    [Export]
    public float Speed = 3f;

    [Export]
    public float RotationSpeed = 5f;

    [Export]
    public NavigationAgent3D NavAgent;

    [Export]
    public Godot.Collections.Array<Node3D> PatrolPoints;

    [Export]
    public Player Player;

    public StateMachine FSM;

    // States
    public PatrolState PatrolState;
    public ChaseState ChaseState;
    public WaitState WaitState;

    public override void _Ready()
    {
        FSM = new StateMachine();

        PatrolState = new PatrolState(this);
        ChaseState = new ChaseState(this);
        WaitState = new WaitState(this);

        FSM.ChangeState(PatrolState);
    }

    public override void _Process(double delta)
    {
        FSM.Update(delta);
        HandleRotation((float)delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        FSM.PhysicsUpdate(delta);
        MoveAndSlide();

        if (NavAgent != null)
        {
            NavAgent.Velocity = Velocity;
        }
    }

    public void MoveTo(Vector3 target)
    {
        if (NavAgent == null)
        {
            return;
        }
        NavAgent.TargetPosition = target;
    }

    public void MoveAlongPath()
    {
        if (NavAgent == null)
        {
            return;
        }

        if (NavAgent.IsNavigationFinished())
        {
            Velocity = Vector3.Zero;
            return;
        }

        Vector3 nextPos = NavAgent.GetNextPathPosition();
        Vector3 dir = (nextPos - GlobalPosition);
        dir.Y = 0;

        Velocity = dir.Normalized() * Speed;
    }

    private void HandleRotation(float delta)
    {
        if (Velocity.LengthSquared() < 0.01f)
        {
            return;
        }

        float targetAngle = Mathf.Atan2(-Velocity.X, -Velocity.Z);
        Rotation = new Vector3(
            Rotation.X,
            Mathf.LerpAngle(Rotation.Y, targetAngle, delta * RotationSpeed),
            Rotation.Z
        );
    }
}
