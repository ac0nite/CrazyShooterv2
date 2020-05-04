using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPickUpBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var weaponComponent = other.GetComponent<Weapon>();
        if (weaponComponent != null)
        {
            
        }
    }
}
