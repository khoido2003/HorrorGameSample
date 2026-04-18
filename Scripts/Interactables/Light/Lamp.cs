using System;
using Godot;

public partial class Lamp : Node3D, IInteractable
{
    [Export]
    private bool isOn = false;

    [Export]
    private StandardMaterial3D onMat;

    [Export]
    private StandardMaterial3D offMat;

    [Export]
    private Color lightColor;

    [Export]
    private MeshInstance3D lampHead;

    [Export]
    private OmniLight3D omniLight3D;

    public override void _Ready()
    {
        omniLight3D.LightColor = lightColor;
        SetLightOnOff();
    }

    public void Interact(Player player)
    {
        GD.Print("Lamp interact");
        ToggleLight();
    }

    private void ToggleLight()
    {
        isOn = !isOn;

        SetLightOnOff();
    }

    private void SetLightOnOff()
    {
        if (isOn)
        {
            lampHead.MaterialOverride = onMat;
            omniLight3D.Visible = isOn;
        }
        else
        {
            lampHead.MaterialOverride = offMat;
            omniLight3D.Visible = isOn;
        }
    }
}
