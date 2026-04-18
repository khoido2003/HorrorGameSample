using Godot;

public partial class InventorySlot : Panel
{
    [Export]
    private TextureRect icon;

    [Export]
    private Label amount;

    [Export]
    private Panel border;

    [Export]
    private StyleBoxFlat normalStyle;

    [Export]
    private StyleBoxFlat selectedStyle;

    [Export]
    private StyleBoxFlat borderNormal;

    [Export]
    private StyleBoxFlat borderSelected;

    public void SetData(ItemResource resource, int count, bool selected)
    {
        icon.Texture = resource?.Icon;
        icon.Visible = resource != null;

        amount.Visible = resource != null && count > 1;
        if (amount.Visible)
        {
            amount.Text = $"×{count}";
        }

        SetSelected(selected);
    }

    public void SetSelected(bool selected)
    {
        AddThemeStyleboxOverride("panel", selected ? selectedStyle : normalStyle);

        border.AddThemeStyleboxOverride("panel", selected ? borderSelected : borderNormal);

        QueueRedraw();
    }

    public void Clear()
    {
        icon.Texture = null;
        icon.Visible = false;
        amount.Visible = false;
        SetSelected(false);
    }
}
