using System;
using Godot;

public partial class Closet : Node3D, IInteractable
{
    [Export]
    private AnimationPlayer left;

    [Export]
    private AnimationPlayer right;

    private bool isOpen = false;

    public void Interact()
    {
        ToggleDoor();
    }

    private void ToggleDoor()
    {
        if (isOpen)
        {
            left.PlayBackwards("open");
            right.PlayBackwards("open");
        }
        else
        {
            left.Play("open");
            right.Play("open");
        }

        isOpen = !isOpen;
    }
}
