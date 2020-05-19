using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;

public class UI_InventoryPanel : MonoBehaviour
{
    [SerializeField] private UI_InventoryItemElement _uiInventoryItemElementPrefab = null;
    [SerializeField] private RectTransform _listContentParent = null;
    [SerializeField] private PlayerCharacterController _playerCharacterController = null;

    private Dictionary<InventoryItem, UI_InventoryItemElement> _itemToListElementDicrinory =
        new Dictionary<InventoryItem, UI_InventoryItemElement>();
    private void Awake()
    {
        _playerCharacterController.Character.CharacterInventory.EventItemPickedUp += OnItemPickedUp;
        _playerCharacterController.Character.CharacterInventory.EventItemDropedDown += OnItemDropedDown;
        foreach (var item in _playerCharacterController.Character.CharacterInventory.Items)
        {
            SpawnInventoryItemElement(item);
        }
    }

    private void OnDestroy()
    {
        if (_playerCharacterController != null)
        {
            _playerCharacterController.Character.CharacterInventory.EventItemPickedUp -= OnItemPickedUp;
            _playerCharacterController.Character.CharacterInventory.EventItemDropedDown -= OnItemDropedDown;
        }
    }

    private void SpawnInventoryItemElement(InventoryItem item)
    {
        var ui_element = Instantiate(_uiInventoryItemElementPrefab);
        ui_element.transform.SetParent(_listContentParent);
        ui_element.SetInfo(item);
        
        // if (_itemToListElementDicrinory[item] != null)
        //     _itemToListElementDicrinory[item] = ui_element;
        // else
            _itemToListElementDicrinory.Add(item, ui_element);
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
}
