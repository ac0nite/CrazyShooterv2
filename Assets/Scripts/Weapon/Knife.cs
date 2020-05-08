using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    public override void Shoot()
    {
        //некая своя логика работы с ножём
        //base.Shoot();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("!!!!!! OnTriggerEnter !!!!!!!!", other);
    }
}
