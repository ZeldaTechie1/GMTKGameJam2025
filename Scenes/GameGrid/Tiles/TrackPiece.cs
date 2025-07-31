using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public partial class TrackPiece : MeshInstance3D
{
    [Export]
    public TrackPieceType PieceType;

    [Export]
    Node3D EntrancePoint1;

    [Export]
    Node3D EntrancePoint2;

    public int CellSize;

   

    public Vector3 GetPathDirection()
    {
       return EntrancePoint1.Position.DirectionTo(EntrancePoint2.Position);
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



