using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
public class PlayerController : MonoBehaviour
{
    Camera cam;
    public LayerMask CanMoveLayer;
    public float MaxDistance = 100;
    NavMeshAgent agent;
    ThirdPersonCharacter character;

    Vector3 DesiredDestination;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
        character = GetComponent<ThirdPersonCharacter>();
        agent.updateRotation = false;
    }
    private void OnDisable()
    {

        character.StopMovement();
        agent.enabled = false;
    }
    private void OnEnable()
    {
        if (agent != null)
        {
            agent.enabled = true;
            agent.destination = DesiredDestination;//resume destination
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, MaxDistance, CanMoveLayer))
            {
                agent.SetDestination(hit.point);
                DesiredDestination = hit.point;
            }
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
}
