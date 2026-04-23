using System;
using Godot;

public partial class TaskTrigger : Area3D
{
    [Export]
    private TaskUI taskUI;

    [Export]
    private string taskText;

    [Export]
    private Enemy enemy;

    private bool isTrigger = false;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;

        enemy.ProcessMode = ProcessModeEnum.Inherit;
        enemy.Visible = true;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body is Player player)
        {
            EnterTrigger(player);
        }
    }

    private void EnterTrigger(Player player)
    {
        isTrigger = true;

        taskUI.SetTask(taskText);
    }
}
