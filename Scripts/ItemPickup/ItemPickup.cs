using Godot;

public partial class ItemPickup : Area3D
{
    [Export]
    private ItemResource item;

    private Node3D visual;

    public override void _Ready()
    {
        SpawnVisual();

        BodyEntered += OnBodyEntered;
    }

    private void SpawnVisual()
    {
        if (item?.WorldScene == null)
        {
            return;
        }

        visual = item.WorldScene.Instantiate<Node3D>();
        AddChild(visual);
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body is Player player && item != null)
        {
            GD.Print("Picked up: " + item.DisplayName);

            player.PlayerInventory.AddItem(item);

            QueueFree();
        }
    }
}
