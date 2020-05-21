using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CurrentWeaponSlot : MonoBehaviour, IDropHandler
{
    /// <summary>
    /// new old
    /// </summary>
    public event Action<UI_InventoryItemElement, UI_InventoryItemElement> EventWeaponAssigned;

    private UI_InventoryItemElement _uiCurrentInventoryItemElement = null;

    public void AssignElementSlot(UI_InventoryItemElement _uiInventoryItemElement)
    {
        _uiCurrentInventoryItemElement = _uiInventoryItemElement;
        _uiInventoryItemElement.transform.SetParent(transform);
        _uiInventoryItemElement.transform.localPosition = Vector3.zero;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var inventoryItemElement = eventData.pointerDrag.gameObject.GetComponentInParent<UI_InventoryItemElement>();
        if (inventoryItemElement != null)
        {
            if (inventoryItemElement.CurrentWeapon.GetType() != typeof(Weapon) && inventoryItemElement.CurrentWeapon.GetType() != typeof(Knife))
                return;

            if (inventoryItemElement.CurrentWeapon != null)
                EventWeaponAssigned?.Invoke(inventoryItemElement, _uiCurrentInventoryItemElement);

            AssignElementSlot(inventoryItemElement);
        }
    }
}
