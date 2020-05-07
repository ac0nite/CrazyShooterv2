using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;

public class Medicine : InventoryItem
{
    [SerializeField] private float _health = 50f;

    public override void PickUp()
    {
        base.PickUp();
    }

    public override void Drop()
    {
        if (_model.gameObject.activeSelf)
        {
            _model.SetParent(transform);
        }
        base.Drop();
    }

    public override void Apply(Character character)
    {
        character.HealthComponent.ModifyHealth(-10f);

        if (character.HealthComponent.Health < character.HealthComponent.MaxHealth)
        {
            character.HealthComponent.ModifyHealth(_health);
            character.DestroyMedicineItem(this);
        }

        Debug.Log($"Health {character.HealthComponent.Health}");
    }
}
