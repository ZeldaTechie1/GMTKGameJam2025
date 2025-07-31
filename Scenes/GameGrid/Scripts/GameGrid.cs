using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
public partial class GameGrid : Node3D
{

    [Export]
    int Rows = 3;

    [Export]
    int Columns = 5;

    [Export]
    float Margin = .02f;

    [Export]
    int CellSize = 1;



    List<Tile> tiles = new List<Tile>();
    List<MeshInstance3D> floor = new List<MeshInstance3D>();

    [Export]
    PackedScene[] TrackPieces;

    List<TrackPiece> PreloadPieces=new List<TrackPiece>();

    List<Tile> CurrentPath = new List<Tile>();
    List<Tile> TotalPath = new List<Tile>();

    List<List<Tile>> TotalGridTiles = new List<List<Tile>>();

    public Tile FirstStartTile;
    public Tile CurrentStartTile;
    public Tile CurrentEndTile;

    GridPathGeneration GPGen;

    public override void _Ready()
    {
        base._Ready();
        LoadPieces();
        CreateFirstGrid();
        GPGen = new GridPathGeneration(TotalGridTiles);
        CurrentPath = GPGen.FindPath(CurrentStartTile,CurrentEndTile);
        PopulateCurrentPathWithTracks();

    }
    public void LoadPieces() 
    {
        for (int i = 0; i < TrackPieces.Count(); i++)
        {
            var test = TrackPieces[i].GetLocalScene();
            TrackPiece holder= (TrackPiece)(TrackPieces[i].Instantiate(PackedScene.GenEditState.Disabled));
            GD.PrintErr("Load index (" +i+ "), Type ["+holder.TrackType+"]," +TrackPieces[i].ToString());
            PreloadPieces.Add(holder);
        }
    }

    void CreateFirstGrid()
    {
        List<List<Tile>> TempGridTiles = new List<List<Tile>>();
        for (int C = 0; C < Columns; C++)
        {
            List<Tile> RowTiles = new List<Tile>();
            for (int R = 0; R < Rows; R++)
            {
                Vector3 position = GlobalPosition + new Vector3(C * (CellSize * Margin), 0,R * (CellSize * Margin));
                //RowPositions.Add(position);

                if ((R == (Columns - 1) / 2) && C == 0)
                {
                    try
                    {
                        Tile tileHolder = new Tile(TileType.Start, new Vector2(C, R), 10);
                        RowTiles.Add(tileHolder);
                        AddChild(tileHolder);
                        tileHolder.GlobalPosition = position;
                        GD.PrintErr("BEANS");
                        CurrentStartTile = tileHolder;
                        FirstStartTile = tileHolder;

                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr(ex.ToString());
                    }

                }
                else
                {
                    try
                    {
                        Random rand = new Random();

                        Tile tileHolder = new Tile(TileType.Empty, new Vector2(C, R), rand.Next(1, 2));
                        RowTiles.Add(tileHolder);
                        AddChild(tileHolder);
                        tileHolder.GlobalPosition = position;

                    }
                    catch
                    {
                        GD.PrintErr("error");
                    }
                }

                

            }
            //GridSpaces.Add(RowPositions);
            TempGridTiles.Add(RowTiles);
            GD.Print("GridTiles:" + TempGridTiles.Count);
        }
        CurrentEndTile = ChooseEndTile(TempGridTiles[TempGridTiles.Count - 1]);

        for (int C = 0; C < TempGridTiles.Count; C++)
        {
            for (int R = 0; R < TempGridTiles[C].Count; R++)
            {
                if (CurrentEndTile.GridPosition == new Vector2(C, R))
                {
                    TempGridTiles[C][R].Tile_Type = TileType.End;
                    TempGridTiles[C][R].DistanceToTarget = 0;
                }
                else
                {
                    TempGridTiles[C][R].DistanceToTarget = Math.Abs(TempGridTiles[C][R].Position.DistanceSquaredTo(CurrentEndTile.Position));
                }
                TempGridTiles[C][R].CellSize = CellSize;
            }
        }
        TotalGridTiles.AddRange(TempGridTiles);
        GD.PrintErr("Grid Temp: R("+TempGridTiles.Count+") C("+TempGridTiles[0].Count+")");
        GD.PrintErr("Grid Total: R("+TotalGridTiles.Count+") C("+TotalGridTiles[0].Count+")");
    }

    public void ChoosePath()
    {
        List<Tile> holder = GPGen.FindPath(CurrentStartTile, CurrentEndTile);

        CurrentPath = holder;
        TotalPath.AddRange(holder);
    }



    public static Tile ChooseEndTile(List<Tile> tilesInLastRow)
    {
        int index = -1;
        if (tilesInLastRow.Count > 0)
        {
            Random random = new Random();

            index = random.Next(0, tilesInLastRow.Count);
        }
        if (index > -1)
        {
            return tilesInLastRow[index];
        }
        else return null;
    }

    public void PopulateCurrentPathWithTracks()
    {
        if (CurrentPath.Count == 0)
        {
            GD.PrintErr("NO Path");
        }
        foreach (Tile t in CurrentPath)
            {
                TrackPiece piece = GetTrackPiece(t);
                if (piece != null)
                {
                    t.Track = piece;
                    t.AddChild(piece);
                    t.OrientTrackPiece();
                }

            }
    }

    public TrackPiece GetTrackPiece(Tile t)
    {   try
        {
            TrackPieceType needed = t.NeededPiece();
            GD.PrintErr("Tile (" + t.GridPosition.X + "," +t.GridPosition.Y + ") Cant Determine Track Peice ["+needed+"]");
            List<int> possiblePieces = new List<int>();
    
            for(int i= 0; i < PreloadPieces.Count();i++)
            {
                GD.PrintErr("preloads:" + PreloadPieces[i].TrackType);
                if (PreloadPieces[i].TrackType == needed)
                {
                    possiblePieces.Add(i);
                }
            }


            Random rand = new Random();

            if (possiblePieces.Count() > 0)
            {
                var scene =(TrackPiece) TrackPieces[possiblePieces[rand.Next(0, possiblePieces.Count())]].Instantiate();
                return scene ;

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

    


       void ClearDisplayGrid()
    {
        foreach (Node x in GetChildren())
        {
            x.QueueFree();
        }

    }

    public override void _Process(double delta)
    {


    }
    

    

}
