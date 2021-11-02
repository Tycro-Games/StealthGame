using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private List<Guard> guards = new List<Guard>();

    private Transform Player;

    [SerializeField]
    private UnityEvent onNoEnemies;
    public void RemoveGuard(Guard g)
    {
        guards.Remove(g);
    }

    private void Awake()
    {
        FindGuards();
    }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        PlayerShooting.onAlert += Alert;
        PlayerEnt.onDead += PlayerDead;
    }

    private void OnDisable()
    {
        PlayerShooting.onAlert -= Alert;
        PlayerEnt.onDead -= PlayerDead;
    }

    public void CheckAliveGuards()
    {
        foreach (Guard g in guards)
        {
            if (g.TryGetComponent(out DeadEnemy en))
                if (en.enabled == false)
                    return;
        }

        onNoEnemies?.Invoke();
       
    }

    public void FindGuards()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Guard guardToAdd = transform.GetChild(i).GetComponentInChildren<Guard>();
            if (guardToAdd.enabled)
                guards.Add(guardToAdd);
        }
    }

    private void Alert(Vector3 pos)
    {

        if (guards.Count == 0 )
            return;
        Guard closestEnemy = ClosestEnemy();
        closestEnemy.StartCoroutine(closestEnemy.Alerted(pos));
        Guard[] remainingGuards = guards.ToArray();
        for (int i = 0; i < remainingGuards.Length; i++)
        {
            if (remainingGuards[i] == closestEnemy)
                continue;
            remainingGuards[i].Alerted();
        }
        
    }

    private Guard ClosestEnemy()
    {
        Guard closestGuard = guards[0];
        if (guards.Count == 0)
            return closestGuard;
        float dist = (Player.position - guards[0].transform.position).sqrMagnitude;
        for (int i = 1; i < guards.Count - 1; i++)
        {
            float newdist = (Player.position - guards[i].transform.position).sqrMagnitude;
            if (dist > newdist)
            {
                dist = newdist;
                closestGuard = guards[i];
            }
        }
        return closestGuard;
    }

    public void PlayerDead()
    {
        foreach (Guard g in guards)
        {
            g.DeactivatePlayerFromManager();
            g.currentState = Guard.States.Finished;
        }
    }
}