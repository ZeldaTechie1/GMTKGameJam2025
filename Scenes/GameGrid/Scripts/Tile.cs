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
        if (NextTile != null)
        {
            return Position.DirectionTo(NextTile.Position);
        }
        else
        {
            return Vector3.Zero;
        }

       
    }

    Vector3 GetDirectionToLast()
    {
        if (LastTile != null)
        {
            return Position.DirectionTo(LastTile.Position);
        }
        else
        {
            return Vector3.Zero;
        }
        
    }

    Vector3 GetFlowDirection()
    {
         return LastTile.Position.DirectionTo(NextTile.Position);
    }

    public void RoatePiece()
    {

    }

    public TrackPieceType NeededPiece()
    {
        if (Tile_Type == TileType.Empty)
        {
            Vector3 next = GetDirectionToNext();

            Vector3 last = GetDirectionToLast();

            if (next.Z > 0)
            {
                if (last.Z < 0)
                {
                    return TrackPieceType.Straight;
                }
                else
                {
                    return TrackPieceType.Curve;
                }

            }
            else if (next.X == (last.X * -1))
            {
                return TrackPieceType.Straight;
            }
            else if (next.X != 0 && last.X == 0)
            {
                return TrackPieceType.Curve;
            }

            GD.PrintErr("Tile (" + GridPosition.X + "," + GridPosition.Y + ") Cant Determine Track Peice ["+next+","+last+"]");
            return default;

        }
        return TrackPieceType.End;
    }


    public void OrientTrackPiece()
    {
        Vector3 a = GetFlowDirection();
        Vector3 b = Track.GetPathDirection();
        Vector3 J = a * b;
        Vector3 K = a.Abs() * b.Abs();
        
        switch (Track.TrackType)
        {
            case TrackPieceType.Curve:
                while (J != K)
                {
                    Track.Rotate(new Vector3(0, 1, 0), 90.0f);
                    a = GetFlowDirection();
                    b = Track.GetPathDirection();
                    J = a * b;
                    K = a.Abs() * b.Abs();
                }

                break;
            case TrackPieceType.End:
                if (Tile_Type == TileType.End)
                {
                    Track.Rotate(new Vector3(0, 1, 0), 180.0f);
                }
            
                break;
            case TrackPieceType.Default:
                break;

            case TrackPieceType.Straight:
                while (J != K)
                {
                    Track.Rotate(new Vector3(0, 1, 0), 90.0f);
                    a = GetFlowDirection();
                    b = Track.GetPathDirection();
                    J = a * b;
                    K = a.Abs() * b.Abs();

                }
                break;

            default:
                break;
        }
    }











}
