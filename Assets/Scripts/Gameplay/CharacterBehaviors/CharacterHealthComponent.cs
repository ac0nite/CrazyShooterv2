using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthComponent : MonoBehaviour
{
    public event Action<CharacterHealthComponent> EventCharacterDead;
    public event Action<CharacterHealthComponent, float> EventHealthChange; 
    [SerializeField] private float _maxHealth = 100f;
    public float Health { get; private set; }
    private void Start()
    {
        Health = _maxHealth;
    }

    // private void OnCharacterDead(CharacterHealthComponent healthComponent)
    // {
    //     
    // }
    public void ModifyHealth(float HealthPoints)
    {
        var newHalth = Mathf.Clamp(HealthPoints + Health, 0f, _maxHealth);
        Debug.Log($"Health: {newHalth}");
        
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
