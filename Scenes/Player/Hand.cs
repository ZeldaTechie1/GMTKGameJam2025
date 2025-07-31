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

        if(Input.IsActionJustPressed("PickCard"))
        {
            TESTUseRandomCard();
        }
    }

    public void RoundStarted()
    {
        mainUIContainer.Show();
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
