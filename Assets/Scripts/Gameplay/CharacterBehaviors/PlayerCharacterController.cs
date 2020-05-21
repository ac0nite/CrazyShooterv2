using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterMovemevtBehavior))]
[RequireComponent(typeof(Character))]
public class PlayerCharacterController : MonoBehaviour
{
    public Character Character = null;
    private CharacterMovemevtBehavior _characterMovemevtBehavior;
    private void Awake()
    {
        Character = GetComponent<Character>();
        _characterMovemevtBehavior = GetComponent<CharacterMovemevtBehavior>();
        
        InputManager.Instance.EventShootingWeapon += OnShootingWeapon;
        InputManager.Instance.EventPlayerMovementDirectionChanged += OnPlayerMovementDirectionChanged;
        InputManager.Instance.EventPlayerSprintMode += OnPlayerSprintMode;
        InputManager.Instance.EventPlayerChangeWeapon += OnPlayerChangeWeapon;
        InputManager.Instance.EventPickUpItemButtonPressed += OnPickUpItemBtttonPressed;
        InputManager.Instance.EventThrowningGrenade += OnThrownigGrenade;
        InputManager.Instance.EventOpenInventoryItem += OnOpenInventoryItem;
        InputManager.Instance.EventPause += OnPause;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var weapons = Character.CharacterInventory.Items.FindAll(i => i.GetType() == typeof(Weapon));
            var currenWeaponIndex = weapons.IndexOf(Character.CurrentWeapon);
            var nextWeapon = weapons[(currenWeaponIndex+1) % weapons.Count];
            nextWeapon.Apply(Character);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            var item = 
                Character.CharacterInventory.Items.Find(i => i != Character.CurrentWeapon);
            if (item != null)
            {
                Character.CharacterInventory.Drop(item);
            }
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            var r = Character.GetComponent<Rigidbody>();
            if (r != null)
            {
                Debug.Log("Key U!!!");
                r.AddForce(new Vector3(0.1f,0.5f,0.1f) * 10f * r.mass, ForceMode.Acceleration);
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Character.GetComponent<CharacterHealthComponent>()?.ModifyHealth(-10f);
        }
    }

    private void OnDestroy()
    {
        if (InputManager.TryInstance != null)
        {
            InputManager.Instance.EventShootingWeapon -= OnShootingWeapon;
            InputManager.Instance.EventPlayerMovementDirectionChanged -= OnPlayerMovementDirectionChanged;
            InputManager.Instance.EventPlayerSprintMode -= OnPlayerSprintMode;
            InputManager.Instance.EventPlayerChangeWeapon -= OnPlayerChangeWeapon;
            InputManager.Instance.EventPickUpItemButtonPressed -= OnPickUpItemBtttonPressed;
            InputManager.Instance.EventOpenInventoryItem -= OnOpenInventoryItem;
            InputManager.Instance.EventPause -= OnPause;
        }
    }

    private void OnPlayerChangeWeapon(KeyCode keyCode)
    {
        //доступно ли то или иное оружий!!!

        Character.ChangeWeapon((WeaponType)(keyCode - 48));
    }

    private void OnPlayerMovementDirectionChanged(Vector3 targetDirection)
    {
        _characterMovemevtBehavior.ChangePlayerMovementDirectionChanged(targetDirection);
    }

    private void OnPlayerSprintMode(bool sprintMode)
    {
        _characterMovemevtBehavior.ChangePlayerSprintMode(sprintMode);
    }
    private void OnShootingWeapon(bool isShootingStarter)
    {
        if(!isShootingStarter)
            return;
        
        Character.Shoot();
    }

    private void OnPickUpItemBtttonPressed()
    {
        Character.TryPickUpItem();
    }

    private void OnThrownigGrenade()
    {
        Character.ThrowGrenade();
    }

    private void OnOpenInventoryItem(bool _active)
    {
        UIManager.Instance.ShowPanel(_active ? UI_PanelType.Inventory : UI_PanelType.GamePlay);
        _characterMovemevtBehavior.ChangePlayerMovementDirectionChanged(Vector3.zero);
    }

    private void OnPause(bool _active)
    {
        UIManager.Instance.ShowPanel(_active ? UI_PanelType.Pause : UI_PanelType.GamePlay);
    }
}
