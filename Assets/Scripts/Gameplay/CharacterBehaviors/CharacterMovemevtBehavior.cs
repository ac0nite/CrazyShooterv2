using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using PolygonCrazyShooter;
using TMPro;
using UnityEditor;
using UnityEngine;


public enum TypeStateLocomotion
{
    idle = 0,
    walk = 1,
    run = 2
}

public static class WeaponTypeExtensions
{
    public static float DeltaScatterLocomotion(this TypeStateLocomotion _this)
    {
        if (_this == TypeStateLocomotion.walk)
            return 2f / 100f;
        if(_this == TypeStateLocomotion.run)
            return 5f / 100f;

        return 0f;
    }
}

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovemevtBehavior : MonoBehaviour
{
    private Rigidbody _rigidbody = null;
    [SerializeField] protected float _speed = 5.0f;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _VelocityLerpSpeed = 5.0f;

    private Vector3 _currentVelocity = Vector3.zero;
    private Quaternion _currentRotation = Quaternion.identity;

    protected Vector3 _targetMovementVelocity = Vector3.zero;
    private Vector3 _targetMovementInputVector = Vector3.zero;
    private Vector3 _lookVector = Vector3.zero;
    private bool _sprint = false;
    public TypeStateLocomotion StateLocomotion = TypeStateLocomotion.idle;

    public virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public virtual void ChangePlayerMovementDirectionChanged(Vector3 targetMovementVector)
    {
        _targetMovementInputVector = targetMovementVector;
    }

    protected void NewTargetMovement()
    {
        _targetMovementVelocity = Vector3.zero;
        float speed = _speed;
        
        
        if(Vector3.Dot(_targetMovementInputVector, (Vector3.forward + Vector3.right).normalized) > 0f)
        {

            _targetMovementVelocity += _lookVector;
            _targetMovementVelocity += -Vector3.Cross(_lookVector, Vector3.up);
        }
        if(Vector3.Dot(_targetMovementInputVector, (Vector3.forward + Vector3.left).normalized) > 0f)
        {
            _targetMovementVelocity += _lookVector;
            _targetMovementVelocity += Vector3.Cross(_lookVector, Vector3.up);
        }
        if(Vector3.Dot(_targetMovementInputVector, (Vector3.back + Vector3.right).normalized) > 0f)
        {
            _targetMovementVelocity += -_lookVector;
            _targetMovementVelocity += -Vector3.Cross(_lookVector, Vector3.up);
            speed *= 0.8f;
        }
        if(Vector3.Dot(_targetMovementInputVector, (Vector3.back + Vector3.left).normalized) > 0f)
        {
            _targetMovementVelocity += -_lookVector;
            _targetMovementVelocity += Vector3.Cross(_lookVector, Vector3.up);
            speed *= 0.8f;
        }
        _targetMovementVelocity = _targetMovementVelocity.normalized * speed;
    }

    public virtual void OnPlayerLookPointChanged(Vector3 lookPoint)
    {
        lookPoint.y = transform.position.y;
        _lookVector = (lookPoint - transform.position).normalized;
        if(_lookVector != Vector3.zero)
            _currentRotation = Quaternion.LookRotation(_lookVector);
        
        // lookPoint.y = transform.position.y;
        // _lookVector = (lookPoint - transform.position).normalized;
        // //_currentRotation = Quaternion.LookRotation(_lookVector);
        // var rotation = Quaternion.LookRotation(_lookVector);
        // _currentRotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10f);
    }
    public void ChangePlayerSprintMode(bool sprint_mode)
    {
        _sprint = sprint_mode;
    }
    public virtual void Update()
    {
        //NewTargetMovement();

        _currentVelocity = Vector3.Lerp(_currentVelocity, _targetMovementVelocity, _VelocityLerpSpeed * Time.deltaTime);
        

        float moveSpeedX = Vector3.Dot(_currentVelocity / _speed, -Vector3.Cross(_lookVector, Vector3.up));
        float moveSpeedZ = Vector3.Dot(_currentVelocity / _speed, _lookVector);
        
        if (_sprint)
            moveSpeedZ += 0.1f;

        _animator.SetFloat("MoveSpeedX", moveSpeedX);
        _animator.SetFloat("MoveSpeedZ", moveSpeedZ);

        _lookVector = transform.forward;
    }

    private void FixedUpdate()
    {
        if(_currentVelocity.magnitude < 0.1f)
            StateLocomotion = TypeStateLocomotion.idle;
        else if (_currentVelocity.magnitude > 0f && _currentVelocity.magnitude <= 0.5f)
            StateLocomotion = TypeStateLocomotion.walk;
        else
            StateLocomotion = TypeStateLocomotion.run;

        _rigidbody.velocity = new Vector3(_currentVelocity.x, _rigidbody.velocity.y, _currentVelocity.z);
        _rigidbody.rotation = _currentRotation;
    }

    public void SetSpeed((float, float) getSpeed)
    {
        _speed = getSpeed.Item1;
        _VelocityLerpSpeed = getSpeed.Item2;
    }
}
