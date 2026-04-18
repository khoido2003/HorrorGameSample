using Godot;

public partial class PlayerFlashlight : SpotLight3D
{
    [Export]
    private Node3D target;

    [Export]
    private float positionLerpSpeed = 10f;

    [Export]
    private float rotationLerpSpeed = 10f;

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("flashlight"))
        {
            Visible = !Visible;
        }

        if (target == null)
        {
            GD.PrintErr("Missing target node");
            return;
        }

        float tPos = (float)delta * positionLerpSpeed;
        float tRot = (float)delta * rotationLerpSpeed;

        // Smooth position
        GlobalPosition = GlobalPosition.Lerp(target.GlobalPosition, tPos);

        // Smooth rotation (IMPORTANT: use Quaternion for smoothness)
        Quaternion currentRot = GlobalTransform.Basis.GetRotationQuaternion();
        Quaternion targetRot = target.GlobalTransform.Basis.GetRotationQuaternion();

        Quaternion newRot = currentRot.Slerp(targetRot, tRot);

        Transform3D t = GlobalTransform;
        t.Basis = new Basis(newRot);
        GlobalTransform = t;
    }
}
