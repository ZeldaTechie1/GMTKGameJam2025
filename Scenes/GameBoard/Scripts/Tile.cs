using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;

public partial class Tile : Node3D
{


    [Export]
    public TileType Tile_Type = TileType.Empty;

    public TrackPiece Track;

    public int CellSize = 50;
    public Node3D[] ObstacleSpawnPoints;

    public int TileID { get; set; }

    public Dictionary<Vector2, double> Adjacent { get; set; } // Key: neighbor's NodeId, Value: distance

    public void addAdjacent(Vector2 adjacentGridPosition, double value)
    {
        Adjacent.Add(adjacentGridPosition, value);
    }

    public float Weight;

    public Tile LastTile = null;
    public Tile NextTile = null;
    public Tile(int TID, TileType type)
    {
        TileID = TID;
        Tile_Type = type;

    }



    public TrackPieceType NeededPiece()
    {
        switch (Tile_Type)
        {
            case TileType.Empty:
                return TrackPieceType.Empty;
                break;
            case TileType.Block:
                return TrackPieceType.Straight;
                break;
            case TileType.Checkpoint:
                return TrackPieceType.Checkpoint;
                break;
            case TileType.Start:
                return TrackPieceType.Start;
                break;
            case TileType.End:
                return TrackPieceType.End;
                break;
        }
        return TrackPieceType.Default;
    }
    public void ClearTrack()
    {
        Track.QueueFree();
    }




}


