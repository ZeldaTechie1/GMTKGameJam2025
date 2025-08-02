using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

public partial class ItemSpawner : Node3D
{
    [Export] LevelManager levelManager;
    [Export] GameBoardManager gameBoardManager;

    [ExportCategory("Level Items")]
    [Export] PackedScene[] obstacles;
    List<LevelItem> PreLoadedObstacles = new List<LevelItem>();
    [Export] PackedScene[] aid;
    List<LevelItem> PreLoadedAid = new List<LevelItem>();

    public override void _Ready()
    {

        levelManager.RoundStarted += LevelManager_RoundStarted;
    }

    private void LevelManager_RoundStarted()
    {
        //SpawnObstacle();
    }

    public void LoadItems()
    {
        for (int i = 0; i < obstacles.Count(); i++)
        {
            var test = obstacles[i].GetLocalScene();
            LevelItem holder = (LevelItem)(obstacles[i].Instantiate());
            PreLoadedObstacles.Add(holder);
        }
        for (int i = 0; i < aid.Count(); i++)
        {
            var test = aid[i].GetLocalScene();
            LevelItem holder = (LevelItem)(obstacles[i].Instantiate());
            PreLoadedAid.Add(holder);
        }
    }

    public void SpawnRandomObstacle()
    {
        List<Tile> validTiles = new List<Tile>();

        foreach (Tile tile in gameBoardManager.GetBoard())
        {
            if (tile.Tile_Type == TileType.Empty || tile.Tile_Type == TileType.Start || tile.Tile_Type == TileType.End)
                continue;

            if (tile.Track.ItemSpawnPoints.Count() == 0)
                continue;

            validTiles.Add(tile);
        }

        bool spawnedObstacle = false;
        while (validTiles.Count > 0 && !spawnedObstacle)
        {
            int randTile = Random.Shared.Next(0, validTiles.Count());
            Tile pickedTile = validTiles[randTile];
            GD.Print($"Picked Tile at index {randTile} with track type {pickedTile.Track.PieceType}");
            List<ItemSpawnPoint> allSpawnPoints = pickedTile.Track.ItemSpawnPoints.ToList();
            List<(LevelItemSize, LevelItemLocation)> obstacleTypes = new List<(LevelItemSize, LevelItemLocation)>();
            foreach (ItemSpawnPoint spawnPoint in allSpawnPoints)
            {
                if (obstacleTypes.Contains((spawnPoint.PointSize, spawnPoint.PointLocation)))
                    continue;
                obstacleTypes.Add((spawnPoint.PointSize, spawnPoint.PointLocation));
            }

            List<LevelItem> potentialObstacles = obstacles.Select((x) =>
            {
                LevelItem newItem = x.Instantiate() as LevelItem;
                if (obstacleTypes.Contains((newItem.ItemSize, newItem.ItemLocation)))
                {
                    return newItem;
                }
                newItem.QueueFree();
                return null;
            }).Where(x => x != null).ToList();

            if (potentialObstacles.Count == 0)
            {
                validTiles.RemoveAt(randTile);
                continue;
            }

            int randObstacle = Random.Shared.Next(0, potentialObstacles.Count);
            LevelItem obstacle = potentialObstacles[randObstacle];
            GD.Print($"Picked obstacle at index {randObstacle} named {obstacle.Name}");
            List<ItemSpawnPoint> validSpawnPoints = allSpawnPoints.Select(x =>
            {
                if (x.PointLocation == obstacle.ItemLocation && x.PointSize == obstacle.ItemSize)
                    return x;

                return null;
            }).Where(x => x != null).ToList();

            if (validSpawnPoints.Count == 0)
            {
                validTiles.RemoveAt(randTile);
                continue;
            }

            int randSpawnPoint = Random.Shared.Next(0, validSpawnPoints.Count);
            GD.Print($"Picked Spawn Point at index {randSpawnPoint} named {validSpawnPoints[randSpawnPoint]}");
            validSpawnPoints[randSpawnPoint].AddChild(obstacle);
            potentialObstacles.RemoveAt(randObstacle);
            GD.Print($"Spawned Obstacle! - {obstacle.Name}");
            foreach (LevelItem item in potentialObstacles)
            {
                item.QueueFree();
            }
            spawnedObstacle = true;
        }

        if (validTiles.Count == 0)
        {
            GD.Print("Start digging in your butt twin!");
        }
    }

    public PackedScene GetRandomObstacle()
    {
        Random rand = new Random();
        if (obstacles.Count() > 0)
        {
            return obstacles[rand.Next(0, obstacles.Count())];
        }
        else return null;
    }


