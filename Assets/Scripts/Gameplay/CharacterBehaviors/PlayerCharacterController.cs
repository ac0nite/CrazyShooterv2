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
    private int d = 20;
    private void Awake()
    {
        _character = GetComponent<Character>();
        _characterMovemevtBehavior = GetComponent<CharacterMovemevtBehavior>();
        
        InputManager.Instance.EventShootingWeapon += OnShootingWeapon;
        InputManager.Instance.EventPlayerMovementDirectionChanged += OnPlayerMovementDirectionChanged;
        InputManager.Instance.EventPlayerSprintMode += OnPlayerSprintMode;
        InputManager.Instance.EventPlayerChangeWeapon += OnPlayerChangeWeapon;
    }
    private void OnDestroy()
    {
        if (InputManager.TryInstance != null)
        {
            InputManager.Instance.EventShootingWeapon -= OnShootingWeapon;
            InputManager.Instance.EventPlayerMovementDirectionChanged -= OnPlayerMovementDirectionChanged;
            InputManager.Instance.EventPlayerSprintMode -= OnPlayerSprintMode;
            InputManager.Instance.EventPlayerChangeWeapon -= OnPlayerChangeWeapon;
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
}
