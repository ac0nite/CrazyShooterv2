using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PolygonCrazyShooter;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class Weapon : InventoryItem
{
    [SerializeField] protected Transform _shootingPoint = null;
    [SerializeField] private WeaponType _currentWeaponType = WeaponType.undefined;
    
    [SerializeField] public Transform LeftHadIKTargetPoint = null;
    [SerializeField] public Transform RightHadIKTargetPoint = null;

    [SerializeField] protected float Damage = 50f;
    [SerializeField] private bool _scatterForWeapon = true;
    [Range(0f, 10f)] [SerializeField] private float _scatterWeapon = 0f;

    public bool CanUse = true;

    public WeaponType Type
    {
        get
        {
            if(_currentWeaponType == WeaponType.undefined)
                new Exception("Type weapon - Undefined!");
            return _currentWeaponType;
        }
        set { _currentWeaponType = value; }
    }

    public override void PickUp()
    {    
        base.PickUp();
    }
    public override void Apply(Character character)
    {
        if (character.CurrentWeapon != null)
        {
            if (gameObject.GetInstanceID() == character.CurrentWeapon.gameObject.GetInstanceID())
                return;
        }

        _model.gameObject.SetActive(true);

       _model.SetParent(character.RightHandBone);
       _model.localPosition = Vector3.zero;
       _model.localRotation = Quaternion.identity;
       
       character.ApplyWeapon(this);
       
       character.GetComponent<CharacterMovemevtBehavior>()?.SetSpeed(character.CurrentWeapon.Type.GetSpeed());
    }
    public override void UnApply()
    {
        _model.gameObject.SetActive(false);
        
        _model.SetParent(transform);
    }

    public override void Drop()
    {
        if (_model.gameObject.activeSelf)
        {
            _model.SetParent(transform);   
        }
        base.Drop();
    }
    public virtual void Shoot(TypeStateLocomotion stateLocomotion = TypeStateLocomotion.idle)
    {
        //var type_locomotion = GetComponentInParent<CharacterMovemevtBehavior>()?.StateLocomotion;
        //Debug.Log($"StateLocomotion: {type_locomotion.ToString()}");
        
        CanUse = false;
        var ray_tmp = Camera.main.ScreenPointToRay(Input.mousePosition);
            var direction_tmp = ray_tmp.origin - _shootingPoint.position;
        var hit_tmp = Physics.RaycastAll(ray_tmp.origin, ray_tmp.direction, float.MaxValue, LayerMask.GetMask("Default", "Enemies", "Obstacles"));
        //Debug.DrawRay(ray_tmp.origin, ray_tmp.GetPoint(100f), Color.black, 10f);
        var hit_list_tmp = hit_tmp.ToList();
        hit_list_tmp.Sort((x,y)=>x.distance.CompareTo(y.distance));
        //Debug.DrawLine(_shootingPoint.position, hit_list_tmp[hit_list_tmp.Count-1].point, Color.black, 10f);

        var direct_ray = hit_list_tmp[0].point - _shootingPoint.position;
        
        //var ray = new Ray(_shootingPoint.position, direct_ray); //DEBUG

        var ray = Scatter(new Ray(_shootingPoint.position, direct_ray), stateLocomotion);
        
        //var ray = new Ray(_shootingPoint.position, transform.forward); //_shootingPoint.forward
        //Debug.DrawLine(ray.origin, ray.GetPoint(50f), Color.red, 3f);
            
        var rayCastHit = Physics.RaycastAll(ray, float.MaxValue, LayerMask.GetMask("Enemies", "Obstacles"));


        var hitList = rayCastHit.ToList();
        hitList.Sort((x, y) => x.distance.CompareTo(y.distance));

        for (int i = 0; i < hitList.Count; i++)
        {
            // //DEBUG
            // var rayOffset = Vector3.up * 0.1f * i;
            // Debug.DrawLine(ray.origin + rayOffset, hitList[i].point + rayOffset, Color.red, 3f);

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = hitList[i].point;
            sphere.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);

            //GamePlay code
            if (hitList[i].collider.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
            {
                return;
            }

            // if (hitList[i].collider.gameObject.layer == LayerMask.NameToLayer("Enemies"))
              
            Debug.Log($"Distance = {hitList[i].distance}");
            var healthComponent = hitList[i].collider.GetComponentInParent<CharacterHealthComponent>();
            if(healthComponent != null)
            {
                //Debug.Log("Character was hit!");
                healthComponent.ModifyHealth(-Damage);
            }
        }
    }

    private Ray Scatter(Ray ray, TypeStateLocomotion stateLocomotion = TypeStateLocomotion.idle)
    {
        if (_scatterForWeapon)
        {
            var rayCastHit = Physics.RaycastAll(ray, float.MaxValue, LayerMask.GetMask("Enemies", "Obstacles"));
            var hitList = rayCastHit.ToList();
            hitList.Sort((x, y) => x.distance.CompareTo(y.distance));
            for (int i = 0; i < hitList.Count; i++)
            {
                var max_delta = hitList[i].distance / 10000f;
            
                var direct = ray.direction;

                //Debug.Log($"{stateLocomotion.DeltaStreapLocomotion()}");

                var deltax = UnityEngine.Random.Range(stateLocomotion.DeltaScatterLocomotion(), max_delta + _scatterWeapon/1000f);
                var deltay = UnityEngine.Random.Range(stateLocomotion.DeltaScatterLocomotion(), max_delta + _scatterWeapon/1000f);
                var deltaz = UnityEngine.Random.Range(stateLocomotion.DeltaScatterLocomotion(), max_delta + _scatterWeapon/1000f);
            
                //Debug.Log($"Distance: {hitList[i].distance}  MaxDelta: {max_delta} {deltax} {deltay} {deltaz}");
            
                direct.x += (UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1) * deltax;
                direct.y += (UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1) * deltay;
                direct.z += (UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1) * deltaz;
            
                return new Ray(ray.origin, direct);
            }
        }

        return ray;
    }
    public virtual void ThrowGrenade(Character character)
    {
//        Debug.Log("ThrowGrenade");
    }
}