    public PackedScene GetRandomAid()
    {
        Random rand = new Random();
        if (aid.Count() > 0)
        {
            return aid[rand.Next(0, aid.Count())];
        }
        else return null;
    }

    public PackedScene GetRandomLevelItem()
    {
        List<PackedScene> Items = new List<PackedScene>();
        Random rand = new Random();
        foreach (PackedScene x in obstacles)
        {
            Items.Add(x);
        }
        foreach (PackedScene x in aid)
        {
            Items.Add(x);
        }
        if (Items.Count > 0)
        {
            return Items[rand.Next(0, Items.Count)];
        }
        else return null;
    }

    public LevelItem SpawnObstacle(PackedScene Obst, Tile tile, bool RandomTile = false)
    {
        LevelItem obstacle = (LevelItem)Obst.Instantiate();
        if (obstacle != null)
        {
            if (RandomTile)
            {
                tile = gameBoardManager.GetRandomTileForItemSpawn(obstacle.ItemLocation, obstacle.ItemSize);

            }
            if (tile != null)
            {
                ItemSpawnPoint spawnPoint = tile.Track.GetRandomItemSpawnPoint(obstacle.ItemSize, obstacle.ItemLocation);
                if (spawnPoint != null)
                {
                    spawnPoint.AddChild(obstacle);
                    obstacle.GlobalPosition = spawnPoint.GlobalPosition;
                    return obstacle;
                }
            }
        }
        obstacle.QueueFree();
        return null;


    }
    public LevelItem SpawnAid(PackedScene Aid, Tile tile, bool RandomTile = false)
    {
        LevelItem AID = (LevelItem)Aid.Instantiate();
        if (AID != null)
        {
            if (RandomTile)
            {
                tile = gameBoardManager.GetRandomTileForItemSpawn(AID.ItemLocation, AID.ItemSize);

            }
            if (tile != null)
            {
                ItemSpawnPoint spawnPoint = tile.Track.GetRandomItemSpawnPoint(AID.ItemSize, AID.ItemLocation);
                if (spawnPoint != null)
                {
                    spawnPoint.AddChild(AID);
                    AID.GlobalPosition = spawnPoint.GlobalPosition;
                    return AID;
                }
            }

        }

        AID.QueueFree();
        return null;
    }

