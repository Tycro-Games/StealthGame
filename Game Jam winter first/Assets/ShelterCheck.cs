using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ShelterCheck : MonoBehaviour
{

    [SerializeField]
    private Vector3 ray;
    [SerializeField]
    private LayerMask shelterLayer = 0;
    RaycastHit hit;
    [SerializeField]
    private float maxDistance = 5;
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
