using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeavingState : EmptyState
{
    public NavMeshAgent agent;

    public override void stateStart(AmalgamCentralAI amalgamBrain)
    {
        base.stateStart(amalgamBrain);
        agent = GetComponent<NavMeshAgent>();
    }

    public override Type stateUpdate(AmalgamCentralAI amalgamBrain)
    {
        base.stateUpdate(amalgamBrain);
        agent.SetDestination(findClosestExit(amalgamBrain.leavingAreas).position);
        if(amalgamBrain.interuptedEvent)
        {
            return typeof(HuntingState);
        }
        return typeof(LeavingState);
    }

    public override void stateExit(AmalgamCentralAI amalgamBrain)
    {
        base.stateExit(amalgamBrain);
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
