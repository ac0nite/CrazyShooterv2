using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PolygonCrazyShooter
{
    public class Enemy : Character
    {
        [SerializeField] public EnemyType EnemyType = EnemyType.Undefined;

        protected override void Update()
        {
            // if(this.gameObject != null && IsDead)
            //     StartCoroutine(DestroyCharacter());
        }
        
    }
}