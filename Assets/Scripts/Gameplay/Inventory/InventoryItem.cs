using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace PolygonCrazyShooter
{
    public abstract class InventoryItem : MonoBehaviour
    {
        [SerializeField] protected Transform _model = null;
        [SerializeField] private Collider _pickUpTrigger = null;
        [SerializeField] public String Name = "Default";

        public virtual void PickUp()
        {
            _model.gameObject.SetActive(false);
            _pickUpTrigger.gameObject.SetActive(false);
            
            _model.GetComponent<Rigidbody>().isKinematic = true;
        }

        public virtual void Drop()
        {
            _model.gameObject.SetActive(true);
            _pickUpTrigger.gameObject.SetActive(true);
            
            _model.GetComponent<Rigidbody>().isKinematic = false;
        }
        public abstract void Apply(Character character);

        public abstract WeaponType GetWeaponType();
        public abstract float GetDamage();

        public virtual float GetScatter()
        {
            return 0f;
        }

        public virtual void UnApply() { }
    }
}
