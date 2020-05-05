using System;   
using PolygonCrazyShooter;
using UnityEngine;

public class InputManager : SingletoneGameObject<InputManager>
{
    //public static InputManager Instance;

    //public event Action<Vector3> EventLookPointChanged; 
    public Action<Vector3> EventPlayerMovementDirectionChanged;
    public Action<Vector3> EventPlayerLookPointChanged;
    public Action<bool> EventPlayerSprintMode;
    public Action<bool> EventShootingWeapon;
    public Action<KeyCode> EventPlayerChangeWeapon;
    public Action EventPickUpItemButtonPressed;


    private Vector3 _targetMovementVelocity = Vector3.zero;
    protected  override void Awake()
    {
        base.Awake();

    }

    private void Update()
    {
        Vector3 _newVelocity = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            _newVelocity += Vector3.forward;
        if (Input.GetKey(KeyCode.S))
            _newVelocity += -Vector3.forward;
        if (Input.GetKey(KeyCode.A))
            _newVelocity += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            _newVelocity += Vector3.right;
        _newVelocity = _newVelocity.normalized;

        if (_targetMovementVelocity != _newVelocity)
        {
             _targetMovementVelocity = _newVelocity;
            EventPlayerMovementDirectionChanged?.Invoke(_targetMovementVelocity);
        }

        ///
        /// PickUp
        ///
        if (Input.GetKeyDown(KeyCode.F))
        {
            EventPickUpItemButtonPressed?.Invoke();
        }

        ///
        /// sprint mode
        /// 
        if (Input.GetKeyDown(KeyCode.LeftShift))
            EventPlayerSprintMode?.Invoke(true);
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            EventPlayerSprintMode?.Invoke(false);

        ///
        /// to change weapon
        ///
        if(Input.GetKeyDown(KeyCode.Alpha1))
            EventPlayerChangeWeapon?.Invoke(KeyCode.Alpha1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            EventPlayerChangeWeapon?.Invoke(KeyCode.Alpha2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            EventPlayerChangeWeapon?.Invoke(KeyCode.Alpha3);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            EventPlayerChangeWeapon?.Invoke(KeyCode.Alpha4);

        ///
        /// shooting
        ///
        if (Input.GetMouseButtonDown(0))
            EventShootingWeapon?.Invoke(true);
        else if(Input.GetMouseButtonUp(0))
            EventShootingWeapon?.Invoke(false);

        //if (Input.GetKey(KeyCode.LeftShift))
        //    EventPlayerSprintMode?.Invoke(Input.GetKeyDown(KeyCode.LeftShift));

        ///
        /// look point
        /// 
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.up, transform.position);

        if (plane.Raycast(ray, out float distance))
        {
            EventPlayerLookPointChanged?.Invoke(ray.GetPoint(distance));
        }
    }
}   