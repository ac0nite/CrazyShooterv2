using System;
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
    public event Action EventThrowGrenade;
    private CharacterHealthComponent _characterHealthComponent = null;
    public CharacterAnimator CharacterAnimator = null;
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
        CharacterAnimator = GetComponent<CharacterAnimator>();
        _characterPickUpBehavior = GetComponent<CharacterPickUpBehavior>();
        _characterMovemevtBehavior = GetComponent<CharacterMovemevtBehavior>();

        _characterHealthComponent.EventHealthChange += OnHealthChange;
        _characterHealthComponent.EventCharacterDead += OnCharacterDead;
        CharacterAnimator.EventStartFlyingGrenade += OnStartFlyingGrenade;
        CharacterAnimator.EventEndAnimation += OnEventEndAnimation;
        CharacterAnimator.EventOneShot += OnOneShoot;
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
        
        CharacterAnimator.SetAnimation(CurrentWeapon.Type.GetIdAnimationTriggerName());
        
        CharacterAnimator.LeftHandIKTarget = CurrentWeapon.LeftHadIKTargetPoint;
        CharacterAnimator.RightHandIKTarget = CurrentWeapon.RightHadIKTargetPoint;    
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
        CharacterAnimator.EventStartFlyingGrenade -= OnStartFlyingGrenade;
        CharacterAnimator.EventEndAnimation -= OnEventEndAnimation;
        CharacterAnimator.EventOneShot += OnOneShoot;
//        Debug.Log($"OnDestroy", this);
    }

    protected virtual void Update()
    {
    }

    public void Shoot(bool IsCameraScreenPoint = true)
    {
        if (CurrentWeapon.CanUse)
        {
            CharacterAnimator.SetAnimation("AttackTriggerA");
            if(CurrentWeapon.Type != WeaponType.Heavy) //Heavy стреляет тройным вестрелом, поэтому обрабатывается по событию анимации 
                CurrentWeapon.Shoot(_characterMovemevtBehavior.StateLocomotion, IsCameraScreenPoint);
        }
    }

    private void OnCharacterDead(CharacterHealthComponent healthComponent, float _score)
    {
        CharacterAnimator.Die();
        
        GetComponent<Rigidbody>().isKinematic = true;
        var colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        //var us = GetComponent<PlayerCharacterController>();
        if(GetComponent<PlayerCharacterController>() == null)
            StartCoroutine(DestroyCharacter());
    }

    private void OnHealthChange(CharacterHealthComponent _characterHealth, float _health)
    {
        if (_health != 0f)
        {
            CharacterAnimator.SetAnimation("DamageTrigger");
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
        // var weapons = CharacterInventory.Items
        //     .FindAll(i => (typeof(Weapon) == i.GetType() || typeof(Weapon) == i.GetType().BaseType)) //выделяем только тип Weapon и производные от Weapon
        //     .FindAll(i => ((Weapon) i).Type == weponType);

        var weapons = CharacterInventory.BusyItems
            .FindAll(i => (typeof(Weapon) == i.GetType() || typeof(Weapon) == i.GetType().BaseType)) //выделяем только тип Weapon и производные от Weapon
            .FindAll(i => ((Weapon) i).Type == weponType);
        
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

            // if (pickedUpItem.GetType() != typeof(Grenade))
            // {
            //     pickedUpItem.Apply(this);
            // }
            _characterPickUpBehavior.ShowMessage.Remove(pickedUpItem.GetInstanceID());

            if (pickedUpItem.GetType() == typeof(Grenade))
            {
                ((Grenade) pickedUpItem).TimerBang = false;
            }
        }
    }

    public void ThrowGrenade()
    {
         var grenades = CharacterInventory.Items.FindAll(i => (i.GetType() == typeof(Grenade)));
         
         //var grenades = CharacterInventory.BusyItems.FindAll(i => (i.GetType() == typeof(Grenade) && ((Grenade)i) != null));
         if (grenades.Count > 0)
        {
            var greande = (Grenade) grenades[0];
            if (greande.CanUse)
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                greande.ThrowGrenade(this);
                CharacterAnimator.SetAnimation("AttackGrenadeTrigger");
                CurrentWeapon.CanUse = false;
                EventThrowGrenade?.Invoke();
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

    IEnumerator DestroyCharacter()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
