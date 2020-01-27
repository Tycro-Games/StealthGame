using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : Shooting
{
    Guard guard;
    // Start is called before the first frame update
    void Start()
    {
        guard = GetComponent<Guard>();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (guard.CanSeePlayer())
        {
            AtackPlayer();
        }
        base.Update();
    }
    void AtackPlayer()
    {
        if (CheckToShoot(guard.player.position))
        {
            guard.enabled = false;
            ShouldRotate = true;
        }
    }
    public override void Resume()
    {
        base.Resume();
        guard.enabled = true;

    }
}
