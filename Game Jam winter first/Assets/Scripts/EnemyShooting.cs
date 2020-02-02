using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : Shooting
{
    Guard guard;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        guard = GetComponent<Guard>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

    }
    private void OnEnable()
    {
        Guard.OnGuardHasSpottedPlayer += AtackPlayer;
    }
    private void OnDisable()
    {
        Guard.OnGuardHasSpottedPlayer -= AtackPlayer;
    }
    void AtackPlayer()
    {
        CheckToShoot(guard.player.position);


    }


}
