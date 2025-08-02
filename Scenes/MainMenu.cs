using Godot;
using System;

public partial class MainMenu : Control
{
    [Export] Button playButton;

    public override void _Ready()
    {
        playButton.ButtonDown += PlayButton_ButtonDown;
    }

    private void PlayButton_ButtonDown()
    {
        GetTree().ChangeSceneToFile("res://Scenes/TutorialScene.tscn");
    }
}
