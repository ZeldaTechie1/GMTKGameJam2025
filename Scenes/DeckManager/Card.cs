using Godot;
using System;

public partial class Card : Node
{
    [ExportGroup("Card Buttons")]
    [Export]
    private Button NameButton;
    [Export]
    private TextureButton PlayCard;


    public CardManager Card_Manager;

    public int CardID;
    public override void _Ready()
    {
        NameButton.Pressed += HandleNameButtonPressed;
        PlayCard.Pressed += HandlePlayCardPressed;
    }

    private void HandlePlayCardPressed()
    {
        GD.Print("Card#" + CardID + " Played");
        Card_Manager.EmitPlayCard(CardID);
    }


    private void HandleNameButtonPressed()
    {
        GD.Print("Card#" + CardID + " Card Name");
    }

    public void ActivateCard()
    {
         GD.Print("Card#" + CardID + "Activated");
    }
    
}
