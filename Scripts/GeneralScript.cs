using System;
using Godot;

public partial class GeneralScript : Node
{
    [Export]
    StandardMaterial3D painting;

    [Export]
    MeshInstance3D plane;

    public override void _Ready()
    {
        if (plane == null)
        {
            plane = GetNode<MeshInstance3D>("Plane");
        }

        plane.MaterialOverride = painting;
    }
}
