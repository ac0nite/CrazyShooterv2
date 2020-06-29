using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AISensor : MonoBehaviour
{
    public abstract bool IsTargetDetected(Transform target);
}
