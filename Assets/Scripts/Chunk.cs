using System;
using System.Collections.Generic;
using UnityEngine;
using PolygonCrazyShooter;

public class Chunk : MonoBehaviour
{
    //[SerializeField] private Direction _direction = Direction.Undefined; 
    [Serializable]
    public class DockPointSettings
    {
        [SerializeField] public Direction Direction = Direction.Undefined;
        [SerializeField] public Transform Point = null; 
    }

    [SerializeField] List<DockPointSettings> _dockPointSettings = null;

    private Dictionary<Direction, Transform> _dockPoint = new Dictionary<Direction, Transform>();
    private readonly Dictionary<Direction, Chunk> _neighbourChunks = new Dictionary<Direction, Chunk>();
     
    private void Awake()
    {
        foreach(var dp in _dockPointSettings)
        {
            _dockPoint[dp.Direction] = dp.Point;
        }
    }
}