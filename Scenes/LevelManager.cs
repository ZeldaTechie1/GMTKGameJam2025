using Godot;
using System;

public partial class LevelManager : Node3D
{
    [Export] Timer levelTimer;
    [Export] RichTextLabel timerText;

    public override void _Ready()
    {
        levelTimer.Start();
    }

    public override void _Process(double delta)
    {
        int minutes = Mathf.FloorToInt(levelTimer.TimeLeft / 60);
        int seconds = Mathf.FloorToInt(levelTimer.TimeLeft % 60);
        int millisecs = Mathf.FloorToInt((levelTimer.TimeLeft - (int)levelTimer.TimeLeft) * 100);
        timerText.Text = $"";
    }
}
