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
        if (CheckToShoot(guard.player.position))
        {
            ShouldRotate = true;
        }
    }
    public override IEnumerator Atack()
    {
        yield return new WaitForSeconds(TimeBetweenShots);//animations time
    }
    public override void Resume()
    {
        base.Resume();
        guard.enabled = true;

    }
}
