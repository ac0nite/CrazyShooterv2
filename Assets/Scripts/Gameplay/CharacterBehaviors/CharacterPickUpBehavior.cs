using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;

public class CharacterPickUpBehavior : MonoBehaviour
{
    //public Action<Weapon> EventWeaponPickUp;
    private List<InventoryItem> _overlappedItems  = new List<InventoryItem>();

    public InventoryItem TryPickUpItem()
    {
        //Debug.Log($"TryPickUpWeapon.Count= {_overlappedItems.Count} ", this);
        if (_overlappedItems.Count > 0)
        {
            var item = _overlappedItems[0];
            _overlappedItems.Remove(item);
            return item;
        } 

        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter {this.gameObject.name} - {other.gameObject.name}", other);
        var item = other.GetComponentInParent<InventoryItem>();
        if (item != null)
        {
            //Debug.Log($"OnTriggerEnter ", other);
             _overlappedItems.Add(item);
            //show UI hint
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log($"OnTriggerExit ", other);
        var item = other.GetComponentInParent<InventoryItem>();
        if (item != null)
        {
            _overlappedItems.Remove(item);
            if (_overlappedItems.Count == 0)
            {
                //hide UI hint
            }
        }
    }
}
