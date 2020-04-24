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
    private Vector3 _lookVector = Vector3.zero;
    private bool _sprint = false;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //Physics.gravity = new Vector3(0, -100, 0);
    }

    private void OnEnable()
    {
        InputManager.Instance.EventPlayerMovementDirectionChanged += OnPlayerMovementDirectionChanged;
        InputManager.Instance.EventPlayerLookPointChanged += OnPlayerLookPointChanged;
        InputManager.Instance.EventPlayerSprintMode += OnPlayerSprintMode;
    }

    private void OnDisable()
    {
        if (InputManager.TryInstance != null)
        {
            InputManager.Instance.EventPlayerMovementDirectionChanged -= OnPlayerMovementDirectionChanged;
            InputManager.Instance.EventPlayerLookPointChanged -= OnPlayerLookPointChanged;
            InputManager.Instance.EventPlayerSprintMode -= OnPlayerSprintMode;
        }
    }

    private void OnPlayerMovementDirectionChanged(Vector3 targetMovementVector)
    {
        _targetMovementVelocity = targetMovementVector.normalized * _speed;
    }

    private void OnPlayerLookPointChanged(Vector3 lookPoint)
    {
        //Debug.Log($"OnPlayerLookPointChanged");
        lookPoint.y = transform.position.y;
        _lookVector = (lookPoint - transform.position).normalized;
        _currentRotation = Quaternion.LookRotation(_lookVector);
    }

    private void OnPlayerSprintMode(bool sprint_mode)
    {
        _sprint = sprint_mode;
        Debug.Log($"SPRINT: {sprint_mode}");
    }
    // Update is called once per frame
    void Update()
    {
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
        _rigidbody.velocity = new Vector3(_currentVelocity.x, _rigidbody.velocity.y, _currentVelocity.z);
        _rigidbody.rotation = _currentRotation;
    }
}
