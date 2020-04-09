using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PolygonCrazyShooter
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] public EnemyType EnemyType = EnemyType.Undefined;
    }
}