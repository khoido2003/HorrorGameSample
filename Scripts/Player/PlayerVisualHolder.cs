using Godot;

public partial class PlayerVisualHolder : Node3D
{
    [Export]
    private Node3D rightHand;

    private Node3D currentItemVisual;

    public void Init(PlayerInventory inventory)
    {
        inventory.HeldItemChanged += OnHeldItemChanged;
    }

    private void OnHeldItemChanged(ItemResource item)
    {
        currentItemVisual?.QueueFree();
        currentItemVisual = null;

        if (item?.HandScene == null)
        {
            return;
        }

        currentItemVisual = item.HandScene.Instantiate<Node3D>();
        rightHand.AddChild(currentItemVisual);
    }

    public void Clear()
    {
        currentItemVisual?.QueueFree();
        currentItemVisual = null;
    }
}
