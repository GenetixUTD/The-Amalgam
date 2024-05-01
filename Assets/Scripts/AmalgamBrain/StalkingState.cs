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

    public StalkingState()
    {
        
    }

    public override void stateStart(AmalgamCentralAI amalgamBrain)
    {

        amalgamBrain.playerInSight = true;
        playerLost = false;
        playerCRRunning = false;
        agent = gameObject.GetComponent<NavMeshAgent>();
        amalgamBrain.tensionMeter += 50;
        agent.speed = 15f;
    }

    public override int stateUpdate(AmalgamCentralAI amalgamBrain)
    {

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

        if(amalgamBrain.playerDistance() < 5f)
        {
            amalgamBrain.PlayerDeath();
        }

        if(playerLost)
        {
            return 0;
        }
        else if(!amalgamBrain.playerInSight && playerCRRunning && amalgamBrain.playerTracker.GetComponent<charactercontroller>().ActiveState == charactercontroller.PlayerState.HidingGame)
        {
            return 1;
        }
        else
        {
            return 5;
        }

}

    public override void stateExit(AmalgamCentralAI amalgamBrain)
    {

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
