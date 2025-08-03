using Godot;
using System;

public partial class MainMenu : Control
{
    [Export] Button playButton;
    [Export] Button exitButton;
    [Export] Button skipButton;

    public override void _Ready()
    {
        playButton.ButtonDown += PlayButton_ButtonDown;
        exitButton.ButtonDown += ExitButton_ButtonDown;
        skipButton.ButtonDown += SkipButton_ButtonDown;
    }

    private void SkipButton_ButtonDown()
    {
        GetTree().ChangeSceneToFile("res://Scenes/PlayerControllerTest.tscn");
    }

    private void ExitButton_ButtonDown()
    {
        GetTree().Quit();
    }

    private void PlayButton_ButtonDown()
    {
        GetTree().ChangeSceneToFile("res://Scenes/TutorialScene.tscn");
    }
}
