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

    [Export]
    private EnemyVisual enemyVisual;

    [Export]
    private Area3D jumpscareArea;

    [Export]
    private Camera3D jumpscareCamera;

    [Export]
    private DeathUI deathUI;

    private bool hasJumpscared;

    public StateMachine FSM;

    private Vector3 lastPosition;
    private float stuckTimer = 0f;

    // States
    public PatrolState PatrolState;
    public ChaseState ChaseState;
    public WaitState WaitState;
    public JumpScareState JumpScareState;

    public override void _Ready()
    {
        FSM = new StateMachine();

        PatrolState = new PatrolState(this);
        ChaseState = new ChaseState(this);
        WaitState = new WaitState(this);
        JumpScareState = new JumpScareState(this);

        FSM.ChangeState(PatrolState);

        jumpscareArea.BodyEntered += OnJumpscareTriggered;
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

    private void OnJumpscareTriggered(Node3D body)
    {
        if (hasJumpscared)
        {
            GD.Print("Already jumpscared");
            return;
        }

        if (body is Player player)
        {
            hasJumpscared = true;
            FSM.ChangeState(JumpScareState);
        }
        else
        {
            GD.Print("Not player, it's: ", body.GetType());
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
        Vector3 dir = nextPos - GlobalPosition;
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

    private void HandleStuckAtCorner(float delta)
    {
        float movement = GlobalPosition.DistanceTo(lastPosition);

        if (movement < 0.02f)
        {
            stuckTimer += delta;
        }
        else
        {
            stuckTimer = 0f;
        }

        lastPosition = GlobalPosition;

        if (stuckTimer > 1.0f)
        {
            ForceUnstuck();

            stuckTimer = 0f;
        }
    }

    private void ForceUnstuck()
    {
        if (NavAgent == null)
        {
            return;
        }

        // Direction away from obstacle (randomized)
        Vector3 randomDir = new Vector3(GD.RandRange(-1, 1), 0, GD.RandRange(-1, 1)).Normalized();

        // Push enemy slightly
        GlobalPosition += randomDir * 1.5f;

        // Force path refresh properly
        Vector3 target = NavAgent.TargetPosition;
        NavAgent.TargetPosition = GlobalPosition;
        NavAgent.TargetPosition = target;
    }

    public void PlayAnimation(string name)
    {
        enemyVisual.PlayAnimation(name);
    }

    public void ActivateJumpscareCamera()
    {
        jumpscareCamera.Current = true;
    }

    public void ShowDeathUI()
    {
        deathUI.ShowDeathScreen();
    }
}
