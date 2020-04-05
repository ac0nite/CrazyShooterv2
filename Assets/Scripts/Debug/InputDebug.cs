using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDebug : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private float _boost = 1.3f;
    [SerializeField] private float _slowdown_horisontal = 0.4f;
    [SerializeField] private float _slowdown_vertical = 0.6f;

    // Update is called once per frame
    void Update()
    {
        float speed = _speed;

        if (Input.GetKey(KeyCode.LeftControl))
            speed *= _boost;

        speed *= Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
            transform.Translate(0, 0, speed);
        else if (Input.GetKey(KeyCode.S))
            transform.Translate(0, 0, -speed* _slowdown_vertical);
        else if (Input.GetKey(KeyCode.A))
            transform.Translate(-speed * _slowdown_horisontal, 0, 0);
        else if (Input.GetKey(KeyCode.D))
            transform.Translate(speed * _slowdown_horisontal, 0, 0);

        //float deltaX = Input.GetAxis("Horizontal") * _speed * Time.deltaTime;
        //float deltaZ = Input.GetAxis("Vertical") * _speed * Time.deltaTime;
        //transform.Translate(deltaX, 0, deltaZ);
    }
}
