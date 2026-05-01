using System;
using Godot;

public partial class JumpScareState : IState
{
    private Enemy enemy;

    private double timer = 0;
    private double jumpscareDuration = 3.0; // match your animation length
    private bool done = false;

    public JumpScareState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.Velocity = Vector3.Zero;

        enemy.Player.DisableControl();

        enemy.ActivateJumpscareCamera();

        enemy.PlayAnimation("jumpscare");

        timer = 0;
        done = false;
    }

    public void Exit() { }

    public void PhysicsUpdate(double delta)
    {
        enemy.Velocity = Vector3.Zero;
    }

    public void Update(double delta)
    {
        if (done)
            return;

        timer += delta;
        if (timer >= jumpscareDuration)
        {
            done = true;
            enemy.ShowDeathUI();
        }
    }
}
