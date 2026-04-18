using Godot;

public partial class PlayerZoom : Node3D
{
    [Export]
    private Camera3D camera;

    [Export]
    private float normalFov = 75f;

    [Export]
    private float zoomFov = 30f;

    [Export]
    private float zoomSpeed = 10f;

    private float currentFov;

    public override void _Ready()
    {
        currentFov = normalFov;
        camera.Fov = currentFov;
    }

    public override void _Process(double delta)
    {
        float targetFov = Input.IsActionPressed("zoom") ? zoomFov : normalFov;

        currentFov = Mathf.Lerp(currentFov, targetFov, (float)delta * zoomSpeed);
        camera.Fov = currentFov;
    }
}
