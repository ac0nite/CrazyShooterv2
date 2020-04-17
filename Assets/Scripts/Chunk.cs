using System;
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
     
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter " + other.name);
        if(EventPlayerEntered != null)
        {
            EventPlayerEntered.Invoke(this);
        }
        //EventPlayerEntered?.Invoke(this);
    }
}