using System;
using Godot;

public partial class Player : CharacterBody3D
{
    private float speed = 5f;
    private const float JumpVelocity = 4.5f;

    private bool isCrouch = false;

    [Export]
    public GuideText guideText;

    [Export]
    private CollisionShape3D collision;

    [Export]
    private PlayerFlashlight flashlightController;

    [Export]
    private PlayerInventory inventory;

    [Export]
    private PlayerVisualHolder visualHolder;

    private bool isDisabled = false;

    public PlayerVisualHolder VisualHolder
    {
        get { return visualHolder; }
    }

    public PlayerInventory PlayerInventory
    {
        get { return inventory; }
    }

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;

        visualHolder.Init(inventory);
    }

    public override void _Process(double delta)
    {
        if (isDisabled)
            return;

        if (Input.IsActionJustPressed("crouch"))
        {
            isCrouch = !isCrouch;

            if (isCrouch)
            {
                speed = 2.5f;
            }
            else
            {
                speed = 5f;
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (isDisabled)
        {
            Velocity = Vector3.Zero;
            MoveAndSlide();
            return;
        }

        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        // Handle Crouch
        if (collision.Shape is CapsuleShape3D capsule)
        {
            if (isCrouch)
            {
                capsule.Height = Mathf.Lerp(capsule.Height, 0.25f, 5f * (float)delta);
            }
            else
            {
                capsule.Height = Mathf.Lerp(capsule.Height, 1.8f, 5f * (float)delta);
            }
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * speed;
            velocity.Z = direction.Z * speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    public void DisableControl()
    {
        isDisabled = true;
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }
}
