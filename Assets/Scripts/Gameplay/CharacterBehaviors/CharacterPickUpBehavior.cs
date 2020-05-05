using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPickUpBehavior : MonoBehaviour
{
    //public Action<Weapon> EventWeaponPickUp;
    private List<Weapon> _overlappedWeapons  = new List<Weapon>();

    public Weapon TryPickUpWeapon()
    {
        Debug.Log($"TryPickUpWeapon.Count= {_overlappedWeapons.Count} ", this);
        if (_overlappedWeapons.Count > 0)
        {
            var weapon = _overlappedWeapons[0];
            _overlappedWeapons.Remove(weapon);
            return weapon;
        }

        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        var weaponComponent = other.GetComponentInParent<Weapon>();
        if (weaponComponent != null)
        {
            Debug.Log($"OnTriggerEnter ", other);
             _overlappedWeapons.Add(weaponComponent);
            //show UI hint
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"OnTriggerExit ", other);
        var weaponComponent = other.GetComponentInParent<Weapon>();
        if (weaponComponent != null)
        {
            _overlappedWeapons.Remove(weaponComponent);
            if (_overlappedWeapons.Count == 0)
            {
                //hide UI hint
            }
        }
    }
}
