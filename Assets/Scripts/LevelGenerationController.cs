using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using PolygonCrazyShooter;

public class LevelGenerationController : MonoBehaviour
{
    public Action<Character> EventCharacterSpawned;
    private void Start()
    {
        //generation first chunk
        var initChunk = GenerateRandomChunk(ChunkDockPointType.Undefined,  false);

        initChunk.transform.position = Vector3.zero;

        GenerationNeighbourChunks(initChunk);
    }


    private Chunk GenerateRandomChunk(ChunkDockPointType type, bool generateAdditionalElements = true)
    {
        var chunks = SettingsManager.Instance.Chunks;
        var suitableChunks = type != ChunkDockPointType.Undefined
            ? chunks.FindAll(c => c.DockPoints.Exists(t => t.Type == type))
            : SettingsManager.Instance.Chunks;

        var prefab = suitableChunks[UnityEngine.Random.Range(0, suitableChunks.Count)];
        //var prefab = chunks[UnityEngine.Random.Range(0, chunks.Count)];

        var spawnChunk = Instantiate(prefab);

        if (generateAdditionalElements)
            EnemiesSpawn(spawnChunk);

        spawnChunk.EventPlayerEntered += OnPlayerEnteredChunk;

        return spawnChunk;
    }

    private void GenerationNeighbourChunks(Chunk center)
    {
        //обходить варианты направлений, смотреть, есть ли созданный чанк для этого направления
        //если нет, создаём
        foreach (var centerDockPoint in center.DockPoints)
        {
            if (center.NeighbourChunks.ContainsKey(centerDockPoint))
                continue;

            var suitableChunks = centerDockPoint.Type != ChunkDockPointType.Undefined
                ? SettingsManager.Instance.Chunks.FindAll(c => c.DockPoints.Exists(t => t.Type == centerDockPoint.Type))
                : SettingsManager.Instance.Chunks;

            while(suitableChunks.Count > 0)
            {
                var randomIndex = UnityEngine.Random.Range(0, suitableChunks.Count);
                var prefab = suitableChunks[randomIndex];
                suitableChunks.RemoveAt(randomIndex);

                var neighbour = Instantiate(prefab);

                var neighbourDockPoint = neighbour.DockPoints.Find(p => p.Type == centerDockPoint.Type);

                var target = Quaternion.LookRotation(-centerDockPoint.transform.forward, centerDockPoint.transform.up);
                var rotationOffset = target * Quaternion.Inverse(neighbourDockPoint.transform.rotation);
                neighbour.transform.rotation *= rotationOffset;

                //выровнять соседа относительно центрального
                var offset = centerDockPoint.transform.position - neighbourDockPoint.transform.position;
                neighbour.transform.position += offset;

                if(IsEnoughSpaceForChunk(neighbour))
                {
                    EnemiesSpawn(neighbour);
                    neighbour.EventPlayerEntered += OnPlayerEnteredChunk;
                    center.NeighbourChunks[centerDockPoint] = neighbour;
                    neighbour.NeighbourChunks[neighbourDockPoint] = center;
                    break;
                }
                else
                {
                    Destroy(neighbour.gameObject);
                }
            }


            //var neighbour = GenerateRandomChunk(centerDockPoint.Type);

            //var neighbourDockPoint = neighbour.DockPoints.Find(p => p.Type == centerDockPoint.Type);

            ////Quaternion target = Quaternion.LookRotation(-centerDockPoint.transform.forward, centerDockPoint.transform.up);
            ////Quaternion source = Quaternion.LookRotation(neighbourDockPoint.transform.forward, neighbourDockPoint.transform.up);
            ////var anglesRotation = target.eulerAngles - source.eulerAngles;
            ////neighbour.transform.rotation *= Quaternion.Euler(anglesRotation);

            //var target = Quaternion.LookRotation(-centerDockPoint.transform.forward, centerDockPoint.transform.up);
            //var rotationOffset = target * Quaternion.Inverse(neighbourDockPoint.transform.rotation);
            //neighbour.transform.rotation *= rotationOffset;

            ////выровнять соседа относительно центрального
            //var offset = centerDockPoint.transform.position - neighbourDockPoint.transform.position;
            //neighbour.transform.position += offset;

            //center.NeighbourChunks[centerDockPoint] = neighbour;
            //neighbour.NeighbourChunks[neighbourDockPoint] = center;

            //check for neighbour
        }

        //BindNeighbourDockPointChanks(curent_chunk);
    }

    private bool IsEnoughSpaceForChunk(Chunk chunk)
    {
        foreach(var boundingCollider in chunk.BoundingColliders)
        {
            //var center = boundingCollider.transform.position +  boundingCollider.transform.TransformPoint(boundingCollider.center);
            var center = boundingCollider.transform.position;
            var selColliders = Physics.OverlapBox(
                center, 
                boundingCollider.size / 2f, 
                boundingCollider.transform.rotation, 
                LayerMask.GetMask("Chunk"),
                QueryTriggerInteraction.Collide);

            if (selColliders.Length != 0)
                return false;
        }

        return true;
    }
    private void EnemiesSpawn(Chunk chunk)
    {
        foreach(var spawnPosition in chunk.EnemiesSpawnPoint)
        {
            if(UnityEngine.Random.Range(0,2) == 1)
            {
                var settingsManager = SettingsManager.Instance;

                //var suitableEnemy = settingsManager.Enemies.FindAll(go => (go.EnemyType & spawnPosition.EnemyType) == go.EnemyType);
                var suitableEnemy = settingsManager.Enemies.FindAll(go =>
                {
                    EnemyType et = go.EnemyType & spawnPosition.EnemyType;
                    //return go.EnemyType == spawnPosition.EnemyType;
                    return et != 0;
                });
                //var enemyPrefab = settingsManager.Enemies[UnityEngine.Random.Range(0, settingsManager.Enemies.Count)];
                var enemyPrefab = suitableEnemy[UnityEngine.Random.Range(0, suitableEnemy.Count)];

                var enemyObject = Instantiate(enemyPrefab, spawnPosition.transform.position, spawnPosition.transform.rotation);
                
                EventCharacterSpawned?.Invoke(enemyObject);
                
                enemyObject.transform.SetParent(chunk.transform);
            }
        }
    }

