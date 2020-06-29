using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterPickUpBehavior : MonoBehaviour
{
    //public Action<Weapon> EventWeaponPickUp;
    private List<InventoryItem> _overlappedItems  = new List<InventoryItem>();
    [SerializeField] public ShowMessage ShowMessage = null;

    public InventoryItem TryPickUpItem()
    {
        //Debug.Log($"TryPickUpWeapon.Count= {_overlappedItems.Count} ", this);
        if (_overlappedItems.Count > 0)
        {
            var item = _overlappedItems[0];
            _overlappedItems.Remove(item);
            return item;
        } 

        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
//        Debug.Log($"OnTriggerEnter {this.gameObject.name} - {other.gameObject.name}", other);
        var item = other.GetComponentInParent<InventoryItem>();
        if (item != null)
        {
          // Debug.Log($"OnTriggerEnter ", other);
             _overlappedItems.Add(item);
            //ShowMessage UI hint
//            Debug.Log($"on ID: {item.GetInstanceID()}  {item.Name}");
            ShowMessage?.Send(item.Name, GetDetails(item), item.GetInstanceID());
        }
    }

    private void OnTriggerExit(Collider other)
    {
//        Debug.Log($"OnTriggerExit {other.name}", other);
        var item = other.GetComponentInParent<InventoryItem>();
        if (item != null)
        {
            _overlappedItems.Remove(item);
            
            ShowMessage?.Remove(item.GetInstanceID());
            //Debug.Log($"off ID: {item.GetInstanceID()}  {item.Name}");
            
            if (_overlappedItems.Count == 0)
            {
                //hide UI hint
                // ShowMessage?.Remove(item.GetInstanceID());
                // Debug.Log($"off ID: {item.GetInstanceID()}  {item.Name}");
            }
        }
    }

    private String GetDetails(InventoryItem item)
    {
       // var t = item.GetType();
        if (item.GetType() == typeof(Medicine))
        {
            return "Пополнить здоровье на " + ((Medicine) item).Healthing.ToString();
        }
        else if (item.GetType() == typeof(Grenade))
        {
            return "Мощность взрыва: " + ((Grenade) item).GetDamage().ToString() + ". Радиус поражения: " + ((Grenade) item).RadiusKilling.ToString();
        }
        else if (item.GetType() == typeof(Weapon))
        {
            return "Уровень поражения: " + ((Weapon) item).GetDamage() + ". Разброс: " + ((Weapon) item).ScatterWeapon.ToString();
        }

        return "";
    }
}
