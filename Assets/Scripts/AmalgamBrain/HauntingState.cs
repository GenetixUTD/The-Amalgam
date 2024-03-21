using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HauntingState : EmptyState
{
    private float SearchRange;
    private Transform centerPoint;
    private Locker lockerTracker;
    private bool pointFound;

    private NavMeshAgent agent;


    public override void stateStart(AmalgamCentralAI amalgamBrain)
    {
        base.stateStart(amalgamBrain);
        lockerTracker = amalgamBrain.playerTracker.GetComponent<charactercontroller>().hidingLocker.GetComponent<Locker>();
        centerPoint = lockerTracker.ExitArea;
        SearchRange = 3f;
        agent = GetComponent<NavMeshAgent>();
        pointFound = false;
    }

    public override Type stateUpdate(AmalgamCentralAI amalgamBrain)
    {
        base.stateUpdate(amalgamBrain);

        if (!pointFound)
        {
            Vector3 point;
            if (RandomPointNearHide(centerPoint.position, SearchRange, out point))
            {
                agent.SetDestination(point);
                amalgamBrain.playerTracker.GetComponent<charactercontroller>().ActiveState = charactercontroller.PlayerState.HidingGame;
                pointFound = true;
            }
        }
        else if (agent.remainingDistance <= agent.stoppingDistance && pointFound && !lockerTracker.minigameOccuring && !lockerTracker.minigameComplete)
        {
            lockerTracker.minigameOccuring = true;
        }
        else if (lockerTracker.minigameComplete)
        {
            return typeof(LeavingState);
        }
        else if(lockerTracker.minigameFailed)
        {
            return typeof(StalkingState);
        }
        return typeof(HauntingState);
    }

    public bool RandomPointNearHide(Vector3 centerPoint, float range, out Vector3 resultPoint)
    {
        Vector3 RandomSearchPoint = centerPoint + UnityEngine.Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(RandomSearchPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            resultPoint = hit.position;
            return true;
        }
        resultPoint = Vector3.zero;
        return false;

    }
}