    public bool CardSpawn(Tile tile, PackedScene Aid, PackedScene Obstacle = null, bool randomAid = false, bool getRandomObstacle = false, bool EffectsRandomTile = false, bool DrawBackToRandomTile = false)
    {
        LevelItem AIDPoint;
        LevelItem ObstaclePoint;

        if (randomAid)
        {
            if (getRandomObstacle || Obstacle == null)
            {
                AIDPoint = SpawnAid(GetRandomAid(), tile, EffectsRandomTile);
                ObstaclePoint = SpawnObstacle(GetRandomObstacle(), tile, DrawBackToRandomTile);
                if (AIDPoint != null && ObstaclePoint != null)
                {
                    return true;
                }
                else
                {
                    if (AIDPoint != null)
                    {
                        GD.Print("Aid");
                        AIDPoint.QueueFree();
                    }
                    if(ObstaclePoint!=null)
                    {
                        GD.Print("Obstacle");
                        ObstaclePoint.QueueFree();
                    }
                    return false;
                }
            }
            if (getRandomObstacle && Obstacle == null)
            {
                AIDPoint = SpawnAid(GetRandomAid(), tile, EffectsRandomTile);
                ObstaclePoint = SpawnObstacle(GetRandomObstacle(), tile, DrawBackToRandomTile);
                if (AIDPoint != null && ObstaclePoint != null)
                {
                    return true;
                }
                else
                {
                    if (AIDPoint != null)
                    {
                        GD.Print("Aid");
                        AIDPoint.QueueFree();
                    }
                    if(ObstaclePoint!=null)
                    {
                        GD.Print("Obstacle");
                        ObstaclePoint.QueueFree();
                    }
                    return false;
                }

            }
            else if (Obstacle != null)
            {
                AIDPoint = SpawnAid(GetRandomAid(), tile, EffectsRandomTile);
                ObstaclePoint = SpawnObstacle(Obstacle, tile, DrawBackToRandomTile);
                if (AIDPoint != null && ObstaclePoint != null)
                {
                    return true;
                }
                else
                {
                    if (AIDPoint != null)
                    {
                        AIDPoint.QueueFree();
                    }
                    if(ObstaclePoint!=null)
                    {
                        ObstaclePoint.QueueFree();
                    }
                    return false;
                }
            }
            else
            {
                AIDPoint = SpawnAid(GetRandomAid(), tile, EffectsRandomTile);
                if (AIDPoint != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        if (!randomAid && Aid == null)
        {

            if (getRandomObstacle || Obstacle == null)
            {
                AIDPoint = SpawnAid(GetRandomAid(), tile, EffectsRandomTile);
                ObstaclePoint = SpawnObstacle(GetRandomObstacle(), tile, DrawBackToRandomTile);
                if (AIDPoint != null && ObstaclePoint != null)
                {
                    return true;
                }
                else
                {
                    if (AIDPoint != null)
                    {
                        GD.Print("Aid");
                        AIDPoint.QueueFree();
                    }
                    if(ObstaclePoint!=null)
                    {
                        GD.Print("Obstacle");
                        ObstaclePoint.QueueFree();
                    }
                    return false;
                }
            }
            if (getRandomObstacle && Obstacle == null)
            {
                AIDPoint = SpawnAid(GetRandomAid(), tile, EffectsRandomTile);
                ObstaclePoint = SpawnObstacle(GetRandomObstacle(), tile, DrawBackToRandomTile);
                if (AIDPoint != null && ObstaclePoint != null)
                {
                    return true;
                }
                else
                {
                    if (AIDPoint != null)
                    {
                        GD.Print("Aid");
                        AIDPoint.QueueFree();
                    }
                    if(ObstaclePoint!=null)
                    {
                        GD.Print("Obstacle");
                        ObstaclePoint.QueueFree();
                    }
                    return false;
                }

            }
            else if (Obstacle != null)
            {
                AIDPoint = SpawnAid(GetRandomAid(), tile, EffectsRandomTile);
                ObstaclePoint = SpawnObstacle(Obstacle, tile, DrawBackToRandomTile);
                if (AIDPoint != null && ObstaclePoint != null)
                {
                    return true;
                }
                else
                {
                    if (AIDPoint != null)
                    {
                        GD.Print("Aid");
                        AIDPoint.QueueFree();
                    }
                    if(ObstaclePoint!=null)
                    {
                        GD.Print("Obstacle");
                        ObstaclePoint.QueueFree();
                    }
                    return false;
                }
            }
            else
            {
                AIDPoint = SpawnAid(GetRandomAid(), tile, EffectsRandomTile);
                if (AIDPoint != null)
                {
                    return true;
                }
                else
                {
                    AIDPoint.QueueFree();

                    return false;
                }
            }
        }
        else
        {

            if (getRandomObstacle || Obstacle == null)
            {
                AIDPoint = SpawnAid(Aid, tile, EffectsRandomTile);
                ObstaclePoint = SpawnObstacle(GetRandomObstacle(), tile, DrawBackToRandomTile);
                if (AIDPoint != null && ObstaclePoint != null)
                {
                    return true;
                }
                else
                {
                    if (AIDPoint != null)
                    {
                        GD.Print("Aid");
                        AIDPoint.QueueFree();
                    }
                    if(ObstaclePoint!=null)
                    {
                        GD.Print("Obstacle");
                        ObstaclePoint.QueueFree();
                    }
                    return false;
                }
            }
            else if (getRandomObstacle && Obstacle == null)
            {
                AIDPoint = SpawnAid(GetRandomAid(), tile, EffectsRandomTile);
                ObstaclePoint = SpawnObstacle(GetRandomObstacle(), tile, DrawBackToRandomTile);
                if (AIDPoint != null && ObstaclePoint != null)
                {
                    return true;
                }
                else
                {
                    if (AIDPoint != null)
                    {
                        GD.Print("Aid");
                        AIDPoint.QueueFree();
                    }
                    if(ObstaclePoint!=null)
                    {
                        GD.Print("Obstacle");
                        ObstaclePoint.QueueFree();
                    }
                    return false;
                }

            }
            else if (Obstacle != null)
            {
                AIDPoint = SpawnAid(GetRandomAid(), tile, EffectsRandomTile);
                ObstaclePoint = SpawnObstacle(Obstacle, tile, DrawBackToRandomTile);
                if (AIDPoint != null && ObstaclePoint != null)
                {
                    return true;
                }
                else
                {
                    if (AIDPoint != null)
                    {
                        GD.Print("Aid");
                        AIDPoint.QueueFree();
                    }
                    if(ObstaclePoint!=null)
                    {
                        GD.Print("Obstacle");
                        ObstaclePoint.QueueFree();
                    }
                    return false;
                }
            }
            else
            {
                AIDPoint = SpawnAid(GetRandomAid(), tile, EffectsRandomTile);
                if (AIDPoint != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        return false;
    }

}
