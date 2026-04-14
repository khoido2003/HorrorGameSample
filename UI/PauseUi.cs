using System;
using Godot;

public partial class PauseUi : CanvasLayer
{
    [Export]
    private Button _resumeButton;

    [Export]
    private Button _quitButton;

    [Export]
    private Button _menuButton;

    [Export]
    private Button _settingsButton;

    [Export]
    private Button _controlsButton;

    public override void _Ready()
    {
        Visible = false;
        SetProcessInput(true);
        ProcessMode = ProcessModeEnum.Always;

        _resumeButton.Pressed += ResumeGame;
        _quitButton.Pressed += QuitGame;
        _menuButton.Pressed += ReturnToMenu;
        _settingsButton.Pressed += OpenSettings;
        _controlsButton.Pressed += OpenControls;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("pause"))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        bool isPaused = !GetTree().Paused;
        GetTree().Paused = isPaused;

        Visible = isPaused;

        Input.MouseMode = isPaused ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
    }

    public void ResumeGame()
    {
        GetTree().Paused = false;
        Visible = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public void QuitGame()
    {
        GetTree().Quit();
    }

    private void OpenControls()
    {
        throw new NotImplementedException();
    }

    private void OpenSettings()
    {
        throw new NotImplementedException();
    }

    private void ReturnToMenu()
    {
        throw new NotImplementedException();
    }
}
