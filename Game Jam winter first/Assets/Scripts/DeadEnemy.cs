using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeadEnemy : MonoBehaviour
{
    [SerializeField]
    private float radiusToBeSeen = 5.0f;
    [SerializeField]
    private UnityEvent OnDead;
    private void Start()
    {
        FindObjectOfType<EnemyManager>().CheckAliveGuards();
        OnDead?.Invoke();
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
