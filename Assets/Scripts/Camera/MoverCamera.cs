using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoverCamera : MonoBehaviour
{
    [SerializeField] private Vector3 _offset = Vector3.zero;
    [SerializeField] private Transform _target = null;
    [SerializeField] private float _LerpT = 3;
    private float _min_distance = 4;
    [SerializeField] private float _max_distance = 20;

    void Start()
    {
        //_offset = transform.position;\
        _min_distance = _offset.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 cam_v = _target.position - _offset;

        Vector3 changed = _offset + _offset * (-Input.mouseScrollDelta.y * 0.1f);
        
        if (changed.magnitude > _min_distance && changed.magnitude < _max_distance)
        { 
            _offset = changed;
        }
        
        transform.position = Vector3.Lerp(transform.position, _target.position + _offset, _LerpT * Time.deltaTime);

        
        if (Quaternion.Angle(transform.rotation, _target.rotation) > 25f)
        {
            var rot  = Quaternion.LookRotation(_target.forward, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 0.8f);
            //_target.rotation = rot;
        }
    }

}
