using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AmalgamCentralAI : MonoBehaviour
{
    private float stunHealth;

    private float movementSpeed;
    private float rotationSpeed;

    public bool playerInSight;

    public bool interuptedEvent;

    private Vector3 targetLocation;
    public GameObject navMeshCenter;

    private NavMeshAgent movementAgent;
    public GameObject playerTracker;
    public Transform movementGoal;

    public Transform[] leavingAreas;

    private void Start()
    {
        interuptedEvent = false;
    }

    private void Update()
    {
        trackPlayerVisability();
    }

    private void trackPlayerVisability()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerTracker.transform.position, out hit, Mathf.Infinity))
        {
            if(hit.transform.tag == "Player")
            {
                playerInSight = true;
            }
            else
            {
                playerInSight = false;
            }
        }
    }
    
}
