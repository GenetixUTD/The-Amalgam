using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HuntingState : EmptyState
{
    private NavMeshAgent agent;

    private float searchTimer;

    public HuntingState()
    {
        
    }
    public override void stateStart(AmalgamCentralAI amalgamBrain)
    {

        agent = GetComponent<NavMeshAgent>();
        amalgamBrain.footstepFrequency = amalgamBrain.sprintingFootstepFrequency;
        AkSoundEngine.SetSwitch("AmalgamFootsteps", "Sprinting", this.gameObject);
        searchTimer = 0;
        agent.speed = 15f;
    }

    public override int stateUpdate(AmalgamCentralAI amalgamBrain)
    {
        searchTimer += Time.deltaTime;
        agent.SetDestination(amalgamBrain.interuptedEvent.transform.position);

        if(amalgamBrain.playerInSight)
        {
            return 5;
        }
        if (agent.remainingDistance < 5f && !Physics.Linecast(this.transform.position, amalgamBrain.interuptedEvent.transform.position))
        {
            amalgamBrain.interuptedEvent.SetActive(false);
            return 4;
        }
        else if (!agent.hasPath && searchTimer > 5f)
        {
            amalgamBrain.interuptedEvent.SetActive(false);
            return 3;
            
        }
        else
        {
            return 2;
        }
    }

    public override void stateExit(AmalgamCentralAI amalgamBrain)
    {
        amalgamBrain.interuptedEvent = null;
    }
}
