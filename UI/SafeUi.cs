using System;
using Godot;

public partial class SafeUi : CanvasLayer
{
    [Export]
    private AnimationPlayer animationPlayer;

    [Export]
    private LineEdit passwordInput;

    [Export]
    private Button backButton;

    [Export]
    private Button confirmButton;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        Visible = false;

        backButton.Pressed += OnBackPressed;
    }

    public void OpenSafePassword()
    {
        Visible = true;

        Input.MouseMode = Input.MouseModeEnum.Visible;
        GetTree().Paused = true;

        passwordInput.GrabFocus();
    }

    public void ExitSafe()
    {
        GetTree().Paused = false;
        Visible = false;

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    private void OnBackPressed()
    {
        ExitSafe();
    }
}
