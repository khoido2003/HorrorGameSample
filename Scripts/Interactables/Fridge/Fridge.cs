using System;
using Godot;

public partial class Fridge : Node3D, IInteractable
{
    [Export]
    private AnimationPlayer animationPlayer;

    [Export]
    private bool isOpen = false;

    public void Interact(Player player)
    {
        ToggleDoor();
    }

    private void ToggleDoor()
    {
        if (!isOpen)
        {
            animationPlayer.Play("open");
        }
        else
        {
            animationPlayer.PlayBackwards("open");
        }

        isOpen = !isOpen;
    }
}
