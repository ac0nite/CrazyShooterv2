using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;

[RequireComponent(typeof(CharacterMovemevtBehavior))]
[RequireComponent(typeof(Character))]
public class PlayerCharacterController : MonoBehaviour
{
    private Character _character = null;
    private CharacterMovemevtBehavior _characterMovemevtBehavior;
    private void Awake()
    {
        _character = GetComponent<Character>();
        _characterMovemevtBehavior = GetComponent<CharacterMovemevtBehavior>();
        
        InputManager.Instance.EventShootingWeapon += OnShootingWeapon;
        InputManager.Instance.EventPlayerMovementDirectionChanged += OnPlayerMovementDirectionChanged;
        InputManager.Instance.EventPlayerSprintMode += OnPlayerSprintMode;
        InputManager.Instance.EventPlayerChangeWeapon += OnPlayerChangeWeapon;
        InputManager.Instance.EventPickUpItemButtonPressed += OnPickUpItemBtttonPressed;
        InputManager.Instance.EventThrownigGrenade += OnThrownigGrenade;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var weapons = _character.CharacterInventory.Items.FindAll(i => i.GetType() == typeof(Weapon));
            var currenWeaponIndex = weapons.IndexOf(_character.CurrentWeapon);
            var nextWeapon = weapons[(currenWeaponIndex+1) % weapons.Count];
            nextWeapon.Apply(_character);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            var item = 
                _character.CharacterInventory.Items.Find(i => i != _character.CurrentWeapon);
            if (item != null)
            {
                _character.CharacterInventory.Drop(item);
            }
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            var r = _character.GetComponent<Rigidbody>();
            if (r != null)
            {
                Debug.Log("Key U!!!");
                r.AddForce(new Vector3(0.1f,0.5f,0.1f) * 10f * r.mass, ForceMode.Acceleration);
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            _character.GetComponent<CharacterHealthComponent>()?.ModifyHealth(-10f);
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
        }
    }

    private void OnPlayerChangeWeapon(KeyCode keyCode)
    {
        //доступно ли то или иное оружий!!!

        _character.ChangeWeapon((WeaponType)(keyCode - 48));
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
        
        _character.Shoot();
    }

    private void OnPickUpItemBtttonPressed()
    {
        _character.TryPickUpItem();
    }

    private void OnThrownigGrenade()
    {
        _character.ThrowGrenade();
    }
}
