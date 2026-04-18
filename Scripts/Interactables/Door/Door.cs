using System;
using Godot;

public partial class Door : Node, IInteractable
{
    [Export]
    private AnimationPlayer animationPlayer;

    [Export]
    private bool isOpen = false;

    [Export]
    private bool isLocked = false;

    [Export]
    private LockId lockId = LockId.None;

    public void Interact(Player player)
    {
        if (isLocked)
        {
            if (!player.PlayerInventory.TryInteractWith(this, player))
            {
                GD.Print("Door is locked.");
                return;
            }
        }

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
            animationPlayer.PlayBackwards("DoorOpen");
            isOpen = false;
        }
    }

    public void SetIsLocked(bool v) => isLocked = v;

    public bool GetIsLocked() => isLocked;

    public LockId GetLockId() => lockId;
}
