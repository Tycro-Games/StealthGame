using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private bool ShouldRotate = false;
    Camera cam;
    PlayerController playerController;
    [Header("Shooting settings")]
    public LayerMask CanAtackLayer;
    public float MaxDistance = 100;
    public float fireRate = 1f;
    private float lastFirerate = 0;
    private float TimeBetweenShots;
    public float smoothRotation = .25f;
    private Vector3 Point;
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerController.enabled = false;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, MaxDistance, CanAtackLayer))
            {
                FireRateCheck(hit.point);
            }
        }
        if (ShouldRotate)
        {
            Rotate(Point);
        }
    }
    void FireRateCheck(Vector3 point)
    {
        if (Time.time > lastFirerate)
        {
            Point = point;
            lastFirerate = Time.time + fireRate / 2;
            TimeBetweenShots = lastFirerate - Time.time;
            ShouldRotate = true;//rotate to target
            StartCoroutine(Atack());
        }


    }
    IEnumerator Atack()
    {
        yield return new WaitForSeconds(TimeBetweenShots);
        //shooting and shit
        ShouldRotate = false;
        playerController.enabled = true;
    }
    void Rotate(Vector3 target)
    {
        Vector3 offset = target - transform.position;
        Quaternion desiredRot = Quaternion.LookRotation(offset);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, smoothRotation);
    }
}
