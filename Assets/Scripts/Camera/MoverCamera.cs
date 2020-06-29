using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public class MoverCamera : MonoBehaviour
{
    [SerializeField] private Transform _target = null;
    [SerializeField] private float _LerpT = 3;
    private Camera _mainCamera = null;
    [SerializeField] private Transform _scope = null;
    [SerializeField] private List<Transform> _pointScope = new List<Transform>();
    private Ray _ray;
    RaycastHit [] hits = new RaycastHit[4];
    private Vector3 _defaultPositionCamera = Vector3.zero;
    private float _defaultDistanceCamera = 0f;
    private void Awake()
    {
        _mainCamera = Camera.main;
        _defaultPositionCamera = _mainCamera.transform.position;
        _defaultDistanceCamera = (_target.position - _mainCamera.transform.position).magnitude;
        Debug.Log($"Distance max: {_defaultDistanceCamera}");
    }

    void Update()
    {
        _scope.position = _target.position;
        transform.position = Vector3.Lerp(transform.position, _target.position, _LerpT * Time.deltaTime);
        
        // Debug.Log($"Distance: {(_target.position - _mainCamera.transform.position).magnitude}   Default: {_defaultDistanceCamera}");
        //
        // if (CheckScope())
        // {
        //     var directionCamera = _target.position - _mainCamera.transform.position;
        //     var distance = _mainCamera.transform.position + directionCamera.normalized;
        //     if (distance.magnitude > 1f)
        //     {
        //         //_mainCamera.transform.position = distance;
        //         _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, distance, 0.5f * Time.deltaTime);
        //     }
        // }
        // else if((_target.position - _mainCamera.transform.position).magnitude < _defaultDistanceCamera)
        // {
        //     var directionCamera = _mainCamera.transform.position - _target.position;
        //     var distance = _mainCamera.transform.position + directionCamera.normalized;
        //     
        //     if (distance.magnitude < _defaultDistanceCamera)
        //     {
        //         //_mainCamera.transform.position = distance;
        //         _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, distance, 0.5f * Time.deltaTime);
        //     }
        // }

        if (Quaternion.Angle(transform.rotation, _target.rotation) > 10f)
        {
            var rot  = Quaternion.LookRotation(_target.forward, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 1f);
            //_target.rotation = transform.rotation;
        }
    }

    private bool CheckScope()
    {
        Array.Clear(hits, 0, hits.Length);
        for (int i = 0; i < _pointScope.Count; i++)
        {
            _ray = new Ray(_mainCamera.transform.position, _pointScope[i].position - _mainCamera.transform.position);
            
            Debug.DrawLine(_mainCamera.transform.position, _pointScope[i].position, Color.red, 0.1f);
            
            hits = Physics.RaycastAll(_ray, (_pointScope[i].position - _mainCamera.transform.position).magnitude,  LayerMask.GetMask("Obstacles"));
            if (hits.Length > 0f)
            {
                return true;
            }
        }

        return false;
    }
}
