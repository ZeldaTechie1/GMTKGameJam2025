using Godot;
using System;
using System.Net.Http;

public partial class Hand : Node3D
{
    [Export] LevelManager LevelManager;
    [Export] Deck currentDeck;
    [Export] Godot.Collections.Array<Card> currentHand;
    [Export] HBoxContainer currentHandContainer;
    [Export] int maxHandCount;

    public override void _Ready()
    {
        LevelManager.RoundStarted += RoundStarted;
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("Draw"))
        {
            Draw();
        }
    }

    public void RoundStarted()
    {
        DrawCards(maxHandCount);
    }

    public void Draw()
    {
        DrawCards(1);
    }

    public void DrawCards(int amount)
    {
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
        }
    }
}
