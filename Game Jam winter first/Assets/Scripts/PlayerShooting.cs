using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : Shooting
{
    public LayerMask CanAtackLayer;
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