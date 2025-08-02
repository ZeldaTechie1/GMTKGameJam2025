using Godot;
using System;

public partial class CardManager : Node
{
    [Signal] public delegate void AffectTileEventHandler(Tile tile, bool EffectsRandomTile, bool DrawBackToRandomTile);
    [Signal] public delegate void CardSpawnItemEventHandler(Tile tile, bool EffectsRandomTile, bool DrawBackToRandomTile, PackedScene Aid, PackedScene Obstacel);

    [Signal] public delegate void PlayCardEventHandler(int CardID);
    [Signal] public delegate void DrawCardEventHandler();
    [Signal] public delegate void DiscardCardEventHandler(int CardID);

    [Export] public ItemSpawner Item_Spawner;

    public void EmitPlayCard(int CardID)
    {
        EmitSignal(SignalName.PlayCard, CardID);
    }
    public void EmitDrawCard()
    {
        EmitSignal(SignalName.DrawCard);
    }
     

}
