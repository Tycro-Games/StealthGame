using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        StartCoroutine(CheckShelter());
    }

    public IEnumerator CheckShelter()
    {

        while (!PlayerEnt.Imobile)
        {
            if (Physics.Raycast(transform.position, ray, out hit, maxDistance, shelterLayer))
            {
                PlayerEnt.InStrom = false;
            }
            else
            {

                PlayerEnt.InStrom = true;
            }


            yield return null;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, ray * maxDistance);
    }
}
