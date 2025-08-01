using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public partial class TrackPiece : MeshInstance3D
{
    [Export]
    public TrackPieceType PieceType;



    [Export]
    public ItemSpawnPoint[] ItemSpawnPoints;

    public int CellSize;


    public ItemSpawnPoint GetRandomItemSpawnPoint(LevelItemSize size, LevelItemLocation location)
    {
        List<ItemSpawnPoint> PossiblePoints = new List<ItemSpawnPoint>();
        if (ItemSpawnPoints != null && ItemSpawnPoints.Count() > 0)
        {
            foreach (ItemSpawnPoint point in ItemSpawnPoints)
            {
                if (point.PointSize == size)
                {
                    if (point.PointLocation == location)
                    {
                        PossiblePoints.Add(point);
                    }
                }
            }

        }

        Random rand = new Random();
        if (PossiblePoints.Count > 0)
        {
            return PossiblePoints[rand.Next(0, PossiblePoints.Count)];
        }
        else
        {
            return null;
        }
    }
   

  


    public override void _Ready()
    {




    }




}

public enum TrackPieceType
{
    Straight,
    Empty,
    End,
    Start,
    Checkpoint,
    Default
}



