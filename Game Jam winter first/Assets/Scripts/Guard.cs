using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Guard : MonoBehaviour, ILiving
{
    [SerializeField]
    public Color ColorToID;

    public States currentState;

    public float waitTime = .3f;

    public float timeToSpotPlayer = 1f;

    [HideInInspector] public Transform player;

    public bool alerted = false;

    [SerializeField]
    private float ReactionTime = 2f;

    private Light spotlight;

    [SerializeField]
    private float viewDistance = 10f;

    [SerializeField]
    private float RangeToSee = 3f;

    [SerializeField]
    private LayerMask viewMask = 0;

    [SerializeField]
    private LayerMask DeadEnemies = 0;

    private float viewAngle;

    [SerializeField]
    private Transform pathHolder = null;

    private Color originalSpotlightColour;

    [SerializeField]
    private Color AlertedColor = Color.yellow;

    [SerializeField]
    private float AlertedAngle = 180;

    private List<Vector3> waypoints = new List<Vector3> ();

    private int currentIndex = 0;

    private float playerVisibleTimer = 0;

    private ThirdPersonCharacter character;

    private NavMeshAgent agent;

    private Vector3 targetWaypoint;

    public delegate void ExecutePlayer ();

    public event ExecutePlayer executePlayer;

    public enum States { Idleing, Walking, Shooting, Finished }

    public void Die ()
    {
        if (currentState == States.Shooting)
            return;
        StopAllCoroutines ();
        character.Die ();
        spotlight.enabled = false;
        GetComponent<Rigidbody> ().isKinematic = true;
        GetComponent<Collider> ().isTrigger = true;
        FindObjectOfType<EnemyManager>().RemoveGuard(this);
        currentState = States.Finished;
        agent.enabled = false;
        GetComponent<DeadEnemy> ().enabled = true;
        
        this.enabled = false;
    }

    public IEnumerator Alerted (Vector3 pos)
    {
        yield return new WaitForSeconds (ReactionTime);
        if (agent.enabled)
        {
            alerted = true;
            gameObject.layer = 12;
            float speed = agent.speed;
            agent.speed = 1;

            agent.SetDestination (pos);

            viewAngle = AlertedAngle;
            spotlight.spotAngle = 100;
            spotlight.color = AlertedColor;
            originalSpotlightColour = spotlight.color;
            currentState = States.Walking;
            StopCoroutine (ReachWaypoint ());
            yield return StartCoroutine (ReachWaypoint (pos));
            agent.speed = speed;
            FollowPath ();
        }
    }

    public void Alerted ()
    {
        alerted = true;
        gameObject.layer = 12;
        viewAngle = AlertedAngle;
        spotlight.spotAngle = viewAngle;
        spotlight.color = AlertedColor;
        originalSpotlightColour = spotlight.color;
    }

    public bool CanSeePlayer ()
    {
        if (currentState == States.Shooting)
            return true;

        Vector3 distance = player.position - transform.position;
        //if (distance.sqrMagnitude < RangeToSee * RangeToSee)
        //{
        //    if (agent.enabled)
        //        DeactivatePlayer ();
        //    return true;
        //}
        if (distance.sqrMagnitude < viewDistance * viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle (transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast (transform.position, player.position, viewMask))
                {
                    if (agent.enabled)
                        DeactivatePlayer ();

                    return true;
                }
            }
        }
        return false;
    }

    public void DeactivatePlayer ()
    {
        character.StopMovement ();
        agent.enabled = false;
        currentState = States.Shooting;
        player.GetComponent<PlayerEnt> ().Deactivate ();
    }

    public void DeactivatePlayerFromManager ()
    {
        StopAllCoroutines ();
        character.StopMovement ();
        agent.enabled = false;
    }

    private void Awake ()
    {
        ColorToID = Random.ColorHSV ();
        agent = GetComponent<NavMeshAgent> ();
        agent.updateRotation = false;
        character = GetComponent<ThirdPersonCharacter> ();
        player = GameObject.FindGameObjectWithTag ("Player").transform;

        for (int i = 0; i < pathHolder.childCount; i++)
        {
            Vector3 pos = pathHolder.GetChild (i).position;
            Vector3 posToAdd = new Vector3 (pos.x, transform.position.y, pos.z);
            waypoints.Add (posToAdd);
        }
    }

    private void Start ()
    {
        spotlight = GetComponentInChildren<Light> ();
        viewAngle = spotlight.spotAngle;
        originalSpotlightColour = spotlight.color;
        StartCoroutine (CanSeeDownAlly ());
    }

    private void SetUp ()
    {
        currentState = States.Idleing;
        agent.enabled = true;
        FollowPath ();
    }

    private void OnEnable ()
    {
        SetUp ();
    }

    private void Update ()
    {
        if (currentState == States.Finished)
        {
            return;
        }
        if (currentState != States.Shooting)
        {
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move (agent.desiredVelocity, false, false);
            }
            else
            {
                character.Move (Vector3.zero, false, false);
            }
        }
        RotateToTarget ();
    }

    private void RotateToTarget ()
    {
        if (CanSeePlayer ())//animate spotlight
        {
            Shooting.Rotate (player.position, transform);
            playerVisibleTimer += Time.deltaTime;
        }
        else
            playerVisibleTimer -= Time.deltaTime;

        playerVisibleTimer = Mathf.Clamp (playerVisibleTimer, 0, timeToSpotPlayer);
        spotlight.color = Color.Lerp (originalSpotlightColour, Color.red, playerVisibleTimer / timeToSpotPlayer);

        if (playerVisibleTimer >= timeToSpotPlayer)
            if (executePlayer != null)
            {
                currentState = States.Finished;
                executePlayer ();
            }
    }

    private IEnumerator CanSeeDownAlly ()
    //for dedecting fallen comrades
    {
        while (alerted != true)
        {
            Physics.queriesHitBackfaces = true;
            if (Physics.Raycast (transform.position, transform.forward, viewDistance, DeadEnemies, QueryTriggerInteraction.Collide))
            {
                Alerted ();
            }

            Physics.queriesHitBackfaces = false;

            yield return null;
        }
    }

    private void FollowPath ()
    {
        if (currentState == States.Idleing)
        {
            targetWaypoint = waypoints[currentIndex];
            agent.SetDestination (targetWaypoint);
            currentState = States.Walking;

            StartCoroutine (ReachWaypoint ());
        }
    }

    private IEnumerator ReachWaypoint ()
    {
        while (currentState == States.Walking)
        {
            Vector3 offset = agent.destination - transform.position;

            if (offset.sqrMagnitude < agent.stoppingDistance)
            {
                currentIndex = (currentIndex + 1) % waypoints.Count;
                currentState = States.Idleing;

                yield return new WaitForSeconds (waitTime);
                FollowPath ();
            }
            yield return null;
        }
    }

    private IEnumerator ReachWaypoint (Vector3 pos)
    {
        while (currentState == States.Walking)
        {
            Vector3 offset = agent.destination - transform.position;

            if (offset.sqrMagnitude < agent.stoppingDistance)
            {
                currentState = States.Idleing;
                yield return new WaitForSeconds (waitTime);
            }
            yield return null;
        }
    }

    private void OnDrawGizmos ()

    {
        Gizmos.color = ColorToID;
        Vector3 startPosition = pathHolder.GetChild (0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere (waypoint.position, .3f);
            Gizmos.DrawLine (previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine (previousPosition, startPosition);

        Gizmos.DrawRay (transform.position, transform.forward * viewDistance);
        Gizmos.DrawWireSphere (transform.position, RangeToSee);
    }
}