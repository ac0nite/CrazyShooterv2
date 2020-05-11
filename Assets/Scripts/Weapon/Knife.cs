using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Knife : Weapon
{
    public override void Shoot()
    {
        //некая своя логика работы с ножём
        //base.Shoot();
    }

    
    public void OnPullingTriggerEnter(Collider other)
    {
        //Debug.Log("!!!!!! OnTriggerEnter !!!!!!!!", other);
        AttackKnife(other);
    }

    private void AttackKnife(Collider other)
    {
        var enemy = other.GetComponentInParent<CharacterHealthComponent>();
        if (enemy != null)
        {
            enemy.ModifyHealth(-Damage);
        }
    }
}
