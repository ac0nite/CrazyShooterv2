using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UI_InventoryItemElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Text _itemName = null;
    [SerializeField] private CanvasGroup _canvasGroup = null;
    private Transform _initTransformParent = null;
    private Canvas _parentCanvas = null;
    public InventoryItem CurrentWeapon { get; private set; }

    public void SetInfo(InventoryItem item)
    {
        _itemName.text = item.Name;
        CurrentWeapon = item;

        //if (item.GetType() == typeof(Knife) || item.GetType() == typeof(Weapon))
        //    CurrentWeapon = item;
        //else
        //    CurrentWeapon = null;
    }

    private void Start()
    {
        _parentCanvas = GetComponentInParent<Canvas>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _initTransformParent = transform.parent;

        transform.SetParent(_parentCanvas.transform);
        _canvasGroup.blocksRaycasts = false;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;

        if (transform.parent == _parentCanvas.transform)
        {
            transform.SetParent(_initTransformParent);
            transform.localPosition = Vector3.zero;
        }
    }
}
