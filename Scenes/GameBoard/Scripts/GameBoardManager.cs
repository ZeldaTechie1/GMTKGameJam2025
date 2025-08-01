using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public partial class GameBoardManager : Node3D
{
    [Export]
    LevelManager Level_Manager;

    [Export]
    int BoardLength;

    [Export]
    float Margin = .02f;

    [Export]
    int CellSize = 1;

    [Export]
    PackedScene[] TrackPieces;

    List<TrackPiece> PreloadPieces = new List<TrackPiece>();

    List<Tile> Board = new List<Tile>();


    public Tile FirstStartTile;
    public Tile LastCheckPointTile;
    public Tile CurrentEndTile;
    public Tile SelectedTile;

    [Export]
    public MeshInstance3D Selecter;

    public override void _Ready()
    {
        Level_Manager.GenerateBoard += LevelStart;
        Level_Manager.AddToBoard += AddToBoard;
        base._Ready();
        LoadPieces();
    }

    public void LevelStart()
    {
        GD.PrintErr("Board Generating");
        GenerateMap();
        PopulateTileshWithTracks(Board);
    }
    public void AddToBoard()
    {
        AppendtoBoard(10);
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
                FirstStartTile = tileHolder;
                Board.Add(tileHolder);

            }
            else if (i == BoardLength - 1)
            {
                Tile tileHolder = new Tile(i, TileType.End);
                AddChild(tileHolder);
                tileHolder.GlobalPosition = position;
                LastCheckPointTile = tileHolder;
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

    public void AppendtoBoard(int aditionaLength)
    {
        List<Tile> NewTiles = new List<Tile>();

        LastCheckPointTile = CurrentEndTile;
        int NewBoardLength = BoardLength + aditionaLength;
        int CheckpointStart = LastCheckPointTile.TileID;
        for (int i = CheckpointStart; i < NewBoardLength; i++)
        {

            Vector3 position = GlobalPosition + new Vector3(0, 0, i * (CellSize * Margin));
            if (i == CheckpointStart)
            {
                Board[CheckpointStart].ClearTrack();
                Board[CheckpointStart].Tile_Type = TileType.Checkpoint;
                NewTiles.Add(Board[CheckpointStart]);

            }

            else if (i == NewBoardLength - 1)
            {
                Tile tileHolder = new Tile(i, TileType.End);
                AddChild(tileHolder);
                tileHolder.GlobalPosition = position;
                CurrentEndTile = tileHolder;
                NewTiles.Add(tileHolder);
            }
            else
            {
                Tile tileHolder = new Tile(i, TileType.Block);
                AddChild(tileHolder);
                tileHolder.GlobalPosition = position;
                NewTiles.Add(tileHolder);
            }
        }
        PopulateTileshWithTracks(NewTiles);

        Board[CheckpointStart] = NewTiles[0];
        NewTiles.RemoveAt(0);
        Board.AddRange(NewTiles);

        BoardLength = NewBoardLength;


    }

    public void PopulateTileshWithTracks(List<Tile> BoardSegment)
    {

        foreach (Tile t in BoardSegment)
        {
            TrackPiece piece = GetTrackPiece(t);
            if (piece != null)
            {
                t.Track = piece;
                t.Track.Position = t.Position;
                if(piece.PieceType == TrackPieceType.End)
                {
                    Level_Manager.GoalArea = piece.GetNode("Area3D") as Area3D;
                }
            }
            else
            {
                GD.PrintErr("ERROR!");
            }

        }
    }


    public TrackPiece GetTrackPiece(Tile t)
    {
        try
        {
            TrackPieceType needed = t.NeededPiece();
            List<int> possiblePieces = new List<int>();

            for (int i = 0; i < PreloadPieces.Count(); i++)
            {

                if (PreloadPieces[i].PieceType == needed)
                {
                    possiblePieces.Add(i);
                }
            }


            Random rand = new Random();

            if (possiblePieces.Count() > 0)
            {
                var scene = (TrackPiece)TrackPieces[possiblePieces[rand.Next(0, possiblePieces.Count())]].Instantiate();
                scene.Position = t.Position;
                t.AddChild(scene);
                return scene;

            }
            else
            {
                GD.PrintErr($"PieceNotFound - {needed}");
                return null;
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr("Eror:" + ex.ToString());
            return null;
        }



    }

    public void LoadPieces()
    {
        for (int i = 0; i < TrackPieces.Count(); i++)
        {
            var test = TrackPieces[i].GetLocalScene();
            TrackPiece holder = (TrackPiece)(TrackPieces[i].Instantiate(PackedScene.GenEditState.Disabled));
            GD.Print("Load index (" + i + "), Type [" + holder.PieceType + "]," + TrackPieces[i].ToString());
            PreloadPieces.Add(holder);
        }
    }

    public void SelectTile(Tile tile)
    {
        SelectedTile = tile;
        
    }



}
