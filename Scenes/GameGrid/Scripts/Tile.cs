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

    public int CellSize = 1;

    public Vector2 GridPosition;

    public float DistanceToTarget;
    public float Cost;
    public float Weight;
    public float F
    {
        get
        {
            if (DistanceToTarget != -1 && Cost != -1)
                return DistanceToTarget + Cost;
            else
                return -1;
        }
    }


    public Tile LastTile = null;
    public Tile NextTile = null;
    public Tile(TileType type, Vector2 gP, float weight = 1)
    {
        Tile_Type = type;

        GridPosition = gP;


    }

    public Tile()
    {
        Tile_Type = TileType.Empty;

    }

    Vector3 GetDirectionToNext()
    {
        return Position.DirectionTo(NextTile.Position);
    }

    Vector3 GetDirectionToLast()
    {
        return Position.DirectionTo(LastTile.Position);
    }

    public void RoatePiece()
    {
        
    }

    public TrackPeiceType NeededPeice()
    {
        if (Tile_Type == TileType.Empty)
        {
            Vector3 next = GetDirectionToNext();

            Vector3 last = GetDirectionToLast();

            if (next.Y > 0)
            {
                if (last.Y < 0)
                {
                    return TrackPeiceType.Straight;
                }
                else
                {
                    return TrackPeiceType.Curve;
                }

            }
            else if (next.X == (last.X * -1))
            {
                return TrackPeiceType.Straight;
            }
            else if (next.X != 0 && last.X == 0)
            {
                return TrackPeiceType.Curve;
            }

            return default;

        }
        return TrackPeiceType.End;



    }

    








}
