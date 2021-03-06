﻿using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterPickUpBehavior))]
[RequireComponent(typeof(CharacterHealthComponent))]
[RequireComponent(typeof(CharacterAnimator))]
//[RequireComponent(typeof(Medicine))]
public class Character : MonoBehaviour
{
    private CharacterHealthComponent _characterHealthComponent = null;
    private CharacterAnimator _characterAnimator = null;
    private CharacterPickUpBehavior _characterPickUpBehavior = null;
    private CharacterMovemevtBehavior _characterMovemevtBehavior = null;

    public Transform RightHandBone => _rightHandBone;
    public Transform LefttHandBone => _leftHandBone;
    public Inventory CharacterInventory => _inventoryComponent;
    public CharacterHealthComponent HealthComponent => _characterHealthComponent;
    public CharacterMovemevtBehavior CharacterMovemevt => _characterMovemevtBehavior;

    [SerializeField] private Transform _rightHandBone = null;
    [SerializeField] private Transform _leftHandBone = null;
    //[SerializeField] private List<WeaponType> _weaponTypes = null;
    [SerializeField] private Inventory _inventoryComponent = null;

    //[SerializeField] private Slider _playerHealthProgressBarSlider = null;

    public Weapon CurrentWeapon { get; set; }

    //private readonly List<Weapon> CurrentWeapons = new List<Weapon>(4);
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
        _characterMovemevtBehavior = GetComponent<CharacterMovemevtBehavior>();

        _characterHealthComponent.EventHealthChange += OnHealthChange;
        _characterHealthComponent.EventCharacterDead += OnCharacterDead;
        _characterAnimator.EventStartFlyingGrenade += OnStartFlyingGrenade;
        _characterAnimator.EventEndAnimation += OnEventEndAnimation;
        _characterAnimator.EventOneShot += OnOneShoot;
    }

    private void Start()
    {
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

        //if (_playerHealthProgressBarSlider != null)
        //{
        //    _playerHealthProgressBarSlider.value = 1f;
        //    var sliderCanvas = _playerHealthProgressBarSlider.gameObject.GetComponentInParent<Canvas>();
        //    //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    sliderCanvas.worldCamera = Camera.main;
        //}
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

    public void DestroyMedicineItem(Medicine medicineItem)
    {
        Debug.Log($"Destroy Item medicine");

        _inventoryComponent.Items.Remove(medicineItem);
        Destroy(medicineItem.gameObject);
    }

    private void OnDestroy()
    {
        _characterHealthComponent.EventCharacterDead -= OnCharacterDead;
        _characterHealthComponent.EventHealthChange -= OnHealthChange;
        _characterAnimator.EventStartFlyingGrenade -= OnStartFlyingGrenade;
        _characterAnimator.EventEndAnimation -= OnEventEndAnimation;
        _characterAnimator.EventOneShot += OnOneShoot;
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
        if (CurrentWeapon.CanUse)
        {
            _characterAnimator.SetAnimation("AttackTriggerA");
            if(CurrentWeapon.Type != WeaponType.Heavy) //Heavy стреляет тройным вестрелом, поэтому обрабатывается по событию анимации 
                CurrentWeapon.Shoot(_characterMovemevtBehavior.StateLocomotion);
            
        }
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

    private void OnHealthChange(CharacterHealthComponent _characterHealth, float _health)
    {
        if (_health != 0f)
        {
            _characterAnimator.SetAnimation("DamageTrigger");
        }

        //if (_playerHealthProgressBarSlider != null)
        //{
        //    _playerHealthProgressBarSlider.value = _health / _characterHealth.MaxHealth;
        //    if(_health == 0f)
        //        _playerHealthProgressBarSlider.gameObject.SetActive(false);
        //}
    }

    public void ChangeWeapon(WeaponType weponType)
    {
        var weapons = CharacterInventory.Items
            .FindAll(i => (typeof(Weapon) == i.GetType() || typeof(Weapon) == i.GetType().BaseType)) //выделяем только тип Weapon и производные от Weapon
            .FindAll(i => ((Weapon) i).Type == weponType);

        //var weapons2 = CharacterInventory.Items.FindAll(i =>
        //{
        //    if (typeof(Weapon) == i.GetType() || typeof(Weapon) == i.GetType().BaseType)
        //    {
        //        return ((Weapon) i).Type == weponType;
        //    }
        //    return false;
        //});

        if (weapons.Count == 1)
        {
            weapons[0].Apply(this);
        }
        else if (weapons.Count > 1)
        {
            var indexCurentWeapon = weapons.IndexOf(CurrentWeapon);
            var nextweapon = weapons[(indexCurentWeapon + 1) % weapons.Count];
            nextweapon.Apply(this);
        }
    }

    public void TryPickUpItem()
    {
        var pickedUpItem = _characterPickUpBehavior.TryPickUpItem();
        
        // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
        if (pickedUpItem != null)
        {
            _inventoryComponent.PickUp(pickedUpItem);

            if (pickedUpItem.GetType() != typeof(Grenade))
            {
                pickedUpItem.Apply(this);
            }

            if (pickedUpItem.GetType() == typeof(Grenade))
            {
                ((Grenade) pickedUpItem).TimerBang = false;
            }
        }
    }

    public void ThrowGrenade()
    {
        var grenades = gameObject.GetComponentsInChildren<Grenade>();
        if (grenades.Length > 0)
        {
            if (grenades[0].CanUse)
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                grenades[0].ThrowGrenade(this);
                _characterAnimator.SetAnimation("AttackGrenadeTrigger");
                CurrentWeapon.CanUse = false;
            }
        }
    }

    private void OnStartFlyingGrenade()
    {
        //Debug.Log("OnStartFlyingGrenade");
        var grenade = CharacterInventory.Items.Find(i => i.GetType() == typeof(Grenade));
        CharacterInventory.Drop(grenade);
        ((Grenade)grenade).StartFlying();
    }

    private void OnEventEndAnimation()
    {
        CurrentWeapon.CanUse = true;
    }

    private void OnOneShoot()
    {
        CurrentWeapon.Shoot(_characterMovemevtBehavior.StateLocomotion);
    }
}
