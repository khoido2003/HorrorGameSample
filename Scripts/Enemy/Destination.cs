using System;
using Godot;

public partial class Destination : Area3D
{
    private RandomNumberGenerator rng = new();

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private async void OnBodyEntered(Node body)
    {
        if (body is Enemy enemy)
        {
            var timer = GetTree().CreateTimer(rng.RandfRange(1.0f, 10.0f), false);
            await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);

            enemy.OnReachedDestination();
        }
    }
}
