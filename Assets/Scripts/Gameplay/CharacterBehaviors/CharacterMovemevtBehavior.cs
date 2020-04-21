using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class CharacterMovemevtBehavior : MonoBehaviour
{
    private Rigidbody _rigidbody = null;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _VelocityLerpSpeed = 5.0f;

    private Vector3 _currentVelocity = Vector3.zero;
    private Quaternion _currentRotation = Quaternion.identity;

    private Vector3 _targetMovementVelocity = Vector3.zero;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //Physics.gravity = new Vector3(0, -100, 0);
    }

    private void OnEnable()
    {
        InputManager.Instance.EventPlayerMovementDirectionChanged += OnPlayerMovementDirectionChanged;
    }

    private void OnDisable()
    {
        InputManager.Instance.EventPlayerMovementDirectionChanged -= OnPlayerMovementDirectionChanged;
    }

    private void OnPlayerMovementDirectionChanged(Vector3 targetMovementVector)
    {
        _targetMovementVelocity = targetMovementVector.normalized * _speed;
    }

    private void OnPlayerLookPointChanged(Vector3 lookPointChanged)
    {

    }

    // Update is called once per frame
    void Update()
    {
        _currentVelocity = Vector3.Lerp(_currentVelocity, _targetMovementVelocity, _VelocityLerpSpeed * Time.deltaTime);


        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.up, transform.position);

        var lookVector = transform.forward;

        if(plane.Raycast(ray,out float distance))
        {
            Vector3 lookPoint = ray.GetPoint(distance);
            lookPoint.y = transform.position.y;
            lookVector = (lookPoint - transform.position).normalized;
            //_currentRotation = Quaternion.LookRotation(lookPoint - transform.position, Vector3.up);
            _currentRotation = Quaternion.LookRotation(lookVector);
         }

        //float moveSpeedX =  Mathf.Sign(Vector3.Dot(_currentVelocity, -Vector3.Cross(lookVector, Vector3.up))) * 
        //                    Vector3.Project(_currentVelocity / _speed, -Vector3.Cross(lookVector, Vector3.up)).magnitude;
        //float moveSpeedZ = Mathf.Sign(Vector3.Dot(_currentVelocity / _speed, lookVector)) * Vector3.Project(_currentVelocity, lookVector).magnitude;

        float moveSpeedX = Vector3.Dot(_currentVelocity / _speed, -Vector3.Cross(lookVector, Vector3.up));
        float moveSpeedZ = Vector3.Dot(_currentVelocity / _speed, lookVector);

        if (Input.GetKey(KeyCode.LeftControl))
            moveSpeedZ += 0.1f;

        _animator.SetFloat("MoveSpeedX", moveSpeedX);
        _animator.SetFloat("MoveSpeedZ", moveSpeedZ);
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(_currentVelocity.x, _rigidbody.velocity.y, _currentVelocity.z);
        _rigidbody.rotation = _currentRotation;
    }
}
