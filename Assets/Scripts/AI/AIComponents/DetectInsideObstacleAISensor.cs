using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DetectInsideObstacleAISensor : AISensor
{
    [SerializeField] private AISensor DetectInsideFielViewSensor = null;
    private RaycastHit[] _hits = new RaycastHit[20];
    public override bool IsTargetDetected(Transform target)
    {
        var ray = new Ray(transform.position, target.transform.position - transform.position);
        
        var count = Physics.RaycastNonAlloc(ray,_hits, ((DetectInsideFielViewAISensor)DetectInsideFielViewSensor).Distance);
        if (count > 0)
        {
            var arr = _hits.ToList();
            arr.Sort((a,b)=> a.distance.CompareTo(b.distance));

            var obj = arr[0].collider?.GetComponentInParent<PlayerCharacterController>();
            if (obj != null)
                return true;   
        }

        return false;
    }
}
