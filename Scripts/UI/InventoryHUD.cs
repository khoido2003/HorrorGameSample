using Godot;

public partial class InventoryHUD : CanvasLayer
{
    [Export]
    private PlayerInventory inventory;

    [Export]
    private HBoxContainer slotContainer;

    [Export]
    private Label heldLabel;

    [Export]
    private PackedScene slotScene;

    [Export]
    private int maxSlots = 8;

    public override void _Ready()
    {
        Visible = false;

        for (int i = 0; i < maxSlots; i++)
        {
            var slot = slotScene.Instantiate<InventorySlot>();
            slotContainer.AddChild(slot);
        }

        inventory.InventoryChanged += Rebuild;
        inventory.HeldItemChanged += OnHeldChanged;
        Rebuild();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("inventory"))
        {
            Visible = !Visible;
        }

        if (Input.IsActionJustPressed("inventory_next"))
        {
            inventory.CycleHeld(1);
        }
        else if (Input.IsActionJustPressed("inventory_prev"))
        {
            inventory.CycleHeld(-1);
        }
    }

    private void Rebuild()
    {
        var items = inventory.GetItems();
        var slots = slotContainer.GetChildren();

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] is not InventorySlot slot)
            {
                continue;
            }

            if (i < items.Count)
            {
                slot.SetData(items[i].resource, items[i].amount, i == inventory.HeldIndex);
            }
            else
            {
                slot.Clear();
            }
        }

        RefreshHeldLabel();
    }

    private void OnHeldChanged(ItemResource _)
    {
        var slots = slotContainer.GetChildren();
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] is InventorySlot slot)
            {
                slot.SetSelected(i == inventory.HeldIndex);
            }
        }
        RefreshHeldLabel();
    }

    private void RefreshHeldLabel()
    {
        var held = inventory.GetHeldItem();
        heldLabel.Text = held?.DisplayName ?? "—";
    }
}
