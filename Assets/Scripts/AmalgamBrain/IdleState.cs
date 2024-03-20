using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EmptyState
{

    public bool idledEnough;
    public bool interuptedEvent;

    public override void stateStart(AmalgamCentralAI amalgamBrain)
    {
        base.stateStart(amalgamBrain);
        idledEnough = false;
        interuptedEvent = false;
        StartCoroutine(IdleTimer());
    }

    public override Type stateUpdate(AmalgamCentralAI amalgamBrain)
    {
        base.stateUpdate(amalgamBrain);
        if (idledEnough && !interuptedEvent)
        {
            return typeof(RoamingState);
        }
        else if(interuptedEvent)
        {
            return typeof(HuntingState);
        }
        else
        {
            return typeof(IdleState);
        }
    }

    public override void stateExit(AmalgamCentralAI amalgamBrain)
    {
        base.stateExit(amalgamBrain);
    }

    public IEnumerator IdleTimer()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 5f));
        idledEnough = true;
    }
}
