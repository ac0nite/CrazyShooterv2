using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoverCamera : MonoBehaviour
{
    [SerializeField] private Vector3 _offset = Vector3.zero;
    [SerializeField] private Transform _target;
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
        //float scrol_delta = 1;
        {
            Vector3 cam_v = _target.position - _offset;
            //Debug.Log("magnitude:" + cam_v.magnitude + "  norm:" + cam_v.normalized);


            //Camera.main.orthographicSize += Input.GetAxis("Mouse ScrollWheel");

            //float gamma = scrol_delta / (cam_v.magnitude - scrol_delta);
            //_offset = (_offset + gamma * _target.position) / (1 + gamma);

            // _offset = _offset + (scrol_delta / cam_v.magnitude) * _target.position;

            // Debug.Log("old:" + _offset.magnitude);
            Vector3 changed = _offset + _offset * (-Input.mouseScrollDelta.y * 0.1f);
           // Debug.Log("changed:" + changed.magnitude);
            if (changed.magnitude > _min_distance && changed.magnitude < _max_distance)
            { 
                _offset = changed;
            }
            //_offset += _offset * (- Input.mouseScrollDelta.y * 0.1f);
            //_offset = Vector3.ClampMagnitude(_offset, _max_distance);
            


            //Debug.Log("new:" + _offset.magnitude);

            //Debug.Log("old:" + cam_v.magnitude + "  new:" + _offset.magnitude);

            //Debug.Log("magnitude:" + cam_v.magnitude + "  norm:" + cam_v.normalized);

            //if (cam_v.magnitude > _min_distance && cam_v.magnitude < _max_distance)
            //{
            //    float cur_distance = (_offset + cam_v * Input.mouseScrollDelta.y).magnitude;
               
            //    //Debug.Log("cur_distance:" + cur_distance);
            //}
            //cam_v.Normalize();
            //float cur_distance = (_offset + cam_v * Input.mouseScrollDelta.y).magnitude;
            //if (cur_distance > _min_distance && cur_distance < _max_distance)
            //    _offset += cam_v * Input.mouseScrollDelta.y;
        }

        transform.position = Vector3.Lerp(transform.position, _target.position + _offset, _LerpT * Time.deltaTime);
    }

}
