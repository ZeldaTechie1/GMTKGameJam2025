using Godot;
using System;

public partial class SignalTester : Node3D
{
    [Export]LevelManager levelManager;

    public override void _Ready()
    {
        levelManager.RoundStarted += () => GD.Print("Round Started");
        levelManager.RoundFinished += () => GD.Print("RoundFinished");
        levelManager.PlayStarted += () => GD.Print("PlayStarted");
        levelManager.PlayEnded += () => GD.Print("PlayEnded");
        levelManager.PlayerFailed += () => GD.Print("PlayerFailed");
        levelManager.PlayerSucceeded += () => GD.Print("PlayerSucceeded");
        levelManager.TimerEnded += () => GD.Print("TimerEnded");
    }
}
