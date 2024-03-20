using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StalkingState : EmptyState
{
    private bool playerLost;
    private bool playerCRRunning;
    private Coroutine trackTimerCR;
    private NavMeshAgent agent;

    public override void stateStart(AmalgamCentralAI amalgamBrain)
    {
        base.stateStart(amalgamBrain);
        amalgamBrain.playerInSight = true;
        playerLost = false;
        playerCRRunning = false;
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    public override Type stateUpdate(AmalgamCentralAI amalgamBrain)
    {
        base.stateUpdate(amalgamBrain);
        if(!amalgamBrain.playerInSight && !playerCRRunning)
        {
            trackTimerCR = StartCoroutine(trackPlayerOutofSight());
        }
        else if (amalgamBrain.playerInSight && playerCRRunning)
        {
            StopCoroutine(trackTimerCR);
            playerCRRunning = false;
            chasePlayer();
        }
        else if(amalgamBrain.playerInSight || playerCRRunning)
        {
            chasePlayer();
        }

        if(playerLost)
        {
            return typeof(RoamingState);
        }
        else
        {
            return typeof(StalkingState);
        }
    }

    public override void stateExit(AmalgamCentralAI amalgamBrain)
    {
        base.stateExit(amalgamBrain);
    }

    public IEnumerator trackPlayerOutofSight()
    {
        playerCRRunning = true;
        yield return new WaitForSeconds(UnityEngine.Random.Range(7f, 10f));

        playerLost = true;
        playerCRRunning = false;
    }

    public void chasePlayer()
    {
        agent.SetDestination(amalgamBrain.playerTracker.transform.position);
    }
}
