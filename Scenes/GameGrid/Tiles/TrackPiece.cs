using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public partial class TrackPiece : MeshInstance3D
{
    [Export]
    TrackPeiceType PeiceType;

    [Export]
    Node3D EntrancePoint1;

    [Export]
    Node3D EntrancePoint2;

    public int CellSize;

    public TrackPeiceType TrackType;

    public Vector3 GetPathDirection()
    {
       return EntrancePoint1.Position.DirectionTo(EntrancePoint2.Position);
    }


    public override void _Ready()
    {




    }




}

public enum TrackPeiceType
{
    Straight,
    Curve,
    End,
    Default
}



