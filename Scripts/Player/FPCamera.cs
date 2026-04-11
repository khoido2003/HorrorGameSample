using System;
using Godot;

public partial class FPCamera : Node3D
{
    private float sensitivity = 0.2f;

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion motion)
        {
            // Left/Right
            var player = GetParent<CharacterBody3D>();

            player.RotateY(Mathf.DegToRad(-motion.Relative.X * sensitivity));

            // Rotate the Head look up/down
            //RotateX(Mathf.DegToRad(-motion.Relative.Y * sensitivity));

            Vector3 rot = Rotation;

            rot.X += Mathf.DegToRad(-motion.Relative.Y * sensitivity);

            rot.X = Mathf.Clamp(rot.X, Mathf.DegToRad(-90), Mathf.DegToRad(90));

            Rotation = rot;
        }
    }
}
