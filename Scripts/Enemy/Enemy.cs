using System;
using System.Linq;
using Godot;

public partial class Enemy : CharacterBody3D
{
    public enum State
    {
        Patrolling,
        Chasing,
        Waiting,
    }

    [ExportGroup("Movement Settings")]
    [Export]
    private float speed = 3.0f;

    [Export]
    private float rotationSpeed = 5.0f;

    [ExportGroup("Navigation")]
    [Export]
    private Godot.Collections.Array<Node3D> PatrolDestination = new();

    [Export]
    private NavigationAgent3D navigationAgent3D;

    [ExportGroup("Detection")]
    [Export]
    private Godot.Collections.Array<RayCast3D> rayCastList = new();

    [Export]
    private float maxChaseTimeWithoutLosingSight = 10.0f;

    private State currentState = State.Patrolling;
    private Node3D currentDestination;
    private int currentPatrolIndex = -1;
    private RandomNumberGenerator rng = new();

    private float stateTimer = 0.0f;
    private Player detectedPlayer;

    public override void _Ready()
    {
        FloorMaxAngle = Mathf.DegToRad(120.0f);

        if (navigationAgent3D != null)
        {
            navigationAgent3D.NavigationFinished += OnReachedDestination;
            navigationAgent3D.PathDesiredDistance = 2.0f;
            navigationAgent3D.TargetDesiredDistance = 2.0f;
        }

        ChangeState(State.Patrolling);
    }

    public override void _Process(double delta)
    {
        HandleRotation((float)delta);
        UpdateTimers((float)delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        CheckForPlayer();

        switch (currentState)
        {
            case State.Patrolling:
                MoveTowardsTarget();
                break;
            case State.Chasing:
                UpdateChaseTarget();
                MoveTowardsTarget();
                break;
            case State.Waiting:
                Velocity = Vector3.Zero;
                break;
        }

        MoveAndSlide();
        if (navigationAgent3D != null)
        {
            navigationAgent3D.Velocity = Velocity;
        }
    }

    private void ChangeState(State newState)
    {
        currentState = newState;
        stateTimer = 0.0f;

        switch (newState)
        {
            case State.Patrolling:
                PickNextPatrolPoint();
                break;
            case State.Chasing:
                // Start chasing immediately
                UpdateChaseTarget();
                break;
            case State.Waiting:
                Velocity = Vector3.Zero;
                stateTimer = rng.RandfRange(1.0f, 10.0f);
                break;
        }
    }

    private void UpdateTimers(float delta)
    {
        if (currentState == State.Waiting)
        {
            stateTimer -= delta;
            if (stateTimer <= 0)
            {
                ChangeState(State.Patrolling);
            }
        }
        else if (currentState == State.Chasing)
        {
            stateTimer += delta;
            if (stateTimer > maxChaseTimeWithoutLosingSight)
            {
                ChangeState(State.Patrolling);
            }
        }
    }

    private void CheckForPlayer()
    {
        foreach (var raycast in rayCastList)
        {
            if (raycast.IsColliding() && raycast.GetCollider() is Player playerNode)
            {
                detectedPlayer = playerNode;
                if (currentState != State.Chasing)
                {
                    ChangeState(State.Chasing);
                }
                else
                {
                    // Reset chase timer while player is in sight
                    stateTimer = 0.0f;
                }
                return;
            }
        }
    }

    private void UpdateChaseTarget()
    {
        if (detectedPlayer != null && navigationAgent3D != null)
        {
            currentDestination = detectedPlayer;
            navigationAgent3D.TargetPosition = currentDestination.GlobalPosition;
        }
    }

    private void PickNextPatrolPoint()
    {
        if (PatrolDestination.Count == 0 || navigationAgent3D == null)
            return;

        int nextIndex;
        do
        {
            nextIndex = rng.RandiRange(0, PatrolDestination.Count - 1);
        } while (PatrolDestination.Count > 1 && nextIndex == currentPatrolIndex);

        currentPatrolIndex = nextIndex;
        currentDestination = PatrolDestination[currentPatrolIndex];
        navigationAgent3D.TargetPosition = currentDestination.GlobalPosition;
    }

    private void MoveTowardsTarget()
    {
        if (currentDestination == null || navigationAgent3D == null)
            return;

        Vector3 nextPathPos = navigationAgent3D.GetNextPathPosition();
        Vector3 direction = (nextPathPos - GlobalPosition);
        direction.Y = 0;

        if (direction.LengthSquared() > 0.01f)
        {
            Velocity = direction.Normalized() * speed;
        }
        else
        {
            Velocity = Vector3.Zero;
        }
    }

    private void HandleRotation(float delta)
    {
        if (Velocity.LengthSquared() > 0.01f)
        {
            float targetAngle = Mathf.Atan2(-Velocity.X, -Velocity.Z);
            Rotation = new Vector3(
                Rotation.X,
                Mathf.LerpAngle(Rotation.Y, targetAngle, delta * rotationSpeed),
                Rotation.Z
            );
        }
    }

    public void OnReachedDestination()
    {
        if (currentState == State.Patrolling)
        {
            ChangeState(State.Waiting);
        }
    }
}
