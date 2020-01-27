using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
public class Guard : MonoBehaviour
{
    public enum States { Idleing, Walking, Chasing, Shooting }
    States currentState;
    public float waitTime = .3f;
    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;
    float viewAngle;

    public Transform pathHolder;
    [HideInInspector] public Transform player;
    Color originalSpotlightColour;
    private List<Vector3> waypoints = new List<Vector3>();
    int currentIndex = 0;

    private ThirdPersonCharacter character;
    NavMeshAgent agent;
    Vector3 targetWaypoint;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        character = GetComponent<ThirdPersonCharacter>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotlight.spotAngle;
        originalSpotlightColour = spotlight.color;
        for (int i = 0; i < pathHolder.childCount; i++)
        {
            Vector3 pos = pathHolder.GetChild(i).position;
            Vector3 posToAdd = new Vector3(pos.x, transform.position.y, pos.z);
            waypoints.Add(posToAdd);
        }

    }
    void SetUp()
    {
        currentState = States.Idleing;
        agent.enabled = true;
        FollowPath();
    }
    private void OnEnable()
    {
        SetUp();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        character.StopMovement();
        agent.enabled = false;
    }
    void Update()
    {
        if (CanSeePlayer())
        {
            spotlight.color = Color.red;
            //check if the player is dead
        }
        else
        {
            spotlight.color = originalSpotlightColour;
        }
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false);

        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }
    }


    public bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void FollowPath()
    {
        if (currentState == States.Idleing)
        {
            targetWaypoint = waypoints[currentIndex];
            agent.SetDestination(targetWaypoint);
            currentState = States.Walking;

            StartCoroutine(ReachWaypoint());

        }
    }
    IEnumerator ReachWaypoint()
    {
        while (currentState == States.Walking)
        {
            Vector3 offset = agent.destination - transform.position;

            if (offset.sqrMagnitude < agent.stoppingDistance)
            {
                currentIndex = (currentIndex + 1) % waypoints.Count;
                currentState = States.Idleing;

                yield return new WaitForSeconds(waitTime);
                FollowPath();

            }
            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }

}
