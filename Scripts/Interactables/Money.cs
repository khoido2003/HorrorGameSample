using System;
using Godot;

public partial class Money : Node3D, IInteractable
{
    [Export]
    private TaskUI taskUI;

    public void Interact(Player player)
    {
        // Update the task
        if (taskUI != null)
        {
            taskUI.SetTask("Escape the house");
        }
        else
        {
            GD.PrintErr("TaskUI is not assigned on Money!");
        }

        QueueFree();
    }
}
