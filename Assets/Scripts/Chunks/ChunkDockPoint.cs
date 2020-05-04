using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PolygonCrazyShooter
{
    //[RequireComponent(typeof(SphereCollider))]
    public class ChunkDockPoint : MonoBehaviour
    {
        [SerializeField] public ChunkDockPointType Type = ChunkDockPointType.Undefined;


        public bool IsOverlappedWithAnotherCollider()
        {
            return Physics.OverlapSphere(transform.position, 0.1f, LayerMask.GetMask("ChunkDockPoints")).Length >= 2;
        }
    }
}
