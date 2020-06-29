using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;

public class CharacterHealthComponent : MonoBehaviour
{
    public event Action<CharacterHealthComponent, float> EventCharacterDead;
    public event Action<CharacterHealthComponent, float> EventHealthChange;
    [SerializeField] public float MaxHealth = 100f;
    public float Health { get; private set; }
    private void Start()
    {
        Health = MaxHealth;
    }

    public void ModifyHealth(float HealthPoints, WeaponType _type = WeaponType.undefined, float _distance = 0f)
    {
        var newHalth = Mathf.Clamp(HealthPoints + Health, 0f, MaxHealth);
//        Debug.Log($"Obj: {this.gameObject.name}  Health: {newHalth}", this);
        
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (Health == newHalth) 
            return;
        
        Health = newHalth;
        EventHealthChange?.Invoke(this, Health);

        if (Health <= 0f)
        {
            EventCharacterDead?.Invoke(this,_type.GetScore(_distance));
        }
    }
}
