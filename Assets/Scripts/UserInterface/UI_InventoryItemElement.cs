using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InventoryItemElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Text _itemName = null;
    [SerializeField] private CanvasGroup _canvasGroup = null;

    public void SetInfo(InventoryItem item)
    {
        _itemName.text = item.Name;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(GetComponentInParent<Canvas>().transform);
        _canvasGroup.blocksRaycasts = false;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
    }
}
