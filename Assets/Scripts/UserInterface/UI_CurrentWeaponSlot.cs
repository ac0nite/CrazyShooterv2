using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CurrentWeaponSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool _fieldPrecision = true;
    [SerializeField] private GameObject _objectPrecision = null;
    [SerializeField] private float _precision = 0;

    [SerializeField] private GameObject _objectDamage = null;
    [SerializeField] private float _damage = 0;

    [SerializeField] private GameObject _objectActiveSlot = null;
    [SerializeField] private bool _activeSlot = false;

    [SerializeField] private GameObject _objectSelect = null;
    [SerializeField] private bool _select = false;

    [SerializeField] private String _nameSlotType = null;
    [SerializeField] private Text _objectNameSlotType = null;

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

    private void Awake()
    {
        SetActive(_objectPrecision, _fieldPrecision);
        SetActive(_objectActiveSlot, !_activeSlot);
        SetActive(_objectSelect, _select);
        _objectNameSlotType.text = _nameSlotType;
    }

    public void SetActive(GameObject obj, bool active)
    {
        obj.SetActive(active);
    }
}
