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
    public override void Start()
    {
        base.Start();
        playerController = GetComponent<PlayerController>();
        cam = Camera.main;

    }
    public override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0) && playerController.enabled)
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
                    playerController.enabled = false;
                }
            }
        }

    }
    public override void Resume()
    {
        if (PlayerEnt.Imobile != true)
        {
            base.Resume();
            playerController.enabled = true;
            playerController.ResumeDestination();
        }

    }

}