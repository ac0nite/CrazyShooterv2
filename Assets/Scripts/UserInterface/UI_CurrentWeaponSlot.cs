using System;
using PolygonCrazyShooter;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CurrentWeaponSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool _visiblePrecision = true;
    [SerializeField] private GameObject _objectPrecision = null;
    [SerializeField] private Slider _sliderPrecision = null;

    [SerializeField] private bool _visibleDamage = true;
    [SerializeField] private GameObject _objectDamage = null;
    [SerializeField] private Slider _sliderDamage = null;

    [SerializeField] private GameObject _objectActiveSlot = null;
    public bool _activeSlot = true;

    [SerializeField] private GameObject _objectSelect = null;

    [SerializeField] private GameObject _WeaponImage = null;

    [SerializeField] public WeaponType TypeWeaponSlot = WeaponType.undefined;
    [SerializeField] private Text NameWeaponType = null;

    public event Action<UI_InventoryItemElement> EventAssigned;
    public event Action<UI_InventoryItemElement /*new*/, UI_InventoryItemElement/*old*/> EventWeaponAssigned;
    public event Action<float> EventAddHelthing;
    public event Action<UI_InventoryItemElement> EventRemoveItem;

    private UI_InventoryItemElement _uiCurrentInventoryItemElement = null;

    private void FixedUpdate()
    {
        //        Debug.Log($" _objectActiveSlot.activeSelf: {_objectActiveSlot.activeSelf}");
        //_objectActiveSlot.SetActive(false);
        //Debug.Log($"b. _activeSlot: {_activeSlot}  _objectActiveSlot: {_objectActiveSlot.activeSelf}");
    }

    public void AssignElementSlot(UI_InventoryItemElement _uiInventoryItemElement)
    {
        Debug.Log($"add - {_uiInventoryItemElement.CurrentWeapon.GetWeaponType().ToString()}");
        _objectActiveSlot.SetActive(false);

        _uiCurrentInventoryItemElement = _uiInventoryItemElement;
        _uiInventoryItemElement.transform.SetParent(_WeaponImage.transform);
        _uiInventoryItemElement.transform.localPosition = Vector3.zero;

        //Debug.Log($"Damage: {_uiInventoryItemElement.CurrentWeapon.GetDamage()}");
        //Debug.Log($"Scatter: {_uiInventoryItemElement.CurrentWeapon.GetScatter()}");

        if(_visibleDamage)
            _sliderDamage.value = _uiInventoryItemElement.CurrentWeapon.GetDamage() / 100f;
        
        if (_visiblePrecision)
            _sliderPrecision.value = 1f - _uiInventoryItemElement.CurrentWeapon.GetScatter() / 10f;
        
        EventAssigned?.Invoke(_uiInventoryItemElement);
    }

    public void AddHealth(UI_InventoryItemElement inventoryItemElement)
    {
        var med = (Medicine) inventoryItemElement.CurrentWeapon;
        var remainder = Healthing(med.Healthing);
        if (remainder == 0)
        {
            EventRemoveItem?.Invoke(inventoryItemElement);
        }
        else
        {
            ((Medicine) inventoryItemElement.CurrentWeapon).Healthing = remainder;
        }
    }

    public float Healthing(float _health)
    {
        float delta = ((_sliderDamage.value + _health / 100f) - 1f) * 100f;
        _sliderDamage.value = Mathf.Clamp(_sliderDamage.value + _health / 100f, 0f, 1f);
        EventAddHelthing?.Invoke(_health);
        return (delta > 0) ? delta : 0f;
    }

    public void AddGrenade(UI_InventoryItemElement inventoryItemElement)
    {
        var grenade = (Grenade) inventoryItemElement.CurrentWeapon;
        if(_sliderPrecision.value == _sliderPrecision.maxValue)
            return;

        //if (_sliderPrecision.value == _sliderPrecision.minValue) 
        // if (_sliderPrecision.value < _sliderPrecision.maxValue)
        // {
            _uiCurrentInventoryItemElement = inventoryItemElement;
            inventoryItemElement.transform.SetParent(_WeaponImage.transform);
            inventoryItemElement.transform.localPosition = Vector3.zero;
        // }
        // else
        // {
        //     EventRemoveItem?.Invoke(inventoryItemElement);
        // }
        
        _sliderPrecision.value += 1f;
        _sliderDamage.value = grenade.GetDamage();
        
        EventAssigned?.Invoke(inventoryItemElement);
    }

    public void RemGrenade()
    {
        _sliderPrecision.value -= 1f;
        if (_sliderPrecision.value == _sliderPrecision.minValue)
        {
            _sliderDamage.value = 0f;
        }
    }

    public void SetHealth(float _health)
    {
        Debug.Log("SetHealth");
        _sliderDamage.value = _health / 100f;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("public void OnDrop(PointerEventData eventData)");
        var inventoryItemElement = eventData.pointerDrag.gameObject.GetComponentInParent<UI_InventoryItemElement>();
        
        if (inventoryItemElement.CurrentWeapon.GetWeaponType() != TypeWeaponSlot)
            return;
        
        if (inventoryItemElement != null)
        {
            var typeInventory = inventoryItemElement.CurrentWeapon.GetType();
            if (typeInventory == typeof(Medicine))
            {
                AddHealth(inventoryItemElement);
                return;
            }
            else if (typeInventory == typeof(Grenade))
            {
                AddGrenade(inventoryItemElement);
                return;
            }
            
            if (typeInventory != typeof(Weapon) && typeInventory != typeof(Knife))
                return;

            if (inventoryItemElement.CurrentWeapon != null)
                EventWeaponAssigned?.Invoke(inventoryItemElement, _uiCurrentInventoryItemElement);

            AssignElementSlot(inventoryItemElement);
        }
    }

    private void Awake()
    {
        Debug.Log("Awake", gameObject);
        _objectDamage.SetActive(_visibleDamage);
        _objectPrecision.SetActive(_visiblePrecision);
        _objectSelect.SetActive(false);
        NameWeaponType.text = TypeWeaponSlot.ToString();
        //_activeSlot = true;
    }

    public bool BusySlot()
    {
        return _activeSlot;
    }
}
