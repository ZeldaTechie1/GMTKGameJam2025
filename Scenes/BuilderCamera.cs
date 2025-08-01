using Godot;
using System;

public partial class BuilderCamera : Camera3D
{
    [Export]LevelManager levelManager;
    [Export]GameBoardManager gameBoardManager;

    Vector3 FirstTilePosition => gameBoardManager.FirstStartTile.GlobalPosition;
    Vector3 EndTilePosition => gameBoardManager.CurrentEndTile.GlobalPosition;

    public override void _Ready()
    {
        levelManager.RoundStarted += LevelManager_RoundStarted;
    }

    public override void _Process(double delta)
    {
        if (levelManager.currentState != LevelState.InRound)
            return;

        Vector2 inputDir = Input.GetVector("Left", "Right", "Forward", "Backward");

        Vector3 velocity = new Vector3(0, 0, inputDir.X);

        GlobalPosition += velocity;
        GlobalPosition = new Vector3(GlobalPosition.X, GlobalPosition.Y, Mathf.Clamp(GlobalPosition.Z, FirstTilePosition.Z, EndTilePosition.Z));
    }

    private void LevelManager_RoundStarted()
    {
        Vector3 boardCenter = (FirstTilePosition + EndTilePosition) / 4;
        GlobalPosition = new Vector3(GlobalPosition.X,GlobalPosition.Y, boardCenter.Z);
    }
}
