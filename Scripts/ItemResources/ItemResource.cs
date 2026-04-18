using Godot;

public enum ItemId
{
    None,
    Key,
}

public partial class ItemResource : Resource
{
    [Export]
    public ItemId Id;

    [Export]
    public string DisplayName;

    [Export]
    public PackedScene WorldScene;

    [Export]
    public PackedScene HandScene;

    [Export]
    public Texture2D Icon;

    public virtual bool TryInteract(Node target, Player player)
    {
        return false;
    }

    public virtual void Use(Player player)
    {
        GD.Print("Using item: " + DisplayName);
    }
}
