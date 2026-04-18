using Godot;

public enum LockId
{
    None,
    Basement,
}

[GlobalClass]
public partial class KeyItem : ItemResource
{
    [Export]
    private LockId lockId;

    public override bool TryInteract(Node target, Player player)
    {
        if (target is Door door)
        {
            if (!door.GetIsLocked())
            {
                return false;
            }

            if (lockId == LockId.None)
            {
                return false;
            }

            if (door.GetLockId() != lockId)
            {
                return false;
            }
            GD.Print("Unlocked door with key: " + DisplayName);

            door.SetIsLocked(false);

            // decide if key is consumed
            player.PlayerInventory.RemoveItem(this, 1);

            return true;
        }

        return false;
    }
}
