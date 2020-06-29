using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectInsideRadiusAISensor : AISensor
{
    [SerializeField, Tooltip("In meters")] public float DetectionRadius = 10f;
    public override bool IsTargetDetected(Transform target)
    {
        return (transform.position - target.position).magnitude <= DetectionRadius;
    }
}
