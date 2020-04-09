using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using PolygonCrazyShooter;

public class LevelGenerationController : MonoBehaviour
{
    private void Start()
    {
        //generation first chunk
        var initChunk = GenerateRandomChunk(false);

        initChunk.transform.position = Vector3.zero;

        GenerationNeighbourChunks(initChunk);
    }


    private Chunk GenerateRandomChunk(bool generateAdditionalElements = true)
    {
        var chunks = SettingsManager.Instance.Chunks;
        var prefab = chunks[UnityEngine.Random.Range(0, chunks.Count)];

        var spawnChunk = Instantiate(prefab);

        if (generateAdditionalElements)
            EnemiesSpawn(spawnChunk);

        spawnChunk.EventPlayerEntered += OnPlayerEnteredChunk;

        return spawnChunk;
    }

    private void GenerationNeighbourChunks(Chunk curent_chunk)
    {
        //обходить варианты направлений, смотреть, есть ли созданный чанк для этого направления
        //если нет, создаём
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            if (direction == Direction.Undefined)
                continue;

            if (curent_chunk._neighbourChunks.ContainsKey(direction))
            {
                continue;
            }


           
            Vector3 a = curent_chunk._dockPoints[direction].transform.position;
            Vector3 b = curent_chunk._dockPoints[direction.GetOpposite()].transform.position;
            Vector3 c = a - b;
            Vector3 d = curent_chunk.transform.position + c;

            int layerMask = 1 << 9;
            Collider[] collider = Physics.OverlapSphere(d, 0.5f, layerMask);
            if (collider.Length > 0)
                continue;

            var neighbour = GenerateRandomChunk();
            curent_chunk._neighbourChunks[direction] = neighbour;
            neighbour._neighbourChunks[direction.GetOpposite()] = curent_chunk;


            //выровнять соседа относительно центрального
            Transform centerChunkDockPoint = curent_chunk._dockPoints[direction];
            Transform neighbourChunkDockPoint = neighbour._dockPoints[direction.GetOpposite()];
            var offset = centerChunkDockPoint.position - neighbourChunkDockPoint.position;

            neighbour.transform.position += offset;

            //EnemiesSpawn(spawnChunk);
        }

        //BindNeighbourDockPointChanks(curent_chunk);
    }

    private void EnemiesSpawn(Chunk chunk)
    {
        foreach(var spawnPosition in chunk.EnemiesSpawnPoint)
        {
            if(UnityEngine.Random.Range(0,2) == 1)
            {
                var settingsManager = SettingsManager.Instance;

                var suitableEnemy = settingsManager.Enemies.FindAll(go => go.EnemyType == spawnPosition.EnemyType);
                //var enemyPrefab = settingsManager.Enemies[UnityEngine.Random.Range(0, settingsManager.Enemies.Count)];
                var enemyPrefab = suitableEnemy[UnityEngine.Random.Range(0, suitableEnemy.Count)];

                var enemyObject = Instantiate(enemyPrefab, spawnPosition.transform.position, spawnPosition.transform.rotation);
                enemyObject.transform.SetParent(chunk.transform);
            }
        }
    }

    private void BindNeighbourDockPointChanks(Chunk center)
    {
        Chunk local_center = null;

        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            if (!center._neighbourChunks.ContainsKey(direction))
                continue;

            local_center = center._neighbourChunks[direction];

            Vector3 c = local_center.transform.position;
            Vector3 d = new Vector3(c.x, c.y, c.z + 20);

            int layerMask = 1 << 9;
            Collider[] collider = Physics.OverlapBox(d, local_center.transform.localScale / 2, Quaternion.identity, layerMask);
            //Chunk ch = collider[0].GetComponents<Chunk>;w

            if (direction == Direction.Up)
            {
                local_center._neighbourChunks[Direction.Right] = center._neighbourChunks[Direction.UpRight];
                local_center._neighbourChunks[Direction.RightDown] = center._neighbourChunks[Direction.Right];
                local_center._neighbourChunks[Direction.Left] = center._neighbourChunks[Direction.LeftUp];
                local_center._neighbourChunks[Direction.DownLeft] = center._neighbourChunks[Direction.Left];
            }
            else if (direction == Direction.UpRight)
            {
                local_center._neighbourChunks[Direction.Left] = center._neighbourChunks[Direction.Up];
                local_center._neighbourChunks[Direction.Down] = center._neighbourChunks[Direction.Right];
            }
            else if (direction == Direction.LeftUp)
            {
                local_center._neighbourChunks[Direction.Right] = center._neighbourChunks[Direction.Up];
                local_center._neighbourChunks[Direction.Down] = center._neighbourChunks[Direction.Left];
            }
            else if (direction == Direction.RightDown)
            {
                local_center._neighbourChunks[Direction.Up] = center._neighbourChunks[Direction.Right];
                local_center._neighbourChunks[Direction.Left] = center._neighbourChunks[Direction.Down];
            }
            else if (direction == Direction.DownLeft)
            {
                local_center._neighbourChunks[Direction.Up] = center._neighbourChunks[Direction.Left];
                local_center._neighbourChunks[Direction.Right] = center._neighbourChunks[Direction.Down];
            }
            else if (direction == Direction.Right)
            {
                local_center._neighbourChunks[Direction.Up] = center._neighbourChunks[Direction.UpRight];
                local_center._neighbourChunks[Direction.LeftUp] = center._neighbourChunks[Direction.Up];
                local_center._neighbourChunks[Direction.DownLeft] = center._neighbourChunks[Direction.Down];
                local_center._neighbourChunks[Direction.Down] = center._neighbourChunks[Direction.RightDown];
            }
            else if (direction == Direction.Left)
            {
                local_center._neighbourChunks[Direction.Up] = center._neighbourChunks[Direction.LeftUp];
                local_center._neighbourChunks[Direction.UpRight] = center._neighbourChunks[Direction.Up];
                local_center._neighbourChunks[Direction.RightDown] = center._neighbourChunks[Direction.Down];
                local_center._neighbourChunks[Direction.Down] = center._neighbourChunks[Direction.DownLeft];
            }
            else if (direction == Direction.Down)
            {
                local_center._neighbourChunks[Direction.Right] = center._neighbourChunks[Direction.RightDown];
                local_center._neighbourChunks[Direction.UpRight] = center._neighbourChunks[Direction.Right];
                local_center._neighbourChunks[Direction.Left] = center._neighbourChunks[Direction.DownLeft];
                local_center._neighbourChunks[Direction.LeftUp] = center._neighbourChunks[Direction.Left];
            }

        }

        //foreach(Direction direction in Enum.GetValues(typeof(Direction)))
        //{
        //    if (center._neighbourChunks.ContainsKey(direction))
        //        continue;

        //    switch(direction)
        //    {
        //        case Direction.Up:
        //            center._neighbourChunks[Direction.Right] = center._neighbourChunks[Direction.Right.GetOpposite()];
        //            center._neighbourChunks[Direction.Right.GetOpposite()] = center._neighbourChunks[Direction.Right];
        //            break;
        //    }
        //}
    }

    private void OnPlayerEnteredChunk(Chunk chunk)
    {
        GenerationNeighbourChunks(chunk);
    }
}
