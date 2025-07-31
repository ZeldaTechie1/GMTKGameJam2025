using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public partial class GameGrid : Node3D
{

    [Export]
    int Width = 3;

    [Export]
    int Height = 5;

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
        for (int i = 0; i < TrackPieces.Count(); i++)
        {
            PreloadPieces.Add((TrackPiece)TrackPieces[i].Instantiate());
        }
        
        CreateFirstGrid();
        GPGen = new GridPathGeneration(TotalGridTiles);

    }

    void CreateFirstGrid()
    {
        List<List<Tile>> TempGridTiles = new List<List<Tile>>();
        for (int w = 0; w < Width; w++)
        {
            List<Vector3> RowPositions = new List<Vector3>();
            List<Tile> RowTiles = new List<Tile>();
            for (int h = 0; h < Height; h++)
            {
                Vector3 position = GlobalPosition + new Vector3(w * (CellSize * Margin), 0, h * (CellSize * Margin));
                //RowPositions.Add(position);

                if (TempGridTiles.Count() > 0)
                {
                    if ((w == (Width - 1) / 2) && h == 0)
                    {
                        try
                        {
                            Tile tileHolder = new Tile(TileType.Start, new Vector2(w,h),10);
                            RowTiles.Add(tileHolder);
                            tileHolder.GlobalPosition = position;
                            CurrentStartTile = tileHolder;
                            FirstStartTile = tileHolder;
                        }
                        catch
                        {

                        }

                    }
                    else
                    {
                        try
                        {
                            Random rand = new Random();

                            Tile tileHolder = new Tile(TileType.Empty,new Vector2(w,h),rand.Next(1,2));
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

            }
            //GridSpaces.Add(RowPositions);
            TempGridTiles.Add(RowTiles);
            GD.Print("GridTiles:" + TempGridTiles.Count);
        }
        CurrentEndTile = ChooseEndTile(TempGridTiles[TempGridTiles.Count - 1]);

        for (int w = 0; w < TempGridTiles.Count; w++)
        {
            for (int h = 0; h < TempGridTiles[w].Count; h++)
            {
                if (CurrentEndTile.GridPosition == new Vector2(w, h))
                {
                    TempGridTiles[w][h].Tile_Type = TileType.End;
                    TempGridTiles[w][h].DistanceToTarget = 0;

                }
                else
                {
                    TempGridTiles[w][h].DistanceToTarget = Math.Abs(TempGridTiles[w][h].Position.DistanceSquaredTo(CurrentEndTile.Position));
                }
                TempGridTiles[w][h].CellSize = CellSize;
            }
        }
        TotalGridTiles.AddRange(TempGridTiles);
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
        foreach (Tile t in CurrentPath)
        {
            TrackPiece piece = GetTrackPiece(t);
            if (piece != null)
            {
                t.Track = piece;
                t.AddChild(piece);
                
            }
            
        }
    }

    public TrackPiece GetTrackPiece(Tile t)
    {   try
        {
            TrackPeiceType needed = t.NeededPeice();
            List<int> possiblePieces = new List<int>();
    
            for(int i= 0; i < PreloadPieces.Count();i++)
            {
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
                return null;
            }
        }
        catch
        {
            return null;
        }
      
     

    }

    


    void CreateDisplayGrid()
    {
        for (int w = 0; w < Width; w++)
        {
            for (int h = 0; h < Height; h++)
            {
                MeshInstance3D mesh = new MeshInstance3D();
                var shape = new PlaneMesh();
                shape.Size = new Vector2(CellSize, CellSize);
                mesh.Mesh = shape;

                AddChild(mesh);
                mesh.GlobalPosition = GlobalPosition + new Vector3(w * (CellSize * Margin), 0, h * (CellSize * Margin));

            }

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
