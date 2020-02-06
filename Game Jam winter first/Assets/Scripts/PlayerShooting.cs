using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : Shooting
{
    [SerializeField]
    Transform AimElement;
    [SerializeField]
    private LayerMask CanAtackLayer = 0;
    public delegate void OnAlert(Vector3 pos);
    public static event OnAlert onAlert;
    Camera cam;
    PlayerController playerController;
    public static bool shooting = false;
    private Ray ray;
    private RaycastHit hit;
    [Header("AimAssist")]
    [SerializeField]
    LineRenderer aimAsist = null;
    [SerializeField]
    private LayerMask aimAsistLayerFromCam = 0;
    [SerializeField]
    private LayerMask AimAsistLayer = 0;
    [SerializeField]
    float maxAimDist = 10f;

    [SerializeField]
    private Gradient AimOnShootGradient;
    private Gradient previousGradient;
    public override void Start()
    {
        base.Start();
        playerController = GetComponent<PlayerController>();
        cam = Camera.main;
        aimAsist.SetPosition(0, ShootingPos.position);
        Cursor.lockState = CursorLockMode.Confined;
        previousGradient = aimAsist.colorGradient;
    }
    private void OnEnable()
    {
        PlayerEnt.onDead += DeactivateAim;
    }
    private void OnDisable()
    {
        PlayerEnt.onDead -= DeactivateAim;
    }
    void DeactivateAim()
    {
        aimAsist.enabled = false;
    }
    public override void Update()
    {

        base.Update();
        ray = cam.ScreenPointToRay(Input.mousePosition);
        LineCast();
        if (Input.GetMouseButtonDown(0) && !shooting)
        {



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

                    aimAsist.colorGradient = AimOnShootGradient;
                }
            }
        }

    }
    void LineCast()
    {
        if (Physics.Raycast(ray, out hit, MaxDistance, aimAsistLayerFromCam))
        {
            aimAsist.SetPosition(0, ShootingPos.position);

            Vector3 aimPos = hit.point;
            aimPos.y = ShootingPos.position.y;
            Vector3 dir = aimPos - ShootingPos.position;

            RaycastHit HitAim;
            if (Physics.SphereCast(aimAsist.GetPosition(0), aimAsist.widthCurve.Evaluate(0), dir.normalized, out HitAim, maxAimDist + dir.magnitude, AimAsistLayer))
            {
                aimPos = HitAim.point;
                AimElement.gameObject.SetActive(false);
            }
            else AimElement.gameObject.SetActive(true);


            aimAsist.SetPosition(1, aimPos);
            AimElement.position = new Vector3(aimAsist.GetPosition(1).x, AimElement.position.y, aimAsist.GetPosition(1).z);

        }
    }

    public override void Resume()
    {

        if (shooting && PlayerEnt.Dead != true)
        {
            base.Resume();

            playerController.ResumeDestination();
        }
        shooting = false;
        aimAsist.colorGradient = previousGradient;
    }


}