using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIMotor : MonoBehaviour
{
    public Vector3 MovementTargetPoint { get; private set; }
    public bool IsFreeLookEnabled { get; private set; }
    public Vector3 FreeLookTargetPoint { get; private set; }

    public virtual void SetMovementTarget(Vector3 targeMovementToPoint)
    {    
        MovementTargetPoint = targeMovementToPoint;
    }

    public virtual void SetFreeLookTarget(bool IsFreeLookTarget, Vector3 lookTarget)
    {
        IsFreeLookEnabled = IsFreeLookTarget;
        FreeLookTargetPoint = lookTarget;
    }
}
