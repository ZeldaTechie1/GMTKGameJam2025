using Godot;
using System;
using System.Collections.Generic;
using System.Net.Http;

public partial class Hand : Node3D
{
    [Export] LevelManager LevelManager;
    [Export] Deck currentDeck;

    List<Card>currentHand=new List<Card>();
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
    [Export]
    CardManager Card_Manager;
    



    public bool MouseInteraction = true;

    public override void _Ready()
    {
        Card_Manager.DrawCard += Draw;
        Card_Manager.PlayCard += PlayCard;
        LevelManager.RoundStarted += RoundStarted;
        LevelManager.RoundFinished += RoundEnded;
    }

    private void PlayCard(int CardID)
    {   Tile tile = LevelManager.GBManager.SelectedTile;
        if (tile != null)
        {
            if (currentHand[CardID].ActivateCard(tile))
            {
                for (int i = 0; i < currentHand.Count; i++)
                {
                    if (currentHand[i].CardID == CardID)
                    {
                        currentHand.RemoveAt(i);
                        currentHandContainer.GetChild(i).QueueFree();
                        UpdateCardIDS();
                        break;
                    }
                }
            }
            else
            {
                GD.Print("NO DICE HOMIE!");
            }

           
        }

      

    }
    public void UpdateCardIDS()
    {
        for (int i = 0; i < currentHand.Count; i++)
        {
            currentHand[i].CardID = i;
        }
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

                    if (holder.GetParent().GetParent() is Tile tile)
                    {
                        GBManager.SelectTile(tile);
                    }
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

        // if (Input.IsActionJustPressed("PickCard") && LevelManager.currentState == LevelState.InRound)
        // {
        //     TESTUseRandomCard();
        // }


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
            Card newCard = (Card)card.Instantiate();
            newCard.Card_Manager = Card_Manager;
            currentHand.Add(newCard);
            currentHandContainer.AddChild(newCard);
            UpdateCardIDS();
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
        
    }
}
