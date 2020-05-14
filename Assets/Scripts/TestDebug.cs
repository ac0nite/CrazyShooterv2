using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDebug : MonoBehaviour
{
    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log($"OnTriggerEnter {this.gameObject.name} - {other.gameObject.name}", other);
    //}

    [SerializeField] private Rigidbody r = null;

    public void Awake()
    {
       // r = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var force = new Vector3(0.1f, 0.1f, 0.1f);
            r.AddForce(force * 10f, ForceMode.Impulse);


            var tr = transform.TransformPoint(r.position);

            var tr2 = r.transform.TransformPoint(Vector3.zero);

            transform.position = tr2;
            Debug.Log($"{r.position} - {tr2}");
        }
    }
}
