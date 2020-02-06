using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertActivate : MonoBehaviour
{

    Guard guard;
    private void Start()
    {
        guard = GetComponentInParent<Guard>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (guard.alerted)
            if (other.tag == "Enemy")
            {
                Guard nearGuard = other.GetComponent<Guard>();
                nearGuard.Alerted();
            }
    }
}
