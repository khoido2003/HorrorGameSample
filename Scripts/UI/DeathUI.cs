using Godot;

public partial class DeathUI : Control
{
    [Export]
    private Button restartButton;

    public override void _Ready()
    {
        Hide();
        restartButton.Pressed += OnRestartPressed;
    }

    public void ShowDeathScreen()
    {
        Show();
    }

    private void OnRestartPressed()
    {
        GetTree().ReloadCurrentScene();
    }
}
