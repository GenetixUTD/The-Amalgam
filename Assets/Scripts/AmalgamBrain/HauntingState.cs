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

    public HauntingState()
    {
        
    }

    public override void stateStart(AmalgamCentralAI amalgamBrain)
    {

        lockerTracker = amalgamBrain.playerTracker.GetComponent<charactercontroller>().hidingLocker.GetComponent<Locker>();
        centerPoint = lockerTracker.ExitArea;
        SearchRange = 30f;
        agent = GetComponent<NavMeshAgent>();
        pointFound = false;
        amalgamBrain.tensionMeter = 90;
        agent.speed = 12f;
    }

    public override int stateUpdate(AmalgamCentralAI amalgamBrain)
    {

        QTEGame.QTEState temp = amalgamBrain.playerTracker.GetComponent<charactercontroller>().qtescript.GetComponent<QTEGame>().gameState;

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
        else if (agent.remainingDistance <= agent.stoppingDistance && pointFound && temp == QTEGame.QTEState.Idle)
        {
            amalgamBrain.playerTracker.GetComponent<charactercontroller>().EnableQTE();
        }
        else if (temp == QTEGame.QTEState.Success)
        {
            return 3;
        }
        else if(temp == QTEGame.QTEState.Failed || amalgamBrain.playerTracker.GetComponent<charactercontroller>().ActiveState == charactercontroller.PlayerState.Active)
        {
            return 5;
        }
        return 1;
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
