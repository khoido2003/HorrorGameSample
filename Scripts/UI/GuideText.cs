using System;
using Godot;

public partial class GuideText : Control
{
    [Export]
    private RichTextLabel textGuide;

    [Export]
    private float displayTimer = 2f;

    private Tween tween;

    public override void _EnterTree()
    {
        Visible = false;
        Modulate = new Color(1, 1, 1, 0);

        if (textGuide != null)
            textGuide.Text = "";
    }

    public async void ShowText(string text)
    {
        if (textGuide == null)
        {
            GD.PrintErr("GuideText: textGuide not assigned!");
            return;
        }

        textGuide.Text = text;

        Visible = true;

        // Fade in
        tween?.Kill();
        tween = CreateTween();
        tween.TweenProperty(this, "modulate:a", 1f, 0.2f);

        // Wait
        await ToSignal(GetTree().CreateTimer(displayTimer), "timeout");

        // Fade out
        tween = CreateTween();
        tween.TweenProperty(this, "modulate:a", 0f, 0.5f);

        await ToSignal(tween, "finished");

        Visible = false;
    }
}
