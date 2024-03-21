using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyState : MonoBehaviour
{
    public AmalgamCentralAI amalgamBrain;

    public virtual void stateStart(AmalgamCentralAI amalgamBrain)
    {
        amalgamBrain = this.GetComponent<AmalgamCentralAI>();
    }
    public virtual Type stateUpdate(AmalgamCentralAI amalgamBrain) { return null; }
    public virtual void stateExit(AmalgamCentralAI amalgamBrain) { }
}
