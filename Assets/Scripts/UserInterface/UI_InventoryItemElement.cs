using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryItemElement : MonoBehaviour
{
    [SerializeField] private Text _itemName = null;

    public void SetInfo(InventoryItem item)
    {
        _itemName.text = item.Name;
    }
}