    //private void BindNeighbourDockPointChanks(Chunk center)
    //{
    //    Chunk local_center = null;

    //    foreach (Direction direction in Enum.GetValues(typeof(Direction)))
    //    {
    //        if (!center._neighbourChunks.ContainsKey(direction))
    //            continue;

    //        local_center = center._neighbourChunks[direction];

    //        Vector3 c = local_center.transform.position;
    //        Vector3 d = new Vector3(c.x, c.y, c.z + 20);

    //        int layerMask = 1 << 9;
    //        Collider[] collider = Physics.OverlapBox(d, local_center.transform.localScale / 2, Quaternion.identity, layerMask);
    //        //Chunk ch = collider[0].GetComponents<Chunk>;w

    //        if (direction == Direction.Up)
    //        {
    //            local_center._neighbourChunks[Direction.Right] = center._neighbourChunks[Direction.UpRight];
    //            local_center._neighbourChunks[Direction.RightDown] = center._neighbourChunks[Direction.Right];
    //            local_center._neighbourChunks[Direction.Left] = center._neighbourChunks[Direction.LeftUp];
    //            local_center._neighbourChunks[Direction.DownLeft] = center._neighbourChunks[Direction.Left];
    //        }
    //        else if (direction == Direction.UpRight)
    //        {
    //            local_center._neighbourChunks[Direction.Left] = center._neighbourChunks[Direction.Up];
    //            local_center._neighbourChunks[Direction.Down] = center._neighbourChunks[Direction.Right];
    //        }
    //        else if (direction == Direction.LeftUp)
    //        {
    //            local_center._neighbourChunks[Direction.Right] = center._neighbourChunks[Direction.Up];
    //            local_center._neighbourChunks[Direction.Down] = center._neighbourChunks[Direction.Left];
    //        }
    //        else if (direction == Direction.RightDown)
    //        {
    //            local_center._neighbourChunks[Direction.Up] = center._neighbourChunks[Direction.Right];
    //            local_center._neighbourChunks[Direction.Left] = center._neighbourChunks[Direction.Down];
    //        }
    //        else if (direction == Direction.DownLeft)
    //        {
    //            local_center._neighbourChunks[Direction.Up] = center._neighbourChunks[Direction.Left];
    //            local_center._neighbourChunks[Direction.Right] = center._neighbourChunks[Direction.Down];
    //        }
    //        else if (direction == Direction.Right)
    //        {
    //            local_center._neighbourChunks[Direction.Up] = center._neighbourChunks[Direction.UpRight];
    //            local_center._neighbourChunks[Direction.LeftUp] = center._neighbourChunks[Direction.Up];
    //            local_center._neighbourChunks[Direction.DownLeft] = center._neighbourChunks[Direction.Down];
    //            local_center._neighbourChunks[Direction.Down] = center._neighbourChunks[Direction.RightDown];
    //        }
    //        else if (direction == Direction.Left)
    //        {
    //            local_center._neighbourChunks[Direction.Up] = center._neighbourChunks[Direction.LeftUp];
    //            local_center._neighbourChunks[Direction.UpRight] = center._neighbourChunks[Direction.Up];
    //            local_center._neighbourChunks[Direction.RightDown] = center._neighbourChunks[Direction.Down];
    //            local_center._neighbourChunks[Direction.Down] = center._neighbourChunks[Direction.DownLeft];
    //        }
    //        else if (direction == Direction.Down)
    //        {
    //            local_center._neighbourChunks[Direction.Right] = center._neighbourChunks[Direction.RightDown];
    //            local_center._neighbourChunks[Direction.UpRight] = center._neighbourChunks[Direction.Right];
    //            local_center._neighbourChunks[Direction.Left] = center._neighbourChunks[Direction.DownLeft];
    //            local_center._neighbourChunks[Direction.LeftUp] = center._neighbourChunks[Direction.Left];
    //        }

    //    }

    //    //foreach(Direction direction in Enum.GetValues(typeof(Direction)))
    //    //{
    //    //    if (center._neighbourChunks.ContainsKey(direction))
    //    //        continue;

    //    //    switch(direction)
    //    //    {
    //    //        case Direction.Up:
    //    //            center._neighbourChunks[Direction.Right] = center._neighbourChunks[Direction.Right.GetOpposite()];
    //    //            center._neighbourChunks[Direction.Right.GetOpposite()] = center._neighbourChunks[Direction.Right];
    //    //            break;
    //    //    }
    //    //}
    //}

    private void OnPlayerEnteredChunk(Chunk chunk)
    {
        GenerationNeighbourChunks(chunk);
    }
}
