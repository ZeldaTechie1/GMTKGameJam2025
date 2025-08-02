using DialogueManagerRuntime;
using Godot;
using System;
using System.Collections.Generic;

public partial class SlideHolder : Control
{
    [Export] public Resource DialogueResource;//What dialogue script are we going to use?
    [Export] public string DialogueStart = "start";//If there are multiple titles, where do I start at?
    [Export] TextureRect textureRect;
    [Export] Texture2D[] slides;
    [Export] Node3D[] lifeguards;
    int currentSlide = 0;
    int currentLifeGuard = 0;
    ExampleBalloon balloon;

    public override void _Ready()
    {
        Reset();
        balloon = DialogueManager.ShowDialogueBalloon(DialogueResource, DialogueStart) as ExampleBalloon;
        balloon.NextDialogue += NextSlide;
        lifeguards[currentLifeGuard].Visible = true;
    }

    public void Reset()
    {
        currentSlide = 0;
        textureRect.Texture = slides[currentSlide];
    }

    public void NextSlide()
    {
        if (currentSlide >= slides.Length - 1)
        {
            GetTree().ChangeSceneToFile("res://Scenes/PlayerControllerTest.tscn");
            return;
        }

        if (currentSlide > 1)
        {
            foreach (Node3D lifeguard in lifeguards)
            {
                lifeguard.GetNode<Node3D>("Visor").Visible = true;
            }
        }

        currentSlide++;
        lifeguards[currentLifeGuard].Visible = false;
        currentLifeGuard++;
        currentLifeGuard = Mathf.Wrap(currentLifeGuard,0,lifeguards.Length);
        lifeguards[currentLifeGuard].Visible = true;


        textureRect.Texture = slides[currentSlide];

    }
    
}
