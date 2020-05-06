using UnityEngine;

// ReSharper disable once CheckNamespace
namespace PolygonCrazyShooter
{
    public abstract class InventoryItem : MonoBehaviour
    {
        [SerializeField] protected Transform _model = null;
        [SerializeField] private Collider _pickUpTrigger = null;

        public virtual void PickUp()
        {
            _model.gameObject.SetActive(false);
            _pickUpTrigger.gameObject.SetActive(false);
        }

        public virtual void Drop()
        {
            _model.gameObject.SetActive(true);
            _pickUpTrigger.gameObject.SetActive(true);
        }
        public abstract void Apply(Character character);
        public abstract void UnApply();
    }
}
