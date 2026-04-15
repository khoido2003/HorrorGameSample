using System;
using Godot;

public partial class TaskUI : CanvasLayer
{
    [Export]
    private RichTextLabel taskText;

    public override void _Ready()
    {
        SetTask("Turn on the light");
    }

    public void SetTask(string text)
    {
        taskText.Text = text;
    }
}
