using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterMovemevtBehavior))]
[RequireComponent(typeof(Character))]
public class PlayerCharacterController : CharacterController
{
    private PlayerMovementBehavior _playerMovementBehavior;
    private void Awake()
    {
        Character = GetComponent<Character>();
        _playerMovementBehavior = GetComponent<PlayerMovementBehavior>();
        
        InputManager.Instance.EventShootingWeapon += OnShootingWeapon;
        InputManager.Instance.EventPlayerMovementDirectionChanged += OnPlayerMovementDirectionChanged;
        InputManager.Instance.EventPlayerSprintMode += OnPlayerSprintMode;
        InputManager.Instance.EventPlayerChangeWeapon += OnPlayerChangeWeapon;
        InputManager.Instance.EventPickUpItemButtonPressed += OnPickUpItemBtttonPressed;
        InputManager.Instance.EventThrowningGrenade += OnThrownigGrenade;
        InputManager.Instance.EventOpenInventoryItem += OnOpenInventoryItem;
        InputManager.Instance.EventPause += OnPause;
        InputManager.Instance.EventPlayerLookPointChanged += OnPlayerLookPointChanged;
        _playerMovementBehavior.EventRevivePlayer += OnRevivePlayer;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var weapons = Character.CharacterInventory.BusyItems
                .FindAll(i => (typeof(Weapon) == i.GetType() || typeof(Weapon) == i.GetType().BaseType));

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
            InputManager.Instance.EventPlayerLookPointChanged -= OnPlayerLookPointChanged;
        }
        _playerMovementBehavior.EventRevivePlayer -= OnRevivePlayer;
    }

    private void OnPlayerChangeWeapon(KeyCode keyCode)
    {
        //доступно ли то или иное оружий!!!

        Character.ChangeWeapon((WeaponType)(keyCode - 48));
    }

    private void OnPlayerMovementDirectionChanged(Vector3 targetDirection)
    {
        _playerMovementBehavior.ChangePlayerMovementDirectionChanged(targetDirection);
    }

    private void OnPlayerSprintMode(bool sprintMode)
    {
        _playerMovementBehavior.ChangePlayerSprintMode(sprintMode);
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
        _playerMovementBehavior.ChangePlayerMovementDirectionChanged(Vector3.zero);
    }

    private void OnPause(bool _active)
    {
        UIManager.Instance.ShowPanel(_active ? UI_PanelType.Pause : UI_PanelType.GamePlay);
    }

    private void OnPlayerLookPointChanged(Vector3 lookPoint)
    {
        _playerMovementBehavior.OnPlayerLookPointChanged(lookPoint);
    }

    private void OnRevivePlayer()
    {
        Debug.Log($"OnRevivePlayer!!!!!s");

        GetComponent<Rigidbody>().isKinematic = false;
        var colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
        
        Character.HealthComponent.ModifyHealth(Character.HealthComponent.MaxHealth);
        
        _playerMovementBehavior.RevivePlayer();
        
        Character.CharacterAnimator.SetAnimation(Character.CurrentWeapon.Type.GetIdAnimationTriggerName());
        
        //Character.ApplyWeapon(Character.CurrentWeapon);
        //Character.CurrentWeapon.Apply(Character);
        //var knife = Character.CharacterInventory.Items.Find(a => a.GetWeaponType() == WeaponType.Knife);
        //Character.ApplyWeapon((Weapon)knife);
    }
}
