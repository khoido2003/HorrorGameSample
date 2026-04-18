using System;
using Godot;

public partial class LightSwitch : Node3D, IInteractable
{
    [Export]
    private bool isOn = false;

    [Export]
    private StandardMaterial3D onMaterial3D;

    [Export]
    private StandardMaterial3D offMaterial3D;

    [Export]
    private MeshInstance3D onSwitch;

    [Export]
    private MeshInstance3D offSwitch;

    [Export]
    private Node3D lightBulb;

    public void Interact(Player player)
    {
        ToggleLight();
    }

    public override void _Ready()
    {
        if (!isOn)
        {
            SetLightBulbOnOff(offMaterial3D, isOn);

            onSwitch.Hide();
        }
        else
        {
            SetLightBulbOnOff(onMaterial3D, isOn);

            offSwitch.Hide();
        }
    }

    private void ToggleLight()
    {
        if (!isOn)
        {
            isOn = true;

            SetLightBulbOnOff(onMaterial3D, isOn);

            offSwitch.Hide();
            onSwitch.Show();
        }
        else
        {
            isOn = false;

            SetLightBulbOnOff(offMaterial3D, isOn);

            offSwitch.Show();
            onSwitch.Hide();
        }
    }

    private void SetLightBulbOnOff(StandardMaterial3D material3D, bool isOn)
    {
        lightBulb.GetNode<MeshInstance3D>("Light").MaterialOverride = material3D;

        lightBulb.GetNode<OmniLight3D>("OmniLight3D").Visible = isOn;
    }
}
