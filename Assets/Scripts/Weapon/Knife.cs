using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;

public class Knife : Weapon
{
    public override void Shoot(TypeStateLocomotion stateLocomotion = TypeStateLocomotion.idle, bool IsCameraScreenPoint = true)
    {
        //base.Shoot(stateLocomotion);
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
            enemy.ModifyHealth(-Damage, WeaponType.Knife);
        }
    }

    public override float GetScatter()
    {
        //return base.GetScatter();
        return 0f;
    }
}
