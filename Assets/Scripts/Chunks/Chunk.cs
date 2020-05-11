using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolygonCrazyShooter;

public class Chunk : MonoBehaviour
{
    public event Action<Chunk> EventPlayerEntered;

    public List<ChunkDockPoint> DockPoints { get { return _dockPointSettings; } }
    public List<EnemySpawnPoint> EnemiesSpawnPoint { get { return _enemiesSpawnPoint; } }
    public List<BoxCollider> BoundingColliders { get { return _boundingColliders; }  }

    [SerializeField] private List<ChunkDockPoint> _dockPointSettings = null;
    [SerializeField] private List<EnemySpawnPoint> _enemiesSpawnPoint = null;
    [SerializeField] private List<BoxCollider> _boundingColliders = null;

    public readonly Dictionary<ChunkDockPoint, Chunk> NeighbourChunks = new Dictionary<ChunkDockPoint, Chunk>();
    private void Update()
    {
        // var enemies = GetComponentsInChildren<Enemy>();
        // for (int i = 0; i < enemies.Length;i++)
        // {
        //         if (enemies[i].IsDead)
        //     {
        //         StartCoroutine(DestroyEnemy(enemies[i], 10f));
        //         //Destroy(enemies[i].gameObject);
        //     }
        // }
    }
    IEnumerator DestroyEnemy(Enemy enemy, float wait)
    {
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        //Debug.Log($"Enemy dead: {enemy.name.ToString()}");
        yield return new WaitForSeconds(wait);
        if (enemy != null) 
            Destroy(enemy.gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
//        Debug.Log("OnTriggerEnter " + other.name);
        EventPlayerEntered?.Invoke(this);
        //EventPlayerEntered?.Invoke(this);
    }
}