using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
public class Guard : MonoBehaviour
{
    public enum States { Idleing, Walking, Finished, Shooting }
    public static event System.Action OnGuardHasSpottedPlayer;


    States currentState;
    public float waitTime = .3f;
    public float timeToSpotPlayer = 1f;
    Light spotlight;
    public float viewDistance;
    public float RangeToSee;
    public LayerMask viewMask;
    float viewAngle;

    public Transform pathHolder;
    [HideInInspector] public Transform player;
    Color originalSpotlightColour;
    public Color AlertedColor;
    public float AlertedAngle = 180;
    private List<Vector3> waypoints = new List<Vector3>();
    int currentIndex = 0;
    float playerVisibleTimer = 0;
    private ThirdPersonCharacter character;
    NavMeshAgent agent;
    Vector3 targetWaypoint;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        character = GetComponent<ThirdPersonCharacter>();
        player = GameObject.FindGameObjectWithTag("Player").transform;


        for (int i = 0; i < pathHolder.childCount; i++)
        {
            Vector3 pos = pathHolder.GetChild(i).position;
            Vector3 posToAdd = new Vector3(pos.x, transform.position.y, pos.z);
            waypoints.Add(posToAdd);
        }

    }
    private void Start()
    {
        spotlight = GetComponentInChildren<Light>();
        viewAngle = spotlight.spotAngle;
        originalSpotlightColour = spotlight.color;

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
        PlayerShooting.onAlert += Alerted;
    }
    private void OnDisable()
    {
        PlayerShooting.onAlert -= Alerted;
    }
    void Update()
    {
        if (currentState == States.Finished)
        {
            return;
        }
        if (currentState != States.Shooting)
        {
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity, false, false);

            }
            else
            {

                character.Move(Vector3.zero, false, false);
            }
        }
        if (currentState != States.Shooting)
        {
            Vector3 offset = player.position - transform.position;
            if (offset.sqrMagnitude <= RangeToSee)
            {
                RotateToTarget(true);
            }
        }
        else
            RotateToTarget();



    }
    void RotateToTarget(bool Rotate = false)
    {
        if (CanSeePlayer() || Rotate)//animate spotlight
        {
            Shooting.Rotate(player.position, transform);
            playerVisibleTimer += Time.deltaTime;

        }
        else
            playerVisibleTimer -= Time.deltaTime;

        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
        spotlight.color = Color.Lerp(originalSpotlightColour, Color.red, playerVisibleTimer / timeToSpotPlayer);


        if (playerVisibleTimer >= timeToSpotPlayer)
            if (OnGuardHasSpottedPlayer != null)
            {
                currentState = States.Finished;
                OnGuardHasSpottedPlayer();
            }
    }
    public IEnumerator Alerted(Vector3 pos)
    {
        float speed = agent.speed;
        agent.speed = 1;

        agent.SetDestination(pos);

        viewAngle = AlertedAngle;
        spotlight.spotAngle = viewAngle;
        spotlight.color = AlertedColor;
        originalSpotlightColour = spotlight.color;
        currentState = States.Walking;
        StopCoroutine(ReachWaypoint());
        yield return StartCoroutine(ReachWaypoint(pos));
        agent.speed = speed;
        FollowPath();


    }
    public void Alerted()
    {

        viewAngle = AlertedAngle;
        spotlight.spotAngle = viewAngle;
        spotlight.color = AlertedColor;
        originalSpotlightColour = spotlight.color;

    }
    public bool CanSeePlayer()
    {
        if (currentState == States.Shooting)
            return true;
        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    currentState = States.Shooting;
                    StopAllCoroutines();
                    character.StopMovement();
                    agent.enabled = false;

                    player.GetComponent<PlayerEnt>().Deactivate();

                    return true;
                }
            }
        }
        return false;
    }
    public void CanSeeDownAlly()//for dedecting fallen comrades
    {

        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    Alerted();
                }
            }
        }

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
    IEnumerator ReachWaypoint(Vector3 pos)
    {
        while (currentState == States.Walking)
        {
            Vector3 offset = agent.destination - transform.position;

            if (offset.sqrMagnitude < agent.stoppingDistance)
            {
                currentState = States.Idleing;
                yield return new WaitForSeconds(waitTime);

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
