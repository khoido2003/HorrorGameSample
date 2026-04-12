using System;
using Godot;

public partial class Door : Node, IInteractable
{
    [Export]
    private AnimationPlayer animationPlayer;

    private bool isOpen = false;

    public void Interact()
    {
        GD.Print("Toggle Door now");
        ToggleDoor();
    }

    private void ToggleDoor()
    {
        if (!isOpen)
        {
            animationPlayer.Play("DoorOpen");
            isOpen = true;
        }
        else
        {
            animationPlayer.Play("DoorClose");
            isOpen = false;
        }
    }
}
