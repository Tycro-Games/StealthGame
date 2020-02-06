using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : Shooting
{
    public LayerMask CanAtackLayer;
    public delegate void OnAlert(Vector3 pos);
    public static event OnAlert onAlert;
    Camera cam;
    PlayerController playerController;
    public static bool shooting = false;
    public override void Start()
    {
        base.Start();
        playerController = GetComponent<PlayerController>();
        cam = Camera.main;

    }
    public override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {


            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, MaxDistance, CanAtackLayer))
            {
                if (!PlayerEnt.InStrom && onAlert != null)
                {
                    onAlert(transform.position);
                }
                if (CheckToShoot(hit.point))
                {
                    playerController.StopDestination();
                    shooting = true;

                }
            }
        }

    }
    public override void Resume()
    {

        if (shooting)
        {
            base.Resume();

            playerController.ResumeDestination();
        }
        shooting = false;

    }


}