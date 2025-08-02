using Godot;

public partial class Tile : Node3D
{


    [Export]
    public TileType Tile_Type = TileType.Empty;
    public TrackPiece Track;
    public int TileID { get; set; }


  
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

    public void SpawnLevelItem(LevelItemSize size, LevelItemLocation location,PackedScene ItemToSpawn) 
    {
        if (Track != null)
        {
            ItemSpawnPoint spawnPoint =Track.GetRandomItemSpawnPoint(size, location);
            if (spawnPoint != null)
            {
                var scene = (LevelItem)ItemToSpawn.Instantiate();
                scene.Position = spawnPoint.Position;
                Track.AddChild(scene);
                
            }
          
        }

    }



}


