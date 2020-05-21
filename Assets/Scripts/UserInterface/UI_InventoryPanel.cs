using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;

public class UI_InventoryPanel : UI_Panel
{
    [SerializeField] private UI_InventoryItemElement _uiInventoryItemElementPrefab = null;
    [SerializeField] private RectTransform _listContentParent = null;
    [SerializeField] private PlayerCharacterController _playerCharacterController = null;
    [SerializeField] private UI_CurrentWeaponSlot _uiCurrentWeaponSlot = null;

    private Dictionary<InventoryItem, UI_InventoryItemElement> _itemToListElementDicrinory =
        new Dictionary<InventoryItem, UI_InventoryItemElement>();
    private void Awake()
    {
        _playerCharacterController.Character.CharacterInventory.EventItemPickedUp += OnItemPickedUp;
        _playerCharacterController.Character.CharacterInventory.EventItemDropedDown += OnItemDropedDown;

        _uiCurrentWeaponSlot.EventWeaponAssigned += OnWeaponAssigned;

        foreach (var item in _playerCharacterController.Character.CharacterInventory.Items)
        {
            var ui_element = SpawnInventoryItemElement(item);
            if (_playerCharacterController.Character.CurrentWeapon == ui_element.CurrentWeapon)
            {
                _uiCurrentWeaponSlot.AssignElementSlot(ui_element);
            }
        }
    }

    private void OnDestroy()
    {
        if (_playerCharacterController != null)
        {
            _playerCharacterController.Character.CharacterInventory.EventItemPickedUp -= OnItemPickedUp;
            _playerCharacterController.Character.CharacterInventory.EventItemDropedDown -= OnItemDropedDown;
        }

        _uiCurrentWeaponSlot.EventWeaponAssigned -= OnWeaponAssigned;
    }

    private UI_InventoryItemElement SpawnInventoryItemElement(InventoryItem item)
    {
        var ui_element = Instantiate(_uiInventoryItemElementPrefab);
        ui_element.transform.SetParent(_listContentParent);
        ui_element.SetInfo(item);
        
        _itemToListElementDicrinory.Add(item, ui_element);

        return ui_element;
    }

    private void RemoveInventoryItemElement(InventoryItem item)
    {
        Destroy(_itemToListElementDicrinory[item].gameObject);
        _itemToListElementDicrinory.Remove(item);
    }
    private void OnItemPickedUp(InventoryItem item)
    {
        SpawnInventoryItemElement(item);
    }

    private void OnItemDropedDown(InventoryItem item)
    {
        RemoveInventoryItemElement(item);
    }

    private void OnWeaponAssigned(UI_InventoryItemElement _inventoryNewItemElement, UI_InventoryItemElement _inventoryOldItemElement)
    {
        _inventoryNewItemElement.CurrentWeapon.Apply(_playerCharacterController.Character);
        _inventoryOldItemElement?.transform.SetParent(_listContentParent);
    }

    public void OnResumeButtonClick()
    {
        UIManager.Instance.ShowPanel(UI_PanelType.GamePlay);
    }
}
