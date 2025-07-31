using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public partial class TrackPiece : MeshInstance3D
{
    [Export]
    TrackPeiceType PeiceType;


    public int CellSize;

    public TrackPeiceType TrackType;

    public override void _Ready()
    {




    }




}

public enum TrackPeiceType
{
    Curve,
    Straight,
    End,
    Default
}



