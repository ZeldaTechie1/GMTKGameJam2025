using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class AIDCard : Card
{
    [ExportGroup("Associated Items")]
    [Export]
    PackedScene AidItem;
    [Export]
    PackedScene Obstacle;

    public override bool ActivateCard(Tile tile)
    {
        bool CardPlayed = false;
        if (CardDrawback)
        {
            try
            {

                CardPlayed = Card_Manager.Item_Spawner.CardSpawn(tile, AidItem, Obstacle, false, EffectsRandomTile, DrawBackToRandomTile);
            }
            catch (Exception ex)
            {
                GD.PrintErr(ex.ToString());
            }

        }
        else
        {
            CardPlayed = Card_Manager.Item_Spawner.CardSpawn(tile, AidItem, null,false, EffectsRandomTile, DrawBackToRandomTile);
        }
        return CardPlayed;
    }
    


}
