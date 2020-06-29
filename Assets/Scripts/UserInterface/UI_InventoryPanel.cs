using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;
using UnityEngine.Serialization;

public class UI_InventoryPanel : UI_Panel
{
    [SerializeField] private UI_InventoryItemElement _uiInventoryItemElementPrefab = null;
    [SerializeField] private RectTransform _listContentParent = null;
    [SerializeField] private PlayerCharacterController _playerCharacterController = null;
    //[SerializeField] private UI_CurrentWeaponSlot _uiCurrentWeaponSlot = null;

    private Dictionary<InventoryItem, UI_InventoryItemElement> _itemToListElementDicrinory =
        new Dictionary<InventoryItem, UI_InventoryItemElement>();
    
    [SerializeField] private List<UI_CurrentWeaponSlot> _currentInventories = new List<UI_CurrentWeaponSlot>();
    public Dictionary<WeaponType, UI_CurrentWeaponSlot> CurrentInventoriesSlot  = new Dictionary<WeaponType, UI_CurrentWeaponSlot>();
    private void Awake()
    {
        foreach (var currentInventory in _currentInventories)
        {
            CurrentInventoriesSlot.Add(currentInventory.TypeWeaponSlot, currentInventory);
            CurrentInventoriesSlot[currentInventory.TypeWeaponSlot].EventWeaponAssigned += OnWeaponAssigned;
            CurrentInventoriesSlot[currentInventory.TypeWeaponSlot].EventAssigned += OnAssigned;
            CurrentInventoriesSlot[currentInventory.TypeWeaponSlot].EventAddHelthing += OnAddHelthing;
            CurrentInventoriesSlot[currentInventory.TypeWeaponSlot].EventRemoveItem += OnRemoveItem;
        }
        
        _playerCharacterController.Character.CharacterInventory.EventItemPickedUp += OnItemPickedUp;
        _playerCharacterController.Character.CharacterInventory.EventItemDropedDown += OnItemDropedDown;
        _playerCharacterController.Character.HealthComponent.EventHealthChange += OnHealthChange;
        _playerCharacterController.Character.EventThrowGrenade += OnThrowGrenade;

        //_uiCurrentWeaponSlot.EventWeaponAssigned += OnWeaponAssigned;
        CurrentInventoriesSlot[WeaponType.Medicine].SetHealth(_playerCharacterController.Character.HealthComponent.Health);
        foreach (var item in _playerCharacterController.Character.CharacterInventory.Items)
        {
            var ui_element = SpawnInventoryItemElement(item);

            // if (ui_element.CurrentWeapon.GetType() == typeof(Medicine))
            // {
            //     CurrentInventoriesSlot[item.GetWeaponType()].AddHealth(_playerCharacterController.Character.HealthComponent.Health);   
            // }
            if (item.GetWeaponType() == WeaponType.Knife && !CurrentInventoriesSlot[item.GetWeaponType()].BusySlot())
            {
                CurrentInventoriesSlot[item.GetWeaponType()].AssignElementSlot(ui_element);
            }
        }
    }
    private void OnDestroy()
    {
        if (_playerCharacterController != null)
        {
            _playerCharacterController.Character.CharacterInventory.EventItemPickedUp -= OnItemPickedUp;
            _playerCharacterController.Character.CharacterInventory.EventItemDropedDown -= OnItemDropedDown;
            _playerCharacterController.Character.HealthComponent.EventHealthChange -= OnHealthChange;
            _playerCharacterController.Character.EventThrowGrenade -= OnThrowGrenade;
        }

       // _uiCurrentWeaponSlot.EventWeaponAssigned -= OnWeaponAssigned;
        foreach (var slot in CurrentInventoriesSlot)
        {
            CurrentInventoriesSlot[slot.Key].EventWeaponAssigned -= OnWeaponAssigned;
            CurrentInventoriesSlot[slot.Key].EventAssigned -= OnAssigned;
            CurrentInventoriesSlot[slot.Key].EventAddHelthing -= OnAddHelthing;
            CurrentInventoriesSlot[slot.Key].EventRemoveItem += OnRemoveItem;
        }
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
        //_playerCharacterController.Character.CharacterInventory.BusyItems.Remove(item);
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
        //CurrentInventoriesSlot[_inventoryNewItemElement.CurrentWeapon.GetWeaponType()] = 
        _inventoryNewItemElement.CurrentWeapon.Apply(_playerCharacterController.Character);
        _inventoryOldItemElement?.transform.SetParent(_listContentParent);
        if(_inventoryOldItemElement != null)
            _playerCharacterController.Character.CharacterInventory.BusyItems.Remove(_inventoryOldItemElement.CurrentWeapon);
    }

    private void OnRemoveItem(UI_InventoryItemElement _inventoryOldItemElement)
    {
        RemoveInventoryItemElement(_inventoryOldItemElement.CurrentWeapon);
    }
    private void OnAssigned(UI_InventoryItemElement _inventoryNewItemElement)
    {
        Debug.Log($"Add: {_inventoryNewItemElement.CurrentWeapon.Name}");
        _playerCharacterController.Character.CharacterInventory.BusyItems.Add(_inventoryNewItemElement.CurrentWeapon);
    }

    private void OnResumeButtonClick()
    {
        UIManager.Instance.ShowPanel(UI_PanelType.GamePlay);
    }

    private void OnAddHelthing(float _health)
    {
        _playerCharacterController.Character.HealthComponent.ModifyHealth(_health);
    }

    private void OnHealthChange(CharacterHealthComponent _characterHealth, float _health)
    {
        CurrentInventoriesSlot[WeaponType.Medicine].SetHealth(_health);
    }

    private void OnThrowGrenade()
    {
        CurrentInventoriesSlot[WeaponType.Grenade].RemGrenade();
    }
}
