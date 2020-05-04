
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PolygonCrazyShooter;
using UnityEngine;

public class CharacterShootingBehavior : MonoBehaviour
{
    [SerializeField] private Vector3 _shootingPointOffset = Vector3.up;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void OnShootingBehahior()
    {

    }


    private void Update()
    {
        // if(Input.GetMouseButtonDown(0))
        // {
        //     //Debug.Log("Input.GetMouseButton(0)");
        //     var ray = new Ray(transform.TransformPoint(_shootingPointOffset), transform.forward);
        //     Debug.DrawLine(ray.origin, ray.GetPoint(50f), Color.red, 3f);
        //     
        //     var rayCastHit = Physics.RaycastAll(ray, float.MaxValue, LayerMask.GetMask("Enemies", "Obstacles"));
        //
        //
        //     var hitList = rayCastHit.ToList();
        //     hitList.Sort((x, y) => x.distance.CompareTo(y.distance));
        //
        //     for (int i = 0; i < hitList.Count; i++)
        //     {
        //         // //DEBUG
        //         // var rayOffset = Vector3.up * 0.1f * i;
        //         // Debug.DrawLine(ray.origin + rayOffset, hitList[i].point + rayOffset, Color.red, 3f);
        //
        //         //GamePlay code
        //         if (hitList[i].collider.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        //         {
        //             //Debug.Log("Obstacle was hit!");
        //             return;
        //         }
        //
        //        // if (hitList[i].collider.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        //       
        //        var healthComponent = hitList[i].collider.GetComponentInParent<CharacterHealthComponent>();
        //        if(healthComponent != null)
        //         {
        //             //Debug.Log("Character was hit!");
        //             healthComponent.ModifyHealth(-50f);
        //         }
        //     }
        // }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(transform.position + _shootingPointOffset, 0.2f);
        Gizmos.DrawSphere(transform.TransformPoint(_shootingPointOffset), 0.2f);
        Debug.Log($"transform.TransformPoint(_shootingPointOffset): {transform.TransformPoint(_shootingPointOffset)}");
    }
}

namespace PolygonCrazyShooter
{
}
