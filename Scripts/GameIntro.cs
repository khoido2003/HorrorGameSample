using System;
using System.Threading.Tasks;
using Godot;

public partial class GameIntro : Node
{
    [Export]
    private AnimationPlayer animationPlayer;

    [Export]
    private PackedScene transitionScene;

    public override void _Ready()
    {
        animationPlayer.AnimationFinished += OnAnimationFinished;
        animationPlayer.Play("cutscene");
    }

    private async void OnAnimationFinished(StringName animName)
    {
        if (animName == "cutscene")
        {
            var transition = transitionScene.Instantiate<Transition>();
            GetTree().Root.AddChild(transition);
            await transition.FadeOut();

            GetTree().ChangeSceneToFile("res://GameScenes/level.tscn");
        }
    }
}
