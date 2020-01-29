using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : Shooting
{
    public LayerMask CanAtackLayer;
    public delegate IEnumerator OnAlert(Vector3 pos);
    public static event OnAlert onAlert;
    Camera cam;
    PlayerController playerController;
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        cam = Camera.main;
    }
    public override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            playerController.enabled = false;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, MaxDistance, CanAtackLayer))
            {
                if (!PlayerEnt.InStrom && onAlert != null)
                {
                    StartCoroutine(onAlert(transform.position));
                }
                Shoot(hit.point);
            }
        }

    }
    public override void Resume()
    {
        base.Resume();
        playerController.enabled = true;

    }
    void Shoot(Vector3 point)
    {
        if (CheckToShoot(point))
        {
            ShouldRotate = true;//rotate to target
        }
    }
}