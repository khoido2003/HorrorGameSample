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

    public override void _Ready()
    {
        PickDestination();
    }

    public override void _Process(double delta)
    {
        if (destination != null)
        {
            float lerpWeight = 0.5f;

            double lookDir = Mathf.LerpAngle(
                Mathf.DegToRad(GlobalRotation.Y),
                Mathf.Atan2(-Velocity.X, -Velocity.Z),
                lerpWeight
            );

            var rot = GlobalRotationDegrees;
            rot.Y = (float)Mathf.RadToDeg(lookDir);
            GlobalRotationDegrees = rot;

            UpdateTargetLocation(destination.GlobalTransform.Origin);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsOnFloor())
        {
            Velocity += GetGravity() * (float)delta;
        }

        if (destination != null)
        {
            var currentLocation = GlobalTransform.Origin;

            var nextLocation = navigationAgent3D.GetNextPathPosition();

            var newVelocity = (nextLocation - currentLocation).Normalized() * speed;

            navigationAgent3D.SetVelocityForced(newVelocity);
        }
    }

    private void PickDestination()
    {
        destination = PatrolDestination[rng.RandiRange(0, PatrolDestination.Count - 1)];
    }

    private void UpdateTargetLocation(Vector3 targetLocation)
    {
        navigationAgent3D.TargetPosition = targetLocation;
    }
}
