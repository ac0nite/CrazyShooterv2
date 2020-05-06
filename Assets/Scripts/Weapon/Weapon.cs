using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PolygonCrazyShooter;
using UnityEngine;

public class Weapon : InventoryItem
{
    //[SerializeField] private Transform _weaponModel = null;
    [SerializeField] private Transform _shootingPoint = null;
    [SerializeField] private WeaponType _currentWeaponType = WeaponType.undefined;
    
    [SerializeField] public Transform LeftHadIKTargetPoint = null;
    [SerializeField] public Transform RightHadIKTargetPoint = null;

//    [SerializeField] private Collider _pickUpTrigger = null;
    
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
        
    }
    public override void Apply(Character character)
    {
        _model.gameObject.SetActive(true);

       _model.SetParent(character.RightHandBone);
       _model.localPosition = Vector3.zero;
       _model.localRotation = Quaternion.identity;


    }
    public override void UnApply()
    {
        _model.gameObject.SetActive(false);

        _model.SetParent(transform);
    }

    public override void Drop()
    {
        DetachModel();
    }

    public void PickUpWeapon(Character character)
    {
//        transform.SetParent(character.transform);
//        transform.localPosition = Vector3.zero;
//        transform.localRotation = Quaternion.identity;

        AttachModel(character.RightHandBone);
    }

    public void DropWeapon()
    {
        //transform.SetParent(null);
    }
    private void AttachModel(Transform rightHandBone)
    {
        _weaponModel.SetParent(rightHandBone);
        _weaponModel.localPosition = Vector3.zero;
        _weaponModel.localRotation = Quaternion.identity;

        _weaponModel.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void DetachModel()
    {
        //transform.SetParent(null);
        _weaponModel.SetParent(transform);
        _weaponModel.GetComponent<Rigidbody>().isKinematic = false;
    }
    
    public virtual void Shoot()
    {
        //Debug.Log($"_shootingPoint.position: {_shootingPoint.position}");
        var ray = new Ray(_shootingPoint.position, transform.forward); //_shootingPoint.forward
        Debug.DrawLine(ray.origin, ray.GetPoint(50f), Color.red, 3f);
            
        var rayCastHit = Physics.RaycastAll(ray, float.MaxValue, LayerMask.GetMask("Enemies", "Obstacles"));


        var hitList = rayCastHit.ToList();
        hitList.Sort((x, y) => x.distance.CompareTo(y.distance));

        for (int i = 0; i < hitList.Count; i++)
        {
            // //DEBUG
            // var rayOffset = Vector3.up * 0.1f * i;
            // Debug.DrawLine(ray.origin + rayOffset, hitList[i].point + rayOffset, Color.red, 3f);

            //GamePlay code
            if (hitList[i].collider.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
            {
                //Debug.Log("Obstacle was hit!");
                return;
            }

            // if (hitList[i].collider.gameObject.layer == LayerMask.NameToLayer("Enemies"))
              
            var healthComponent = hitList[i].collider.GetComponentInParent<CharacterHealthComponent>();
            if(healthComponent != null)
            {
                //Debug.Log("Character was hit!");
                healthComponent.ModifyHealth(-50f);
            }
        }
    }
}
