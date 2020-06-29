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
    [Range(0f, 10f)] [SerializeField] public float ScatterWeapon = 0f;
    [SerializeField] protected ParticleSystem _muzzleFlashPS = null;

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

    public override WeaponType GetWeaponType()
    {
        return _currentWeaponType;
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
    public virtual void Shoot(TypeStateLocomotion stateLocomotion = TypeStateLocomotion.idle, bool IsCameraScreenPoint = true)
    {
        CanUse = false;

        _muzzleFlashPS.Play();
        
        Ray ray_tmp;
        if (IsCameraScreenPoint)
        { 
            ray_tmp = Camera.main.ScreenPointToRay(Input.mousePosition);
            //var direction_tmp = ray_tmp.origin - _shootingPoint.position;
        }
        else
        {
            ray_tmp = new Ray(_shootingPoint.position, transform.forward);
        }
        
        var hit_tmp = Physics.RaycastAll(ray_tmp.origin, ray_tmp.direction, float.MaxValue, LayerMask.GetMask("Default", "Player", "Enemies", "Obstacles"));
        var hit_list_tmp = hit_tmp.ToList();
        hit_list_tmp.Sort((x,y)=>x.distance.CompareTo(y.distance));

        var direct_ray = hit_list_tmp[0].point - _shootingPoint.position;

        var ray = Scatter(new Ray(_shootingPoint.position, direct_ray), stateLocomotion);
        
        Debug.DrawLine(ray.origin, ray.GetPoint(50f), Color.red, 3f);
            
        var rayCastHit = Physics.RaycastAll(ray, float.MaxValue, LayerMask.GetMask("Player", "Enemies", "Obstacles"));
        
        var hitList = rayCastHit.ToList();
        hitList.Sort((x, y) => x.distance.CompareTo(y.distance));

        for (int i = 0; i < hitList.Count; i++)
        {
            // GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // sphere.transform.position = hitList[i].point;
            // sphere.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            // StartCoroutine(RemoveSphere(sphere));

            //GamePlay code
            if (hitList[i].collider.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
            {
                return;
            }

            // if (hitList[i].collider.gameObject.layer == LayerMask.NameToLayer("Enemies"))

            var healthComponent = hitList[i].collider.GetComponentInParent<CharacterHealthComponent>();
            if(healthComponent != null)
            {
                //Debug.Log("Character was hit!");
                healthComponent.ModifyHealth(- GetDamageBody(hitList[i]), _currentWeaponType, hitList[i].distance);
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

                var deltax = UnityEngine.Random.Range(stateLocomotion.DeltaScatterLocomotion(), max_delta + ScatterWeapon/1000f);
                var deltay = UnityEngine.Random.Range(stateLocomotion.DeltaScatterLocomotion(), max_delta + ScatterWeapon/1000f);
                var deltaz = UnityEngine.Random.Range(stateLocomotion.DeltaScatterLocomotion(), max_delta + ScatterWeapon/1000f);
            
                //Debug.Log($"Distance: {hitList[i].distance}  MaxDelta: {max_delta} {deltax} {deltay} {deltaz}");
            
                direct.x += (UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1) * deltax;
                direct.y += (UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1) * deltay;
                direct.z += (UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1) * deltaz;
            
                return new Ray(ray.origin, direct);
            }
        }

        return ray;
    }

    private float GetDamageBody(RaycastHit hit)
    {
        var caps_collider = (CapsuleCollider)hit.collider;
        var body_part = hit.point.y / caps_collider.height;
        
        if (body_part < (3f / 8f)) //ноги
            return Damage * (1f / 3f);
        else if (body_part < (6f / 8f) && body_part > (3f / 8f)) // туловище
            return Damage * (2f / 3f);
        
        return Damage; //голова
    }
    public virtual void ThrowGrenade(Character character)
    {
//        Debug.Log("ThrowGrenade");
    }

    IEnumerator RemoveSphere(GameObject sphere)
    {
        yield return new WaitForSeconds(3f);
        Destroy(sphere.gameObject);
    }


    public override float GetDamage()
    {
        return Damage;
    }

    public override float GetScatter()
    {
        return ScatterWeapon;
    }
}
