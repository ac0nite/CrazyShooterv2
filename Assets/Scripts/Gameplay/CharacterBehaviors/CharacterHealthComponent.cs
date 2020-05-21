using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthComponent : MonoBehaviour
{
    public event Action<CharacterHealthComponent> EventCharacterDead;
    public event Action<CharacterHealthComponent, float> EventHealthChange;
    [SerializeField] public float MaxHealth = 100f;
    public float Health { get; private set; }
    private void Start()
    {
        Health = MaxHealth;
    }

    public void ModifyHealth(float HealthPoints)
    {
        var newHalth = Mathf.Clamp(HealthPoints + Health, 0f, MaxHealth);
        Debug.Log($"Obj: {this.gameObject.name}  Health: {newHalth}", this);
        
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (Health == newHalth) 
            return;
        
        Health = newHalth;
        EventHealthChange?.Invoke(this, Health);

        if (Health <= 0f)
        {
            EventCharacterDead?.Invoke(this);
        }
    }
}
