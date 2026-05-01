using System;
using Godot;

public partial class Drawer : Node3D, IInteractable
{
    [Export]
    private AnimationPlayer animationPlayer;

    [Export]
    public Node3D MovingPart;

    private bool isOpen = false;

    [Export]
    private AudioStreamPlayer3D doorOpenSound;

    [Export]
    private AudioStreamPlayer3D doorCloseSound;

    public void Interact(Player player)
    {
        GD.Print("Interact");
        ToggleDrawer();
    }

    private void ToggleDrawer()
    {
        if (isOpen)
        {
            animationPlayer.Play("close");
            doorCloseSound.Play();
        }
        else
        {
            animationPlayer.Play("open");
            doorOpenSound.Play();
        }

        isOpen = !isOpen;
    }
}
