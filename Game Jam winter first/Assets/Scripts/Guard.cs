using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
public class Guard : MonoBehaviour
{
    [SerializeField]
    public Color ColorToID;
    public enum States { Idleing, Walking, Finished, Shooting }
    public static event System.Action OnGuardHasSpottedPlayer;
    States currentState;
    public float waitTime = .3f;
    public float timeToSpotPlayer = 1f;
    Light spotlight;
    [SerializeField]
    private float viewDistance = 10f;
    [SerializeField]
    private float RangeToSee = 3f;
    public LayerMask viewMask;
    float viewAngle;
    [SerializeField]
    private Transform pathHolder = null;
    [HideInInspector] public Transform player;
    Color originalSpotlightColour;
    [SerializeField]
    private Color AlertedColor = Color.yellow;
    [SerializeField]
    private float AlertedAngle = 180;
    private List<Vector3> waypoints = new List<Vector3>();
    int currentIndex = 0;
    float playerVisibleTimer = 0;
    private ThirdPersonCharacter character;
    NavMeshAgent agent;
    Vector3 targetWaypoint;
    void Awake()
    {
        ColorToID = Random.ColorHSV();
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
        RotateToTarget();




    }
    void RotateToTarget()
    {
        if (CanSeePlayer())//animate spotlight
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

        Vector3 distance = player.position - transform.position;
        if (distance.sqrMagnitude < RangeToSee * RangeToSee)
        {
            DeactivatePlayer();
            return true;
        }
        if (distance.sqrMagnitude < viewDistance * viewDistance)
        {

            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    DeactivatePlayer();

                    return true;
                }
            }
        }
        return false;
    }
    void DeactivatePlayer()
    {
        currentState = States.Shooting;
        StopAllCoroutines();
        character.StopMovement();
        agent.enabled = false;

        player.GetComponent<PlayerEnt>().Deactivate();
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
        Gizmos.color = ColorToID;
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {

            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);


        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
        Gizmos.DrawWireSphere(transform.position, RangeToSee);
    }

}
