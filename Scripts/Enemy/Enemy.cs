using System;
using Godot;

public partial class Enemy : CharacterBody3D
{
    [Export]
    private Godot.Collections.Array<Node3D> PatrolDestination = new();

    [Export]
    private Player player;

    [Export]
    private NavigationAgent3D navigationAgent3D;

    private float speed = 3.0f;

    private RandomNumberGenerator rng = new();

    private Node3D destination;

    private bool isChasing = false;

    /////////////////////////////////////////////////////

    public override void _Ready()
    {
        FloorMaxAngle = Mathf.DegToRad(120.0f);

        navigationAgent3D.NavigationFinished += OnReachedDestination;

        navigationAgent3D.PathDesiredDistance = 2.0f;
        navigationAgent3D.TargetDesiredDistance = 2.0f;

        PickDestination();
    }

    public int DestinationIndex { get; private set; }

    public override void _Process(double delta)
    {
        if (Velocity.LengthSquared() > 0.01f)
        {
            float targetAngle = Mathf.Atan2(-Velocity.X, -Velocity.Z);

            float newAngle = Mathf.LerpAngle(
                Rotation.Y,
                targetAngle,
                (float)(delta * 5.0f) // rotation speed
            );

            Rotation = new Vector3(Rotation.X, newAngle, Rotation.Z);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (destination == null)
        {
            return;
        }

        var currentLocation = GlobalTransform.Origin;
        var nextLocation = navigationAgent3D.GetNextPathPosition();

        var direction = nextLocation - currentLocation;
        direction.Y = 0;

        if (direction.LengthSquared() > 0.0001f)
        {
            direction = direction.Normalized();
            Velocity = direction * speed;
        }
        else
        {
            Velocity = Vector3.Zero;
        }

        MoveAndSlide();

        navigationAgent3D.Velocity = Velocity;
    }

    private void PickDestination(int indexNotChosenDestination = -1)
    {
        int chosenIndex;

        do
        {
            chosenIndex = rng.RandiRange(0, PatrolDestination.Count - 1);
        } while (PatrolDestination.Count > 1 && chosenIndex == indexNotChosenDestination);

        DestinationIndex = chosenIndex;
        destination = PatrolDestination[chosenIndex];

        UpdateTargetLocation();
    }

    private void UpdateTargetLocation()
    {
        navigationAgent3D.TargetPosition = destination.GlobalTransform.Origin;
    }

    public void OnReachedDestination()
    {
        PickDestination(DestinationIndex);
    }
}
