using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public readonly List<InventoryItem> Items = new List<InventoryItem>();

    public void PickUp(InventoryItem item)
    { 
        Items.Add(item);

        item.transform.SetParent(transform); //имеем ввиду что находимся под персонажем
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        item.PickUp();
    }

    public void Drop(InventoryItem item)
    {
        Items.Remove(item);
        item.transform.SetParent(null);

        item.Drop();
    }
}
