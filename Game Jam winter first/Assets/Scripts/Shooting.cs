using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    protected bool ShouldRotate = false;
    [Header("Shooting settings")]

    public float MaxDistance = 100;
    public float fireRate = 1f;
    private float lastFirerate = 0;
    public float TimeBetweenShots;
    public float smoothRotation = 15f;
    protected Vector3 Point;
    public virtual void Update()
    {
        if (ShouldRotate)
        {
            Rotate(Point, transform);
        }
    }
    public bool CheckToShoot(Vector3 point)
    {
        if (Time.time > lastFirerate)
        {
            Point = point;
            lastFirerate = Time.time + fireRate / 2;

            StartCoroutine(Atack());
            return true;//can shoot
        }
        return false;

    }
    private IEnumerator Atack()
    {
        ShouldRotate = true;
        yield return new WaitForSeconds(TimeBetweenShots);//animations time

        //shooting and shit

        Resume();
    }
    public virtual void Resume()
    {
        ShouldRotate = false;
    }
    public static void Rotate(Vector3 target, Transform transform)
    {
        Vector3 offset = target - transform.position;
        Quaternion desiredRot = Quaternion.LookRotation(offset);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, 15);
    }
}
