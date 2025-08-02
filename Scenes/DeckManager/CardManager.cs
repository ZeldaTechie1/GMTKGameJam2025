using Godot;
using System;

public partial class CardManager : Node
{
    [Signal] public delegate void PlayCardEventHandler(int CardID);
    [Signal] public delegate void DrawCardEventHandler();
    [Signal] public delegate void DiscardCardEventHandler(int CardID);

    public void EmitPlayCard(int CardID)
    {
        EmitSignal(SignalName.PlayCard, CardID);
    }
    public void EmitDrawCard()
    {
        EmitSignal(SignalName.DrawCard);
    }
     

}
