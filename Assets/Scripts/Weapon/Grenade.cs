using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;

public class Grenade : Weapon
{
    public override void Shoot()
    {
        //некая своя логика броска и взрыва
        base.Shoot();
    }

    public override void ThrowGrenade(Character character)
    {
        if (CanThrowGrenade)
        {
            base.ThrowGrenade(character);
            
            CanThrowGrenade = false;
            ApplyGrenade(character);
        }
    }

    private void ApplyGrenade(Character character)
    {
        var grenade = character.CharacterInventory.Items.Find(i => i.GetType() == typeof(Grenade));

        _model.gameObject.SetActive(true);
        
        _model.SetParent(character.LefttHandBone);
        _model.localPosition = Vector3.zero;
        _model.localRotation = Quaternion.identity;
    }
}
