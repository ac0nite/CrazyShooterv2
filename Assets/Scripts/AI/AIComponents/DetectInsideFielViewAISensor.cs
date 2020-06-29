using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;


public class DetectInsideFielViewAISensor : AISensor
{
    [SerializeField] public float Angle = 20f;
    [SerializeField] public float Distance = 20f;
    private RaycastHit[] _hits = new RaycastHit[20];
    public override bool IsTargetDetected(Transform target)
    {
        if ((transform.position - target.position).magnitude <= Distance)
        {
            var targetDir = target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(targetDir);
            var angle = Quaternion.Angle(transform.rotation, rotation);
            if (angle <= Angle)
            {
                //Debug.Log($"angle: {angle}");
                return DetectInsideObstacle(target);
            }
        }
        
        return false;
    }

    private bool DetectInsideObstacle(Transform target)
    {
        Array.Clear(_hits, 0, _hits.Length);
        
        var targetDir = new Vector3(target.transform.position.x, 0.5f, target.transform.position.z);
        var ray = new Ray(transform.position, targetDir - transform.position);
        Debug.DrawLine(ray.origin, ray.GetPoint(30f), Color.blue, 3f);
        
        var count = Physics.RaycastNonAlloc(ray,_hits, Distance, LayerMask.GetMask( "Player", "Enemies", "Obstacles"));
        var h = Physics.RaycastAll(ray, Distance, LayerMask.GetMask( "Player", "Enemies", "Obstacles"));
        if (count > 0)
        {
            var arr = _hits.ToList();
            arr.Sort((a,b)=> a.distance.CompareTo(b.distance));
            foreach (var hit in arr)
            {
                if (hit.distance > 0f)
                {
                    var obj = hit.collider.GetComponentInParent<PlayerCharacterController>();
                    if (obj != null)
                        return true;
                }
            }
        }
        return false;
    }
}
