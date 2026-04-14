using System;
using Godot;

public partial class DoorBell : Node3D
{
    [Export]
    private AnimationPlayer animationPlayer;

    [Export]
    private Door door;

    private float timeRung = 0;

    public override void _Ready()
    {
        door.SetIsLocked(true);
    }

    private void RingBell()
    {
        if (animationPlayer.CurrentAnimation != "click" && timeRung < 2)
        {
            timeRung += 1;
            animationPlayer.Play("click");
        }
        else
        {
            door.SetIsLocked(false);
            animationPlayer.PlayBackwards("click");
        }
    }
}
