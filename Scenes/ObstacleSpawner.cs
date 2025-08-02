using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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
        if(levelManager.currentRound > 1)
        {
            //SpawnObstacle();
        }
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

        int randTile = Random.Shared.Next(0, validTiles.Count());
        Tile obstacle = validTiles[randTile];
        List<ItemSpawnPoint> spawnPoints = obstacle.Track.ItemSpawnPoints.ToList();
        List<(LevelItemSize, LevelItemLocation)> obstacleTypes = new List<(LevelItemSize, LevelItemLocation)>();
        foreach(ItemSpawnPoint spawnPoint in spawnPoints)
        {
            if (obstacleTypes.Contains((spawnPoint.PointSize, spawnPoint.PointLocation)))
                continue;
            obstacleTypes.Add((spawnPoint.PointSize, spawnPoint.PointLocation));
        }
    }
}
