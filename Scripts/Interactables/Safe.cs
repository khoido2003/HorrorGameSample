using System;
using Godot;

public partial class Safe : Node3D, IInteractable
{
    [Export]
    private SafeUi safeUi;

    public void Interact()
    {
        safeUi.OpenSafePassword();
    }
}
