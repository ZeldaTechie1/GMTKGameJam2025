using Godot;
using System;

public partial class LevelManager : Node3D
{
    [Export] Timer levelTimer;
    [Export] RichTextLabel timerText;
    [Export] Area3D GoalArea;

    [Signal] public delegate void TimerEndedEventHandler();
    [Signal] public delegate void RoundStartedEventHandler();
    [Signal] public delegate void RoundFinishedEventHandler();
    [Signal] public delegate void PlayerFailedEventHandler();
    [Signal] public delegate void PlayerSucceededEventHandler();

    public override void _Ready()
    {
        
        StartRound();
    }

    private void GoalArea_BodyEntered(Node3D body)
    {
        if (body is BasicPlayer)
            GoalReached();
    }

    public override void _Process(double delta)
    {
        if(!levelTimer.IsStopped())
        {
            int minutes = Mathf.FloorToInt(levelTimer.TimeLeft / 60);
            int seconds = Mathf.FloorToInt(levelTimer.TimeLeft % 60);
            int millisecs = Mathf.FloorToInt((levelTimer.TimeLeft - (int)levelTimer.TimeLeft) * 100);
            timerText.Text = $"{minutes}:{seconds}:{millisecs}";
        }
    }

    public void StartRound()
    {
        levelTimer.Start();
        EmitRoundStarted();
        levelTimer.Timeout += TimerFinished;
        GoalArea.BodyEntered += GoalArea_BodyEntered;
    }
    public void TimerFinished()
    {
        levelTimer.Stop();
        EmitTimerEnded();
        EmitPlayerFailed();
        EndRound();
    }

    public void GoalReached()
    {
        levelTimer.Stop();
        EmitPlayerSucceeded();
        EndRound();
    }

    public void EndRound()
    {
        EmitRoundFinished();
        levelTimer.Timeout -= TimerFinished;
        GoalArea.BodyEntered -= GoalArea_BodyEntered;
    }

    private void EmitTimerEnded()
    {
        EmitSignal(SignalName.TimerEnded);
    }

    private void EmitRoundStarted()
    {
        EmitSignal(SignalName.RoundStarted);
    }

    private void EmitRoundFinished()
    {
        EmitSignal(SignalName.RoundFinished);
    }


    private void EmitPlayerFailed()
    {
        EmitSignal(SignalName.PlayerFailed);
    }

    private void EmitPlayerSucceeded()
    {
        EmitSignal(SignalName.PlayerSucceeded);
    }

}
