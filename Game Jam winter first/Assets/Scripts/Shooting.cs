using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    protected Animator anim;
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
    private float lastFirerate = 0;
    [SerializeField]
    private float TimeBetweenShots = 1.3f;
    [SerializeField]
    private float ShootMultiplier = 1.0f;

    protected Vector3 Point;
    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("ShootMultiplier", ShootMultiplier);
    }
    public virtual void Update()
    {
        if (ShouldRotate)
        {
            Rotate(Point, transform);
            Rotate(Point, ShootingPos);
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
        //aiming

        anim.SetTrigger("Aim");

        ShouldRotate = true;
        yield return new WaitForSeconds(TimeBetweenShots * (1 / ShootMultiplier));//animations time
        DrawBullet();
        Resume();
    }
    void DrawBullet()
    {
        Instantiate(bullet, ShootingPos.position, transform.rotation);
    }
    public virtual void Resume()
    {
        ShouldRotate = false;
    }
    public static void Rotate(Vector3 target, Transform ObjectToRotate)
    {
        Vector3 offset = target - ObjectToRotate.position;
        Quaternion desiredRot = Quaternion.LookRotation(offset);

        ObjectToRotate.rotation = Quaternion.RotateTowards(ObjectToRotate.rotation, desiredRot, 15);

    }
}
