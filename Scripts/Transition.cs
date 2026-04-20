using System.Threading.Tasks;
using Godot;

public partial class Transition : CanvasLayer
{
    [Export]
    private ColorRect fadeRect;

    public override void _Ready() { }

    public async Task FadeOut()
    {
        fadeRect.Modulate = new Color(0, 0, 0, 0);

        var tween = CreateTween();
        tween.TweenProperty(fadeRect, "modulate:a", 1f, 1.0f);

        await ToSignal(tween, Tween.SignalName.Finished);
    }
}
