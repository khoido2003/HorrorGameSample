using System;
using Godot;

public partial class Destination : Area3D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body) { }
}
