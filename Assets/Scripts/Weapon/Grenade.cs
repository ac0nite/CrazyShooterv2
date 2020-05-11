using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public class Grenade : Weapon
{
    public override void Shoot()
    {
        //некая своя логика броска и взрыва
        base.Shoot();
    }

    [SerializeField] private float _timeSecondsBeforeBang = 2f;
    [SerializeField] private float _radiusKilling = 4f;
    public override void ThrowGrenade(Character character)
    {
        CanUse = false;
        base.ThrowGrenade(character);
        ApplyGrenade(character);
    }

    private void ApplyGrenade(Character character)
    {
        _model.gameObject.SetActive(true);
        
        _model.SetParent(character.LefttHandBone);
        _model.localPosition = Vector3.zero;
        _model.localRotation = Quaternion.identity;
        StartCoroutine(Bang(character));
    }

    public void StartFlying()
    {
        Debug.Log("FlyingTest");
        var character = this.GetComponentInParent<Character>();
        if (character != null)
        {
            CanUse = true;
            character.CharacterInventory.Drop(this);
            var rigidbodyGreande = this.GetComponentInChildren<Rigidbody>();
            if (rigidbodyGreande != null)
            {
                Debug.Log("AddForce", this);
                rigidbodyGreande.AddForce(this.transform.position, ForceMode.Impulse);
                //rigidbodyGreande.AddForce(character.transform.position, ForceMode.Impulse);   
            }
        }
    }
    
    // public void EndAnimation()
    // {
    //     Debug.Log("EndAnimation");
    //     CanUse = true;
    // }

    IEnumerator Bang(Character character)
    {
        TimerBang = true;
        yield return new WaitForSeconds(_timeSecondsBeforeBang);
        if (TimerBang)
        {
            Debug.Log($"BANG! {this.gameObject.name}");
            //процедура взрыва
            // var colliders = Physics.OverlapSphere(
            //     this.transform.position,
            //     4f, 
            //     LayerMask.GetMask("Enemies", "Obstacles", "Player"), 
            //     QueryTriggerInteraction.Collide);
            var colliders = Physics.OverlapSphere(this.transform.position, _radiusKilling, LayerMask.GetMask( "Player", "Enemies"));
            foreach (var collider in colliders)
            {
                //collider.gameObject.GetComponentInParent<CharacterHealthComponent>()?.ModifyHealth(-Damage);
                var obj = collider.gameObject.GetComponentInParent<CharacterHealthComponent>();
                if (obj != null)
                {
                    //collider.Raycast, )
                    Debug.Log($"Bang {obj.gameObject.name}");
                    obj.ModifyHealth(-Damage);
                    continue;
                }
                // var obj = collider.gameObject.GetComponentInParent<Character>();
                // if (obj != null)
                // {
                //     obj.HealthComponent.ModifyHealth(-this.Damage);
                //     continue;
                // }
                // obj = collider.gameObject.GetComponentInParent<CharacterHealthComponent>();
                // {
                //     
                // }
            }
            //var ray = new Ray(this.transform.position, );
            Destroy(this.gameObject);   
        }
    }
}
