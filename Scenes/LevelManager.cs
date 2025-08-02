using Godot;
using System;

public partial class LevelManager : Node3D
{
    [Export] Timer levelTimer;
    [Export] RichTextLabel timerText;
    [Export] Camera3D OverviewCamera;
    [Export] Camera3D PlayerCamera;
    [Export] int maxRounds;
    [Export] public int currentRound { get; private set; }

    [Export] public GameBoardManager GBManager;

    [Export] public Area3D GoalArea;
    [Signal] public delegate void LevelStartedEventHandler();
    [Signal] public delegate void TimerEndedEventHandler();
    [Signal] public delegate void RoundStartedEventHandler();
    [Signal] public delegate void RoundFinishedEventHandler();
    [Signal] public delegate void PlayStartedEventHandler();
    [Signal] public delegate void PlayEndedEventHandler();
    [Signal] public delegate void PlayerFailedEventHandler();
    [Signal] public delegate void PlayerSucceededEventHandler();
    [Signal] public delegate void PlayerDeathEventHandler();
    [Signal] public delegate void GameOverEventHandler();
    [Signal] public delegate void GenerateBoardEventHandler();
    [Signal] public delegate void AddToBoardEventHandler();

    public LevelState currentState;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        StartLevel();
    }

    public override void _Process(double delta)
    {
        if (!levelTimer.IsStopped())
        {
            int minutes = Mathf.FloorToInt(levelTimer.TimeLeft / 60);
            int seconds = Mathf.FloorToInt(levelTimer.TimeLeft % 60);
            int millisecs = Mathf.FloorToInt((levelTimer.TimeLeft - (int)levelTimer.TimeLeft) * 100);
            timerText.Text = $"{minutes}:{seconds}:{millisecs}";
        }

        if (Input.IsActionJustPressed("StartPlay"))
        {
            if (currentState == LevelState.InRound)
            {
                EndRound();
                StartPlay();
            }
        }
    }

    private void GoalArea_BodyEntered(Node3D body)
    {
        if (body is BasicPlayer)
            GoalReached();
    }

    public void StartLevel()
    {
        EmitGenerateBoard();
        StartRound();
    }

    public void StartRound()
    {
        if (currentRound > maxRounds)
        {
            GameIsOver();
            GD.Print("Waahoo");
            return;
        }
        currentRound++;
        currentState = LevelState.InRound;
        OverviewCamera.Current = true;
        PlayerCamera.Current = false;
        EmitRoundStarted();
    }
    public void EndRound()
    {
        OverviewCamera.Current = false;
        PlayerCamera.Current = true;
        EmitRoundFinished();
    }

    public void StartPlay()
    {
        currentState = LevelState.InPlay;
        EmitPlayStarted();
        levelTimer.Start();
        levelTimer.Timeout += TimerFinished;
        GoalArea.BodyEntered += GoalArea_BodyEntered;
    }

    public void EndPlay()
    {
        EmitPlayFinished();
        levelTimer.Timeout -= TimerFinished;
        GoalArea.BodyEntered -= GoalArea_BodyEntered;
        StartRound();
    }

    public void TimerFinished()
    {
        levelTimer.Stop();
        EmitTimerEnded();
        PlayerFail();
    }

    public void GoalReached()
    {
        levelTimer.Stop();
        EmitPlayerSucceeded();
        EndPlay();
    }

    public void PlayerDead()
    {
        PlayerFail();
    }

    public void PlayerFail()
    {
        EmitPlayerFailed();
        EndPlay();
    }

    public void GameIsOver()
    {
        EmitGameOver();
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

    private void EmitPlayStarted()
    {
        EmitSignal(SignalName.PlayStarted);
    }

    private void EmitPlayFinished()
    {
        EmitSignal(SignalName.PlayEnded);
    }


    private void EmitPlayerFailed()
    {
        EmitSignal(SignalName.PlayerFailed);
    }

    private void EmitPlayerSucceeded()
    {
        EmitSignal(SignalName.PlayerSucceeded);
    }

    private void EmitPlayerDeath()
    {
        EmitSignal(SignalName.PlayerDeath);
    }
    private void EmitGenerateBoard()
    {
        EmitSignal(SignalName.GenerateBoard);
    }
    private void EmitAddToBoard()
    {
        EmitSignal(SignalName.AddToBoard);
    }

    private void EmitGameOver()
    {
        EmitSignal(SignalName.GameOver);
    }
}

public enum LevelState
{
    InRound,
    InPlay
}
