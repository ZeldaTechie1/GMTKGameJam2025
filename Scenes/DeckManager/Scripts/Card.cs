using Godot;
using System;

public partial class Card : Node
{

    [ExportGroup("Card Settings")]
    [Export]
    public CardType Card_Type;
    [Export]
    public bool CardDrawback = false;
    [Export]
    public bool EffectsRandomTile = false;
    [Export]
    public bool DrawBackToRandomTile = false;



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

    public virtual bool ActivateCard(Tile tile)
    {

        GD.Print("Generic Card");
        return false;
    }

}
public enum CardType
{
    Aid,
    Modifier,

    Track,
}
public enum CardDrawbackType
{
    RemoveAid,
    AddObstacle,
    Track,
}
