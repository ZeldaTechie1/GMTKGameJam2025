using Godot;
using System;
using System.Net.Http;

public partial class Hand : Node3D
{
    [Export] LevelManager LevelManager;
    [Export] Deck currentDeck;
    [Export] Godot.Collections.Array<Card> currentHand;
    [Export] HBoxContainer currentHandContainer;
    [Export] Control mainUIContainer;
    [Export] int maxHandCount;
    [Export] int maxDraws;
    [Export] int currentDraws;
    [Export] Camera3D OverViewCamera;

    [Export] GameBoardManager GBManager;
    // Mouse controls
    Vector3 RayOrigin;
    Vector3 RayEnd;
    

    public bool MouseInteraction = true;

    public override void _Ready()
    {
        LevelManager.RoundStarted += RoundStarted;
        LevelManager.RoundFinished += RoundEnded;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (MouseInteraction)
        {
            if (Input.IsActionJustPressed("Click") && LevelManager.currentState == LevelState.InRound)
            {
                var spaceState = GetWorld3D().DirectSpaceState;
                Vector2 mousePosition = GetParent().GetViewport().GetMousePosition();
                RayOrigin = OverViewCamera.ProjectRayOrigin(mousePosition);
                RayEnd = OverViewCamera.ProjectRayNormal(mousePosition) * 2000;
                PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(RayOrigin, RayEnd);
                var results = spaceState.IntersectRay(query);
                if (results.Count > 0)
                {
                    //GD.Print("Hit at point: ", results["collider_id"]);
                    var holder = (Node3D)results["collider"];

                     GD.Print("Parent:"+ holder.Name);
                }   
                    

            }
        
        }

    }


    public override void _Process(double delta)
    {


        if (Input.IsActionJustPressed("Draw") && LevelManager.currentState == LevelState.InRound)
        {
            Draw();
        }

        if (Input.IsActionJustPressed("PickCard") && LevelManager.currentState == LevelState.InRound)
        {
            TESTUseRandomCard();
        }



    }

    public void RoundStarted()
    {
        mainUIContainer.Show();
        DrawCards(maxHandCount);
        currentDraws = 0;
    }

    public void RoundEnded()
    {
        mainUIContainer.Hide();
    }

    public void Draw()
    {
        DrawCards(1);
    }

    public void DrawCards(int amount)
    {
        if (currentDraws >= maxDraws)
        {
            GD.Print("Not enough draws");
            return;
        }
        if (currentHand.Count + amount > maxHandCount)
        {
            amount = maxHandCount - currentHand.Count;
            GD.Print($"Trying to draw too many cards, reduced amount to {amount}");
        }
        if (amount == 0)
        {
            GD.Print("Hand already full!");
            return;
        }

        PackedScene[] drawnCards = currentDeck.Draw(amount);
        if (drawnCards == null)
        {
            GD.Print("Not enough cards in the deck. What have you done?! It's over, you've ruined it. This establishment was built on cards and now it's all gone!");
            return;
        }

        foreach (PackedScene card in drawnCards)
        {
            Node newCard = card.Instantiate(PackedScene.GenEditState.Disabled);
            currentHand.Add(newCard as Card);
            currentHandContainer.AddChild(newCard);
            currentDraws++;
        }
    }

    public void TESTUseRandomCard()
    {
        if(currentHand.Count == 0)
        {
            GD.Print("Not enough cards to pick from");
            return;
        }
        int randCard = Random.Shared.Next(0, currentHand.Count);
        currentHand.RemoveAt(randCard);
        currentHandContainer.GetChild(randCard).QueueFree();
    }
}
