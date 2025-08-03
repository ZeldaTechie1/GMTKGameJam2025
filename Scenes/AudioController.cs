using Godot;
using System;
using System.ComponentModel;


public partial class AudioController : Node3D
{
    [Export] LevelManager levelManager;
    [Export] AudioStreamPlayer[] musicPlayers;
    [Export] Hand hand;
    [Export] AudioStreamPlayer playCardSound;
    [Export] AudioStreamPlayer drawCardSound;

    public override void _Ready()
    {
        LowerAllMusic();
        levelManager.RoundStarted += PlayRoundMusic;
        levelManager.PlayStarted += PlayPlayingMusic;
        hand.CardPlayed += CardPlayed;
        hand.CardDrawn += CardDrawn;
    }

    private void CardDrawn()
    {
        drawCardSound.Play();
    }

    private void CardPlayed()
    {
        playCardSound.Play();
    }

    public void PlayRoundMusic()
    {
        LowerAllMusic();
        musicPlayers[0].VolumeDb = -10;
    }
    public void PlayPlayingMusic()
    {
        LowerAllMusic();
        musicPlayers[1].VolumeDb = -10;
    }

    public void LowerAllMusic()
    {
        foreach(AudioStreamPlayer players in musicPlayers)
        {
            players.VolumeLinear = 0;
        }
    }
}
