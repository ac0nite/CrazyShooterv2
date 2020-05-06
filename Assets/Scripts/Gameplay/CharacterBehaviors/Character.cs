using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterPickUpBehavior))]
[RequireComponent(typeof(CharacterHealthComponent))]
[RequireComponent(typeof(CharacterAnimator))]
public class Character : MonoBehaviour
{
    private CharacterHealthComponent _characterHealthComponent = null;
    private CharacterAnimator _characterAnimator = null;
    private CharacterPickUpBehavior _characterPickUpBehavior = null;
    
    public Transform RightHandBone => _rightHandBone;
    public Inventory CharacterInventory => _inventoryComponent;
    [SerializeField] private Transform _rightHandBone = null;
    [SerializeField] private List<WeaponType> _weaponTypes = null;
    [SerializeField] private Inventory _inventoryComponent = null;
    public Weapon CurrentWeapon { get; set; }
    public bool IsDead
    {
        get
        {
            if (_characterHealthComponent != null)
            {
                if (_characterHealthComponent.Health <= 0f)
                    return true;
            }
            return false;
        }
    }
    private void Awake()
    {
        _characterHealthComponent = GetComponent<CharacterHealthComponent>();
        _characterAnimator = GetComponent<CharacterAnimator>();
        _characterPickUpBehavior = GetComponent<CharacterPickUpBehavior>();

        _characterHealthComponent.EventCharacterDead += OnCharacterDead;
       
        var defaultWeapon = GetComponentInChildren<Weapon>();
        //var defaultWeapon = Instantiate(SettingsManager.Instance.Weapons[0], transform);

        if (defaultWeapon != null)
        {
            //PickUpWeapon(Instantiate(SettingsManager.Instance.Weapons[0], transform));
            //PickUpWeapon(defaultWeapon);
            _inventoryComponent.PickUp(defaultWeapon);
            defaultWeapon.Apply(this);
          //  ApplyWeapon(defaultWeapon);
        }
    }

    public void ApplyWeapon(Weapon weapon)
    {
        if (CurrentWeapon != null)
        {
            //_inventory.Drop(weapon);
           // CurrentWeapon.Drop();
           CurrentWeapon.UnApply();
        }

        CurrentWeapon = weapon;
        //CurrentWeapon.Apply(this);
        
        _characterAnimator.SetAnimation(CurrentWeapon.Type.GetIdAnimationTriggerName());
        
        _characterAnimator.LeftHandIKTarget = CurrentWeapon.LeftHadIKTargetPoint;
        _characterAnimator.RightHandIKTarget = CurrentWeapon.RightHadIKTargetPoint;    
    }    

    private void OnDestroy()
    {
        _characterHealthComponent.EventCharacterDead -= OnCharacterDead;
    }

    protected virtual void Update()
    {
//        if (Input.GetKey(KeyCode.F) && CurrentWeapon != null)
//        {
//            CurrentWeapon.DetachModel();
//        }
    }

    public void Shoot()
    {
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        CurrentWeapon.Shoot();
        _characterAnimator.SetAnimation("AttackTrigger");
    }

    private void OnCharacterDead(CharacterHealthComponent healthComponent)
    {
        _characterAnimator.Die();
        
        GetComponent<Rigidbody>().isKinematic = true;
        var colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    public void ChangeWeapon(WeaponType weponType)
    {
        _characterAnimator.SetAnimation(weponType.GetIdAnimationTriggerName());
    }

    public void TryPickUpItem()
    {
        var pickedUpItem = _characterPickUpBehavior.TryPickUpItem();
        
        // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
        if (pickedUpItem != null)
        {
            _inventoryComponent.PickUp(pickedUpItem);
        }
    }
}
