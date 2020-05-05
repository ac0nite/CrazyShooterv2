using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;

[RequireComponent(typeof(CharacterPickUpBehavior))]
[RequireComponent(typeof(CharacterHealthComponent))]
[RequireComponent(typeof(CharacterAnimator))]
public class Character : MonoBehaviour
{
    private CharacterHealthComponent _characterHealthComponent = null;
    private CharacterAnimator _characterAnimator = null;
    private CharacterPickUpBehavior _characterPickUpBehavior = null;
    public Transform RightHandBone => _rightHandBone;
    [SerializeField] private Transform _rightHandBone = null;
    [SerializeField] private List<WeaponType> _weaponTypes = null;
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
       
        //var defaultWeapon = GetComponentInChildren<Weapon>();
        var defaultWeapon = Instantiate(SettingsManager.Instance.Weapons[0], transform);

        if (defaultWeapon != null)
        {
            //PickUpWeapon(Instantiate(SettingsManager.Instance.Weapons[0], transform));
            PickUpWeapon(defaultWeapon);
        }
    }

    private void PickUpWeapon(Weapon weapon)
    {
        if(weapon == null)
            return;

        if (CurrentWeapon != null)
        {
            CurrentWeapon.DropWeapon();
        }

        CurrentWeapon = weapon;
        CurrentWeapon.PickUpWeapon(this);
        
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

    public void TryPickUpBehavior()
    {
        var pickedUpWeapon = _characterPickUpBehavior.TryPickUpWeapon();
        if (pickedUpWeapon != null)
        {
            PickUpWeapon(pickedUpWeapon);
        }
    }
}
