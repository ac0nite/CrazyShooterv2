using System.Collections;
using System.Collections.Generic;
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
    private bool _pressButton = false;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //Physics.gravity = new Vector3(0, -100, 0);
    }

    // Update is called once per frame
    void Update()
    {

        if(_currentVelocity != Vector3.zero)
            _pressButton = true;

       var _newVelocity = Vector3.zero;

        float speed = _speed;

        //speed *= Time.deltaTime;

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            _pressButton = true;

        if (Input.GetKey(KeyCode.W))
            _newVelocity += Vector3.forward;
        if (Input.GetKey(KeyCode.S))
            _newVelocity += -Vector3.forward;
        if (Input.GetKey(KeyCode.A))
            _newVelocity += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            _newVelocity += Vector3.right;


        _newVelocity = _newVelocity.normalized * _speed;
        _currentVelocity = Vector3.Lerp(_currentVelocity, _newVelocity, _VelocityLerpSpeed * Time.deltaTime);


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
       // if(_pressButton) 
            _rigidbody.velocity = _currentVelocity;

        _rigidbody.rotation = _currentRotation;

        _pressButton = false;
    }
}
