using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ShelterCheck : MonoBehaviour
{

    [SerializeField]
    private Vector3 ray;
    [SerializeField]
    LayerMask shelterLayer;
    RaycastHit hit;
    [SerializeField]
    float maxDistance;
    private void Start()
    {
        ray = ray.normalized;
    }
    private void Update()
    {
        CheckShelter();
    }
    public void CheckShelter()
    {
        if (Physics.Raycast(transform.position, ray, out hit, maxDistance, shelterLayer))
        {
            PlayerEnt.InStrom = false;
        }
        else
        {
            PlayerEnt.InStrom = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, ray * maxDistance);
    }
}
