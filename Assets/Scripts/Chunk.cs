using System;
using System.Collections.Generic;
using UnityEngine;
using PolygonCrazyShooter;

public class Chunk : MonoBehaviour
{
    public event Action<Chunk> EventPlayerEntered;

    //[SerializeField] private Direction _direction = Direction.Undefined; 
    [Serializable]
    public class DockPointSettings
    {
        [SerializeField] public Direction Direction = Direction.Undefined;
        [SerializeField] public Transform Point = null; 
    }

    public List<EnemySpawnPoint> EnemiesSpawnPoint { get { return _enemiesSpawnPoint; } }

    [SerializeField] private List<DockPointSettings> _dockPointSettings = null;
    [SerializeField] private List<EnemySpawnPoint> _enemiesSpawnPoint = null;
     
    public Dictionary<Direction, Transform> _dockPoints = new Dictionary<Direction, Transform>();
    public readonly Dictionary<Direction, Chunk> _neighbourChunks = new Dictionary<Direction, Chunk>();
     
    private void Awake()
    {
        foreach(var dp in _dockPointSettings)
        {
            _dockPoints[dp.Direction] = dp.Point;
        }
    }

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