using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyState : MonoBehaviour
{
    AmalgamCentralAI amalgamBrain;

    public virtual void stateStart(AmalgamCentralAI amalgamBrain) { }
    public virtual Type stateUpdate(AmalgamCentralAI amalgamBrain) { return null; }
    public virtual void stateExit(AmalgamCentralAI amalgamBrain) { }
}
