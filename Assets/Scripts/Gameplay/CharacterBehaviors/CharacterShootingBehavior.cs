using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PolygonCrazyShooter
{
    public class CharacterShootingBehavior : MonoBehaviour
    {
        [SerializeField] private Vector3 _shootingPointOffset = Vector3.up;


        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Input.GetMouseButton(0)");
                var ray = new Ray(transform.TransformPoint(_shootingPointOffset), transform.forward);
                //Debug.DrawLine(ray.origin, ray.GetPoint(8f), Color.red);
                var rayCastHit = Physics.RaycastAll(ray, float.MaxValue, LayerMask.GetMask("Enemies", "Obstacles"));


                var hitList = rayCastHit.ToList();
                hitList.Sort((x, y) => x.distance.CompareTo(y.distance));
                
                for (int i = 0; i < hitList.Count; i++)
                {
                    //DEBUG
                    var rayOffset = Vector3.up * 0.1f * i;
                    Debug.DrawLine(ray.origin + rayOffset, hitList[i].point + rayOffset, Color.red, 3f);

                    //GamePlay code
                    if (hitList[i].collider.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
                    {
                        Debug.Log("Obstacle was hit!");
                        return;
                    }

                    if (hitList[i].collider.gameObject.layer == LayerMask.NameToLayer("Enemies"))
                    {
                        Debug.Log("Enemy was hit!");
                    }
                }
;            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawSphere(transform.position + _shootingPointOffset, 0.2f);
            Gizmos.DrawSphere(transform.TransformPoint(_shootingPointOffset), 0.2f);
        }
    }

}
