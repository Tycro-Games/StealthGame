using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
public class PlayerController : MonoBehaviour
{
    Camera cam;
    [SerializeField]
    private float MaxDistance = 100;

    NavMeshAgent agent;
    ThirdPersonCharacter character;
    [HideInInspector]
    public bool reached = true;
    Vector3 DesiredDestination;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();
    }
    void Start()
    {
        DesiredDestination = transform.position;
        cam = Camera.main;


        agent.updateRotation = false;
    }
    public void StopDestination()
    {

        character.StopMovement();
        agent.enabled = false;

    }
    public void ResumeDestination()
    {

        agent.enabled = true;
        agent.destination = DesiredDestination;//resume destination
    }

    // Update is called once per frame
    void Update()
    {

        if (agent.enabled == false)
        {
            if (PlayerShooting.shooting != true)
                agent.enabled = true;
            else
                return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, MaxDistance))
            {
                if (hit.transform.gameObject.tag == "walkable")
                {
                    agent.SetDestination(hit.point);
                    reached = false;
                    DesiredDestination = hit.point;
                }
                else
                {
                    return;
                }
            }
        }
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false);
        }
        else
        {

            character.Move(Vector3.zero, false, false);
            reached = true;
        }
    }
}
