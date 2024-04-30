using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeavingState : EmptyState
{
    public NavMeshAgent agent;

    public LeavingState()
    {

    }

    public override void stateStart(AmalgamCentralAI amalgamBrain)
    {

        agent = GetComponent<NavMeshAgent>();
    }

    public override int stateUpdate(AmalgamCentralAI amalgamBrain)
    {

        agent.SetDestination(findClosestExit(amalgamBrain.leavingAreas).position);
        if(amalgamBrain.interuptedEvent)
        {
            return 2;
        }
        else if(amalgamBrain.playerInSight)
        {
            return 5;
        }
        return 3;
    }

    public override void stateExit(AmalgamCentralAI amalgamBrain)
    {

    }

    public Transform findClosestExit(Transform[] exits)
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach(Transform t in exits)
        {
            float exitdistance = Vector3.Distance(t.position, currentPos);
            if(exitdistance < minDist)
            {
                closest = t;
                minDist = exitdistance;
            }
        }
        return closest;
    }
}
