using Godot;
using System;

public partial class GameFinish : Node3D
{
    [Export] Button exitButton;
    [Export] Button mainMenuButton;

    public override void _Ready()
    {
        exitButton.ButtonUp += ExitButton_ButtonUp;
        mainMenuButton.ButtonDown += MainMenuButton_ButtonDown;
    }

    private void MainMenuButton_ButtonDown()
    {
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
    }

    private void ExitButton_ButtonUp()
    {
        GetTree().Quit();
    }
}
