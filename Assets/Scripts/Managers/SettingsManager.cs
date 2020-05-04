using System;
using PolygonCrazyShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : SingletoneGameObject<SettingsManager>
{
    // [SerializeField] private Direction _direction = Direction.Undefined;
    //    public static SettingsManager Instance;

    [SerializeField] public List<Chunk> Chunks = null;
    [SerializeField] public List<Enemy> Enemies = null;
    [SerializeField] public List<Weapon> Weapons = null;

  protected override void Awake()
    {
        base.Awake();
        
    }
}    
