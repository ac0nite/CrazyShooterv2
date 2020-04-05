using PolygonCrazyShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    // [SerializeField] private Direction _direction = Direction.Undefined;
    public static SettingsManager Instance;

    [SerializeField] public List<Chunk> Chunks = null;

    private void Awake()
    {
        Instance = this;
    }
}
