using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
public partial class GameGrid : Node3D
{

    [Export]
    int BoardLength;

    [Export]
    float Margin = .02f;

    [Export]
    int CellSize = 1;

    [Export]
    PackedScene[] TrackPieces;

    List<TrackPiece> PreloadPieces=new List<TrackPiece>();

    List<Tile> Board = new List<Tile>();
  

    public Tile FirstStartTile;
    public Tile CurrentStartTile;
    public Tile CurrentEndTile;



    public override void _Ready()
    {
        base._Ready();
        LoadPieces();
        GenerateMap();
        PopulateTileshWithTracks(Board);

     
      
     

    }
    public void GenerateMap()
    {
        for (int i = 0; i < BoardLength; i++)
        {
            Vector3 position = GlobalPosition + new Vector3(0, 0, i * (CellSize * Margin));
            if (i == 0)
            {

                Tile tileHolder = new Tile(i, TileType.Start);
                AddChild(tileHolder);
                tileHolder.GlobalPosition = position;
                CurrentStartTile = tileHolder;
                Board.Add(tileHolder);

            }
            else if (i == BoardLength - 1)
            {
                Tile tileHolder = new Tile(i, TileType.End);
                AddChild(tileHolder);
                tileHolder.GlobalPosition = position;
                CurrentEndTile = tileHolder;
                Board.Add(tileHolder);
            }
            else
            {
                Tile tileHolder = new Tile(i, TileType.Block);
                AddChild(tileHolder);
                tileHolder.GlobalPosition = position;
                Board.Add(tileHolder);
            }
            
        }
    }
    public void PopulateTileshWithTracks(List<Tile>BoardSegment)
    {
     
        foreach (Tile t in BoardSegment)
        {
            TrackPiece piece = GetTrackPiece(t);
            if (piece != null)
            {
                t.Track = piece;
                t.Track.Position = t.Position;
            }

            else
            { 
                 GD.PrintErr("ERROR!");
            }

        }
    }


    public TrackPiece GetTrackPiece(Tile t)
    {   try
        {
            TrackPieceType needed = t.NeededPiece();
            List<int> possiblePieces = new List<int>();
    
            for(int i= 0; i < PreloadPieces.Count();i++)
            {
               
                if (PreloadPieces[i].PieceType== needed)
                {
                    possiblePieces.Add(i);
                }
            }


            Random rand = new Random();

            if (possiblePieces.Count() > 0)
            {
                var scene =(TrackPiece) TrackPieces[possiblePieces[rand.Next(0, possiblePieces.Count())]].Instantiate();
                scene.Position = t.Position;
                t.AddChild(scene);
                return scene;

            }
            else
            {
                GD.PrintErr("PieceNotFound");
                return null;
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr("Eror:"+ex.ToString());
            return null;
        }
      
     

    }

    public void LoadPieces()
    {
        for (int i = 0; i < TrackPieces.Count(); i++)
        {
            var test = TrackPieces[i].GetLocalScene();
            TrackPiece holder = (TrackPiece)(TrackPieces[i].Instantiate(PackedScene.GenEditState.Disabled));
            GD.PrintErr("Load index (" + i + "), Type [" + holder.PieceType + "]," + TrackPieces[i].ToString());
            PreloadPieces.Add(holder);
        }
    }

}
