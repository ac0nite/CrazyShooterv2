using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"KnifeAttack {this.gameObject.name} - {other.gameObject.name}", other);
        var knife = gameObject.GetComponentInParent<Character>().GetComponentInChildren<Knife>();
        if (knife != null)
        {
            knife.OnPullingTriggerEnter(other);
        }
    }
}
