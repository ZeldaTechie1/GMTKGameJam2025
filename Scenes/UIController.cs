using Godot;
using System;

public partial class UIController : Node3D
{
    [Export] LevelManager LevelManager;
    [Export] Hand currentHand;
    [Export] Label drawsLeftLabel;
    [Export] Label playsLeftLabel;

    public override void _Ready()
    {
        currentHand.CardDrawn += CurrentHand_CardDrawn;
        LevelManager.RoundStarted += CurrentHand_CardDrawn;
        LevelManager.RoundStarted += LevelManager_PlayStarted;
        drawsLeftLabel.Text = $"Draws Left: {currentHand.maxDraws}";
        playsLeftLabel.Text = $"Plays Left: {LevelManager.maxPlaysFailed}";
    }

    private void CurrentHand_CardDrawn()
    {
        drawsLeftLabel.Text = $"Draws Left: {currentHand.maxDraws - currentHand.currentDraws}";
    }

    private void LevelManager_PlayStarted()
    {
        playsLeftLabel.Text = $"Plays Left: {LevelManager.maxPlaysFailed - LevelManager.playsFailed}";
    }
}
