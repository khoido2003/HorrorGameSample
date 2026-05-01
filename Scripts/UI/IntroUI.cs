using Godot;

public partial class IntroUI : Control
{
    [Export]
    public string NextScene = "res://scenes/MainMenu.tscn";

    [Export]
    private RichTextLabel _studioLabel;

    [Export]
    private RichTextLabel _gameTitle;

    public override void _Ready()
    {
        // Start hidden
        _studioLabel.Modulate = new Color(1, 1, 1, 0);
        _gameTitle.Modulate = new Color(1, 1, 1, 0);

        RunIntro();
    }

    private async void RunIntro()
    {
        // 1. Fade studio label in (0.8s)
        await FadeTo(_studioLabel, 1f, 0.8);

        // 2. Hold for 5s
        await ToSignal(GetTree().CreateTimer(5.0), "timeout");

        // 3. Fade studio label out (0.6s)
        await FadeTo(_studioLabel, 0f, 0.6);

        // 4. Fade game title in (1.2s)
        await FadeTo(_gameTitle, 1f, 1.2);

        // Title stays on screen — connect intro_finished signal
        // or uncomment below to auto-advance after a delay:
        // await ToSignal(GetTree().CreateTimer(5.0), "timeout");
        // await FadeTo(this, 0f, 1.0);
        // GetTree().ChangeSceneToFile(NextScene);
    }

    private async System.Threading.Tasks.Task FadeTo(
        CanvasItem node,
        float targetAlpha,
        double duration
    )
    {
        var tween = CreateTween();
        tween.SetEase(Tween.EaseType.InOut);
        tween.SetTrans(Tween.TransitionType.Sine);
        tween.TweenProperty(node, "modulate:a", targetAlpha, duration);
        await ToSignal(tween, "finished");
    }
}
