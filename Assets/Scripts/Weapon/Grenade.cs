using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Serialization;

public class Grenade : Weapon
{
    [SerializeField] private float _timeSecondsBeforeBang = 2f;
    [SerializeField] public float RadiusKilling = 4f;
    [SerializeField] private Rigidbody _rigidbody = null;
    [SerializeField] private float _powerThrow = 15f;
    public bool TimerBang = false;

    public override void ThrowGrenade(Character character)
    {
        CanUse = false;
        base.ThrowGrenade(character);
        ApplyGrenade(character);
    }


    private void Start()
    {
        //StartCoroutine(Bang(null));
    }
    private void ApplyGrenade(Character character)
    {
        _model.gameObject.SetActive(true);
        
        _model.SetParent(character.LefttHandBone);
        _model.localPosition = Vector3.zero;
        _model.localRotation = Quaternion.identity;
//        Debug.Log("Create StartCoroutine");
        StartCoroutine(Bang(character));
    }
    
    public void StartFlying()
    {
        CanUse = true;
        Debug.Log("AddForce", this);
       // Time.timeScale = 0.1f;
        _rigidbody.AddForce(_shootingPoint.transform.forward * _powerThrow * _rigidbody.mass, ForceMode.Impulse);
        //Time.timeScale = 0.2f;
    }

    IEnumerator Bang(Character character)
    {
        TimerBang = true;
        yield return new WaitForSeconds(_timeSecondsBeforeBang);
        
        if (TimerBang)
        {
            //transform.position = _model.transform.TransformPoint(Vector3.zero);
            
            _muzzleFlashPS.Play();

            Debug.Log($"BANG! {this.gameObject.name}");

            //TODO добавить ещё маску Obstacles что бы откидывать предметы, только с массой нужно что-то решить
            var colliders = Physics.OverlapSphere(_model.transform.position, RadiusKilling, LayerMask.GetMask( "Player", "Enemies"));
            foreach (var collider in colliders)
            {
                Debug.Log($"colliders: {colliders.Length}");
                var obj_character = collider.GetComponentInParent<CharacterHealthComponent>();
                if(obj_character == null)
                    continue;

                var direction = collider.transform.position - _model.transform.position;
                Ray ray = new Ray(_model.transform.position, direction);
                

                float proportion_damage = 0f;
                RaycastHit hit;
                if (collider.Raycast(ray, out hit, RadiusKilling))
                {
                    proportion_damage = (RadiusKilling - hit.distance) / RadiusKilling;

                    Debug.DrawLine(_model.transform.position, collider.transform.position, Color.red, 10f);
                    Debug.DrawLine(ray.origin, ray.GetPoint(hit.distance), Color.red, 10f);

                    Debug.Log($"Raycast {collider.gameObject.name} HitDistance:{hit.distance} PDamage: {proportion_damage}", collider);
                }

                var rigidbody = obj_character.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    Debug.Log("AddForce");
                    rigidbody.AddForce(direction * 4f * rigidbody.mass * proportion_damage, ForceMode.Impulse);
                    //yield return new WaitForSeconds(2f);
                    //obj_character.GetComponent<CharacterAnimator>()?.SetAnimation("DamageTriggerGrenade");
                }
                obj_character.ModifyHealth(-(Damage * proportion_damage), WeaponType.Grenade);
            }
            //Destroy(this.gameObject);
            transform.position = _model.transform.TransformPoint(Vector3.zero);
            _model.gameObject.SetActive(false);
            StartCoroutine(DestroyObjectGreande());
        }
    }

    IEnumerator DestroyObjectGreande()
    {
        yield return new WaitForSeconds(_muzzleFlashPS.duration);
        Destroy(this.gameObject);
    }

    public override float GetScatter()
    {
        //return base.GetScatter();
        return 0f;
    }
}
