using System;
using Godot;

public partial class SceneChangedTrigger : Area3D
{
    [Export]
    private PackedScene scene;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is not Player)
        {
            return;
        }

        if (scene == null)
        {
            GD.PrintErr("Scene  is not set!");
            return;
        }

        GetTree().ChangeSceneToPacked(scene);
    }
}
