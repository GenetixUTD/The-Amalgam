using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AmalgamCentralAI : MonoBehaviour
{
    private float stunHealth;

    private float movementSpeed;
    private float rotationSpeed;

    private Vector3 targetLocation;
    public GameObject navMeshCenter;

    private NavMeshAgent movementAgent;
    public GameObject playerTracker;
    public Transform movementGoal;
    
}
