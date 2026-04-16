using System;
using Godot;

public partial class SafeUi : CanvasLayer
{
    [Export]
    private LineEdit passwordInput;

    [Export]
    private Button backButton;

    [Export]
    private Button confirmButton;

    private string currentPassword;

    public bool IsOpen => Visible;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        Visible = false;

        backButton.Pressed += OnBackPressed;
        confirmButton.Pressed += OnConfirmPressed;
    }

    private void OnConfirmPressed()
    {
        if (passwordInput.Text == currentPassword)
        {
            GD.Print("Correct password!");
            ExitSafe();

            // TODO: open safe animation, give loot, etc.
        }
        else
        {
            GD.Print("Wrong password!");
            passwordInput.Text = "";
        }
    }

    public void OpenSafePassword(string password)
    {
        currentPassword = password;
        Visible = true;

        Input.MouseMode = Input.MouseModeEnum.Visible;
        GetTree().Paused = true;

        passwordInput.Text = "";
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
