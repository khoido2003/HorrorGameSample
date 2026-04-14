using System;
using Godot;

public partial class Door : Node, IInteractable
{
    [Export]
    private AnimationPlayer animationPlayer;

    private bool isOpen = false;

    private bool isLocked = false;

    public void Interact()
    {
        GD.Print("Toggle Door now");
        ToggleDoor();
    }

    private void ToggleDoor()
    {
        if (!isOpen && !isLocked)
        {
            animationPlayer.Play("DoorOpen");
            isOpen = true;
        }
        else
        {
            animationPlayer.PlayBackwards("DoorOpen");
            isOpen = false;
        }
    }

    public void SetIsLocked(bool v)
    {
        isLocked = v;
    }

    public bool GetIsLocked()
    {
        return isLocked;
    }
}
