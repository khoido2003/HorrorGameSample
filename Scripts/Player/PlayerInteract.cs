using System;
using Godot;

public partial class PlayerInteract : RayCast3D
{
    [Export]
    private TextureRect crosshair;

    [Export]
    private SafeUi safeUi;

    public override void _PhysicsProcess(double delta)
    {
        if (IsColliding())
        {
            var interactable = FindInteractable(GetCollider() as Node);

            if (interactable != null && !crosshair.Visible)
            {
                crosshair.Visible = true;
            }

            if (interactable != null && Input.IsActionJustPressed("interact"))
            {
                interactable.Interact();
            }
        }
        else
        {
            crosshair.Visible = false;
        }
    }

    private IInteractable FindInteractable(Node node)
    {
        while (node != null)
        {
            if (node is IInteractable interactable)
                return interactable;

            node = node.GetParent();
        }

        return null;
    }
}
