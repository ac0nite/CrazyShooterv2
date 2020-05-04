using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;
[RequireComponent(typeof(CharacterHealthComponent))]
[RequireComponent(typeof(CharacterAnimator))]
public class Character : MonoBehaviour
{
    private CharacterHealthComponent _characterHealthComponent = null;
    private CharacterAnimator _characterAnimator = null;
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
        
        _characterHealthComponent.EventCharacterDead += OnCharacterDead;
        
        //var defaultWeapon = GetComponentInChildren<Weapon>();
        var defaultWeapon = Instantiate(SettingsManager.Instance.Weapons[0], transform);

        if (defaultWeapon != null)
        {
            //PickUpWeapon(Instantiate(SettingsManager.Instance.Weapons[0], transform));
            PickUpWeapon(defaultWeapon);
        }
    }

    public void PickUpWeapon(Weapon weapon)
    {
        if(CurrentWeapon != null)
            CurrentWeapon.DetachModel();

        CurrentWeapon = weapon;
        CurrentWeapon.AttachModel(_rightHandBone);
        
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
        if (Input.GetKey(KeyCode.F) && CurrentWeapon != null)
        {
            CurrentWeapon.DetachModel();
        }
        // if(Input.GetKeyDown(KeyCode.Alpha1))
        //     _characterAnimator.SetAnimation("HandGun_Trigger");
        // if(Input.GetKeyDown(KeyCode.Alpha2))
        //     _characterAnimator.SetAnimation("Heavy_Trigger");
        // if(Input.GetKeyDown(KeyCode.Alpha3))
        //     _characterAnimator.SetAnimation("Infantry_Trigger");
        // if(Input.GetKeyDown(KeyCode.Alpha4))
        //     _characterAnimator.SetAnimation("Knife_Trigger");

        // if (Input.GetKeyDown(KeyCode.N))
        // {
        //     int index = UnityEngine.Random.Range(0, 4);
        //     Debug.Log($"Press key, index {index}");
        //     string[] triggers =
        //         {"HandGun_Trigger", "Heavy_Trigger", "Infantry_Trigger", "Knife_Trigger"};
        //     _characterAnimator.SetAnimation(triggers[UnityEngine.Random.Range(0, 4)]);
        // }
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
}
