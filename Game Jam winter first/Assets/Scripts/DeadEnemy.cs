using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEnemy : MonoBehaviour
{

    [SerializeField]
    private float radiusToBeSeen = 5.0f;
    private void Start()
    {
        gameObject.layer = 11;
        SphereCollider sc = gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
        sc.isTrigger = true;
        sc.radius = radiusToBeSeen;
    }
    private void OnDrawGizmos()
    {
        if (this.enabled)
            Gizmos.DrawWireSphere(transform.position, radiusToBeSeen);
    }
}
