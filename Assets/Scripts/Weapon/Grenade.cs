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
    [SerializeField] private Rigidbody _rigidbody = null;
    [SerializeField] private float _powerThrow = 15f;
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
        Debug.Log("Create StartCoroutine");
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
            Debug.Log("AddForce", this);
           // _shootingPoint.TransformPoint()
           
           var r2 = gameObject.GetComponentInChildren<Rigidbody>();
           r2.AddForce(_shootingPoint.transform.forward * _powerThrow, ForceMode.Impulse);

            //_rigidbody.AddForce(_shootingPoint.transform.forward * _powerThrow, ForceMode.Impulse);

            //rigidbodyGreande.AddForce(character.transform.position, ForceMode.Impulse);   
        }
    }
    
    // public void EndAnimation()
    // {
    //     Debug.Log("EndAnimation");
    //     CanUse = true;
    // }

    IEnumerator Bang(Character character)
    {
        Debug.Log("Start StartCoroutine");

        TimerBang = true;
        yield return new WaitForSeconds(_timeSecondsBeforeBang);

        //var cur_position = transform.TransformPoint(_model.transform.position);
        //transform.position = Vector3.zero;
        // transform.position = cur_position;

        //var gl = _model.transform.TransformPoint(Vector3.zero);


        Debug.Log($"curOld {_model.transform.position}");
        var gl = this.transform.TransformDirection(_model.transform.position);
       Debug.Log($"gl {gl}");
       Debug.Log($"cur {_model.transform.position}");

        // _model.transform.position = Vector3.zero;

        Debug.Log("Process StartCoroutine");
        if (TimerBang)
        {
            Debug.Log($"BANG! {this.gameObject.name}");
            //процедура взрыва
            // var colliders = Physics.OverlapSphere(
            //     this.transform.position,
            //     4f, 
            //     LayerMask.GetMask("Enemies", "Obstacles", "Player"), 
            //     QueryTriggerInteraction.Collide);

            var m = transform.TransformPoint(_model.transform.position);

            var colliders = Physics.OverlapSphere(_model.transform.position, _radiusKilling, LayerMask.GetMask( "Player", "Enemies"));
            foreach (var collider in colliders)
            {
                
                //collider.gameObject.GetComponentInParent<CharacterHealthComponent>()?.ModifyHealth(-Damage);
                var obj_character = collider.GetComponentInParent<CharacterHealthComponent>();
                if(obj_character == null)
                    continue;

                //var direction  = (obj_character.transform.position - _model.transform.position);
                var direction = (collider.transform.position - _model.transform.position);
                Debug.Log($"Collider:{collider.transform.position}  model: {_model.transform.position}");
                Ray ray = new Ray(_model.transform.position, direction);

                Debug.DrawRay(ray.origin, ray.GetPoint(50f), Color.green, 10f);

                float proportion_damage = 0f;
                RaycastHit hit;
                if (collider.Raycast(ray, out hit, _radiusKilling))
                {
                    proportion_damage = (_radiusKilling - hit.distance) / _radiusKilling;
                    Debug.Log($"Raycast {collider.gameObject.name} {hit.distance} {proportion_damage}", collider);
                }

                //var rigidbody = obj_character.GetComponent<Rigidbody>();
                //if (rigidbody != null)
                //{
                //    rigidbody.AddForce(direction.normalized * 50f * rigidbody.mass /** proportion_damage*/, ForceMode.Impulse);
                //    obj_character.GetComponent<CharacterAnimator>()?.SetAnimation("DamageTriggerGrenade");
                //}
                obj_character.ModifyHealth(-(Damage * proportion_damage));
            }

            //Destroy(this.gameObject);
        }
    }
}
