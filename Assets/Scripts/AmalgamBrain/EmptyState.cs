using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyState : MonoBehaviour
{
    public AmalgamCentralAI amalgamBrain;

    public virtual void stateStart(AmalgamCentralAI amalgamBrain)  
    {
        amalgamBrain = this.gameObject.GetComponent<AmalgamCentralAI>();
    }
    public virtual int stateUpdate(AmalgamCentralAI amalgamBrain) { return 0; }
    public virtual void stateExit(AmalgamCentralAI amalgamBrain) { }
}
