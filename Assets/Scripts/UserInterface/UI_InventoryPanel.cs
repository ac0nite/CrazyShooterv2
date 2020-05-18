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

    private void Awake()
    {
        _playerCharacterController.Character.CharacterInventory.EventItemPickedUp += OnItemPickedUp;
        _playerCharacterController.Character.CharacterInventory.EventItemDropedDown += OnItemDropedDown;
    }

    private void OnDestroy()
    {
        if (_playerCharacterController != null)
        {
            _playerCharacterController.Character.CharacterInventory.EventItemPickedUp -= OnItemPickedUp;
            _playerCharacterController.Character.CharacterInventory.EventItemDropedDown -= OnItemDropedDown;
        }
    }

    private void OnItemPickedUp(InventoryItem item)
    {
        var ui_element = Instantiate(_uiInventoryItemElementPrefab);
        ui_element.transform.SetParent(_listContentParent);
        ui_element.SetInfo(item);
    }

    private void OnItemDropedDown(InventoryItem item)
    {
        
    }
}
