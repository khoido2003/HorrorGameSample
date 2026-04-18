using System;
using Godot;

public partial class Drawer : Node3D, IInteractable
{
    [Export]
    private AnimationPlayer animationPlayer;

    [Export]
    public Node3D MovingPart;

    private bool isOpen = false;

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
        }
        else
        {
            animationPlayer.Play("open");
        }

        isOpen = !isOpen;
    }
}
