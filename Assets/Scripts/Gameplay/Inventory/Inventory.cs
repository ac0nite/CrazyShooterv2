using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> Items = new List<InventoryItem>();
    public List<InventoryItem> BusyItems = new List<InventoryItem>();

    public event Action<InventoryItem> EventItemPickedUp;
    public event Action<InventoryItem> EventItemDropedDown;
    
    public void PickUp(InventoryItem item)
    { 
//        Debug.Log("PickUp");
        Items.Add(item);

        item.transform.SetParent(transform); //имеем ввиду что находимся под персонажем
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        item.PickUp();

        if (EventItemPickedUp != null)
        {
            Debug.Log("EventItemPickedUp");
            EventItemPickedUp(item);
        }
        //EventItemPickedUp?.Invoke(item);
    }

    public void Drop(InventoryItem item)
    {
        Items.Remove(item);
        BusyItems.Remove(item);
        
        item.transform.SetParent(null);

        item.Drop();

        EventItemDropedDown?.Invoke(item);
    }
}
