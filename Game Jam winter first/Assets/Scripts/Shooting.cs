using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    protected bool ShouldRotate = false;
    [Header("Shooting settings")]
    [SerializeField]
    private GameObject bullet = null;
    [SerializeField]
    private Transform ShootingPos = null;
    [SerializeField]
    protected float MaxDistance = 100;
    [SerializeField]
    private float fireRate = 1f;
    [SerializeField]
    private float lastFirerate = 0;
    [SerializeField]
    private float TimeBetweenShots = 0;

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
        Instantiate(bullet, ShootingPos.position, transform.rotation);


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
