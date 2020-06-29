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
        
        InventorySpawn(spawnChunk);

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
                    InventorySpawn(neighbour);
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
            {
//                Debug.Log($"1. chunk", chunk);
//                Debug.Log($"2. boundingCollider",boundingCollider);
                return false;
            }
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

    private void InventorySpawn(Chunk chunk)
    {
        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            foreach (var spawnPosition in chunk.InventorySpawnPoints)
            {
                var settingsManager = SettingsManager.Instance;
                var suitableInventory = settingsManager.Inventories.FindAll(i => i.GetWeaponType() == spawnPosition.Type);
                var inventoryPrefab = suitableInventory[UnityEngine.Random.Range(0, suitableInventory.Count)];
                var inventoryObject = Instantiate(inventoryPrefab, spawnPosition.transform.position, spawnPosition.transform.rotation);
            }
        }
    }
    
    private void OnPlayerEnteredChunk(Chunk chunk)
    {
        GenerationNeighbourChunks(chunk);
    }
}
