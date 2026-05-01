using System;
using Godot;

public partial class Door : Node, IInteractable
{
    [Export]
    private AnimationPlayer animationPlayer;

    [Export]
    private Area3D AITrigger;

    [Export]
    private bool isOpen = false;

    [Export]
    private bool isLocked = false;

    [Export]
    private LockId lockId = LockId.None;

    [Export]
    private AudioStreamPlayer3D doorOpenSound;

    [Export]
    private AudioStreamPlayer3D doorCloseSound;

    public override void _Ready()
    {
        if (AITrigger == null)
        {
            AITrigger = GetNode<Area3D>("AITrigger");
        }
        AITrigger.BodyEntered += OnBodyEntered;
    }

    public void Interact(Player player)
    {
        if (isLocked)
        {
            if (!player.PlayerInventory.TryInteractWith(this, player))
            {
                GD.Print("Door is locked.");

                player.guideText?.ShowText("Need a key");

                return;
            }
        }

        ToggleDoor();
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Enemy)
        {
            // if (isLocked)
            //     return;

            if (!isOpen)
            {
                ToggleDoor();
            }
        }
    }

    private void ToggleDoor()
    {
        if (!isOpen)
        {
            animationPlayer.Play("DoorOpen");
            isOpen = true;
            doorOpenSound.Play();
        }
        else
        {
            animationPlayer.PlayBackwards("DoorOpen");
            isOpen = false;
            doorCloseSound.Play();
        }
    }

    public void SetIsLocked(bool v) => isLocked = v;

    public bool GetIsLocked() => isLocked;

    public LockId GetLockId() => lockId;
}
