using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System.Linq;

public class AmalgamCentralAI : MonoBehaviour
{
    private float stunHealth;

    public bool playerInSight;

    public Transform interuptedEvent;

    public GameObject playerTracker;

    public Transform[] level2eavingAreas;
    public Transform[] level1eavingAreas;
    public Transform[] level0eavingAreas;

    public Transform playerRespawnLocation;

    public Transform OffMap;

    public Transform[] currentAmalgamSpawns; 

    public float tensionMeter;

    private int randomIndex;

    public AmalgamFSM FSMLogic;

    [SerializeField]
    public List<EmptyState> states;

    private bool pauseTracker;

    private float tensionDecayTimer;
    private bool tensionDecaying;

    private charactercontroller.PlayerState stateTracker;

    private void Start()
    {
        interuptedEvent = null;
        FSMLogic = GetComponent<AmalgamFSM>();
        /*states = new Dictionary<Type, EmptyState>()
        {
            {typeof(RoamingState), new RoamingState() },
            {typeof(IdleState), new IdleState() },
            {typeof(StalkingState), new StalkingState() },
            {typeof(HuntingState), new HuntingState() },
            {typeof(HauntingState), new HauntingState() },
            {typeof(LeavingState), new LeavingState() }
        };*/
        tensionMeter = 100;
        tensionDecayTimer = 0;
        Debug.Log(states.ToList());
        FSMLogic.GrabAllStates(states);
        //gameObject.GetComponent<AmalgamFSM>().enabled = false;
    }

    private void Update()
    {
        trackPlayerVisability();
        

        if (stateTracker == charactercontroller.PlayerState.Paused)
        {
            gameObject.GetComponent<NavMeshAgent>().SetDestination(this.transform.position);
            gameObject.GetComponent<AmalgamFSM>().enabled = false;
            pauseTracker = true;
            Debug.Log("Pause");
        }
        else if (stateTracker != charactercontroller.PlayerState.Paused && pauseTracker == true)
        {
            Debug.Log("unpause");
            if (!tensionDecaying)
            {
                gameObject.GetComponent<AmalgamFSM>().enabled = true;
            } 
            pauseTracker = false;
        }
        stateTracker = playerTracker.GetComponent<charactercontroller>().ActiveState;
        if (tensionMeter == 0 && !this.GetComponent<AmalgamFSM>().enabled && pauseTracker != true)
        {
            Debug.Log("newspawn");
            currentAmalgamSpawns = fetchAmalgamSpawns();
            startAmalgam();
        }

        if((tensionMeter >= 100 && !this.GetComponent<AmalgamFSM>().enabled || tensionDecaying) && pauseTracker == false)
        {
            tensionDecaying = true;
            tensionDecayTimer += Time.deltaTime;
            if (tensionDecayTimer > 1)
            {
                tensionMeter -= UnityEngine.Random.Range(0f, 5f);
                tensionDecayTimer = 0;
            }
            tensionMeter = Mathf.Clamp(tensionMeter, 0, 100);
            if(tensionMeter == 0)
            {
                tensionDecaying = false;
            }
        }
        
    }

    private void trackPlayerVisability()
    {
        RaycastHit hit;
        Vector3 dir = -this.transform.position + playerTracker.transform.position; 
        if (Physics.Raycast(transform.position, dir * 10, out hit, 1000.0f))
        {
            float temp = Vector3.Dot(dir.normalized, transform.forward);
            temp = (Mathf.Acos(temp) * Mathf.Rad2Deg);
            if(hit.transform.tag == "Player" && temp < 70)
            {
                
                playerInSight = true;
            }
            else
            {
                playerInSight = false;
            }
        }
    }

    public float playerDistance()
    {
        return Vector3.Distance(this.transform.position, playerTracker.transform.position);
    }
    
    public bool startAmalgam()
    {
        this.transform.position = currentAmalgamSpawns[new System.Random().Next(0, currentAmalgamSpawns.Length)].position;
        this.gameObject.GetComponent<AmalgamFSM>().enabled = true;
        this.GetComponent<NavMeshAgent>().enabled = true;
        this.gameObject.GetComponent<AmalgamFSM>().switchStates(0);
        return true;
    }

    public Transform[] fetchAmalgamSpawns()
    {
        switch (playerTracker.GetComponent<charactercontroller>().currentFloor)
        {
            case 2:
                return level2eavingAreas;
            case 1:
                return level1eavingAreas;
            case 0:
                return level0eavingAreas;
            default:
                return new Transform[0];
        }
    }

    public void stopAmalgam()
    {
        this.GetComponent<NavMeshAgent>().enabled = false;
        this.GetComponent<AmalgamFSM>().enabled = false;
        this.gameObject.transform.position = OffMap.position;
    }

    public void PlayerDeath()
    {
        stopAmalgam();
        tensionMeter = 100;
        playerTracker.GetComponent<CharacterController>().enabled = false;
        playerTracker.gameObject.transform.position = playerRespawnLocation.position;
        playerTracker.GetComponent<CharacterController>().enabled = true;
    }
}
