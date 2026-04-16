using System;
using Godot;

public partial class Safe : Node3D, IInteractable
{
    [Export]
    private SafeUi safeUi;

    [Export]
    private AnimationPlayer animationPlayer;

    [Export]
    private SafeCodePaper safeCodePaper;

    private string password;

    public override void _Ready()
    {
        GeneratePassword();
    }

    private void GeneratePassword()
    {
        Random random = new Random();

        // Example: 4-digit code
        password = random.Next(1000, 9999).ToString();

        GD.Print("Safe password: " + password);

        safeCodePaper.SetPassword(password);
    }

    public void Interact()
    {
        safeUi.OpenSafePassword(password);
    }
}
