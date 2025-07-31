using Godot;
using System;
using Godot.Collections;
using System.Linq;

public partial class Deck : Node3D
{
    [Export] PackedScene[] deck;

    public override void _Ready()
    {
        Shuffle();
    }

    public void Shuffle()
    {
        Random.Shared.Shuffle(deck);
    }

    public PackedScene[] Draw(int amount)
    {
        if (deck.Count() < amount)
            return null;

        PackedScene[] drawnCards = new PackedScene[amount];

        for(int index = 0; index < amount; index++)
        {
            drawnCards[index] = deck[0];
            var tempDeck = deck.ToList();
            tempDeck.RemoveAt(0);
            deck = tempDeck.ToArray();
        }

        return drawnCards;
    }
}
