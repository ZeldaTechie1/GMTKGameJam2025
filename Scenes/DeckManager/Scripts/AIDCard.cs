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
                //GD.Print(Card_Manager.Item_Spawner.Name);
                GD.Print(tile.Name);
                GD.Print(AidItem.ResourceName.ToString());
                GD.Print(Obstacle.ResourceName.ToString());
                GD.Print(EffectsRandomTile.ToString());
                GD.Print(DrawBackToRandomTile.ToString());

                CardPlayed = Card_Manager.Item_Spawner.CardSpawn(tile, AidItem, Obstacle, false, false, EffectsRandomTile, DrawBackToRandomTile);
            }
            catch (Exception ex)
            {
                GD.PrintErr(ex.ToString());
            }

        }
        else
        {
            CardPlayed = Card_Manager.Item_Spawner.CardSpawn(tile, AidItem, null, false, false, EffectsRandomTile, DrawBackToRandomTile);
        }
        return CardPlayed;
    }
    


}
