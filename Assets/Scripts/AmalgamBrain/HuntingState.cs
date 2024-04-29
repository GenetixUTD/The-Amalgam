using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HuntingState : EmptyState
{
    private NavMeshAgent agent;

    public HuntingState()
    {

    }
    public override void stateStart(AmalgamCentralAI amalgamBrain)
    {

        agent = GetComponent<NavMeshAgent>();
    }

    public override int stateUpdate(AmalgamCentralAI amalgamBrain)
    {

        agent.SetDestination(amalgamBrain.interuptedEvent.position);
        if(agent.remainingDistance < 5f)
        {
            return 4;
        }
        else if(amalgamBrain.playerInSight)
        {
            return 5;
        }
        else
        {
            return 2;
        }
    }

    public override void stateExit(AmalgamCentralAI amalgamBrain)
    {

    }
}
