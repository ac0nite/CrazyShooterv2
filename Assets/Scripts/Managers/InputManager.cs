using System;
using PolygonCrazyShooter;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    //public event Action<Vector3> EventLookPointChanged; 
    public Action<Vector3> EventPlayerMovementDirectionChanged;
    public Action<Vector3> EventPlayerLookPointChanged;

    private Vector3 _targetMovementVelocity = Vector3.zero;
    public void Awake()
    {
        Instance = this;
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


        //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //var plane = new Plane(Vector3.up, transform.position);

        //if (plane.Raycast(ray, out float distance))
        //{
        //    EventPlayerLookPointChanged?.Invoke(ray.GetPoint(distance));
        //}
    }
}   