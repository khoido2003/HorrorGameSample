using System;
using Godot;

public partial class EnemyVisual : Node3D
{
    [Export]
    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        PlayAnimation("idle");
    }

    public void PlayAnimation(string animation)
    {
        animationPlayer.Play(animation);
    }
}
