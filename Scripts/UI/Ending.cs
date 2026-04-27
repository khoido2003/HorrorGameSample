using System;
using System.Threading.Tasks;
using Godot;

public partial class Ending : Control
{
    [Export]
    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        RunEnding();
    }

    private async void RunEnding()
    {
        animationPlayer.Play("fade");

        var timer = GetTree().CreateTimer(8.0);
        await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);

        GetTree().Quit();
    }
}
