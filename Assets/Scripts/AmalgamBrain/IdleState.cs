using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : EmptyState
{

    public bool idledEnough;

    public IdleState()
    {
        GetComponent<NavMeshAgent>().speed = 12f;
    }
    public override void stateStart(AmalgamCentralAI amalgamBrain)
    {
        idledEnough = false;
        StartCoroutine(IdleTimer());
    }

    public override int stateUpdate(AmalgamCentralAI amalgamBrain)
    {
        if (idledEnough && amalgamBrain.interuptedEvent == null)
        {
            return 0;
        }
        else if(amalgamBrain.interuptedEvent != null)
        {
            return 2;
        }
        else
        {
            return 4;
        }
    }

    public override void stateExit(AmalgamCentralAI amalgamBrain)
    {

    }

    public IEnumerator IdleTimer()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 5f));
        idledEnough = true;
    }
}
