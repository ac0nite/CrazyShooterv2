using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CurrentWeaponSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var inventoryItemElement = eventData.pointerDrag.gameObject.GetComponent<UI_InventoryItemElement>();
        if (inventoryItemElement != null)
        {
            inventoryItemElement.transform.SetParent(transform);
            inventoryItemElement.transform.localPosition = Vector3.zero;
        }
    }
}
