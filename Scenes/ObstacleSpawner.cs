using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

public partial class ObstacleSpawner : Node3D
{
    [Export]LevelManager levelManager;
    [Export]GameBoardManager gameBoardManager;
    [Export]PackedScene[] obstacles;

    public override void _Ready()
    {
        levelManager.RoundStarted += LevelManager_RoundStarted;
    }

    private void LevelManager_RoundStarted()
    {
        SpawnObstacle();
    }

    public void SpawnObstacle()
    {
        List<Tile> validTiles = new List<Tile>();

        foreach(Tile tile in gameBoardManager.GetBoard())
        {
            if (tile.Tile_Type == TileType.Empty)
                continue;

            if (tile.Track.ItemSpawnPoints.Count() == 0)
                continue;

            validTiles.Add(tile);
        }

        bool spawnedObstacle = false;
        while(validTiles.Count > 0 && !spawnedObstacle)
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
            }).Where(x=> x!= null).ToList();

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

            if(validSpawnPoints.Count == 0)
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
}
