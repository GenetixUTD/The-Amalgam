using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AmalgamFSM : MonoBehaviour
{
    public AmalgamCentralAI amalgamBrain;
    private Dictionary<Type, EmptyState> amalgamStates;

    public EmptyState currentState;

    private void Start()
    {
        //Fetch all Amalgam Commands
        amalgamBrain = GetComponent<AmalgamCentralAI>();
        //Start in Roaming State
        currentState = amalgamStates.Values.First();
    }

    private void Update()
    {
        //Run state's update and store output (output will be next state)
        var temp = currentState.stateUpdate(amalgamBrain);

        //If the stored state is different -> switch states to new state
        if (temp != currentState.GetType())
        {
            switchStates(temp);
        }
    }

    public void GrabAllStates(Dictionary<Type, EmptyState> states)
    {
        //Fetch dictionary of all states
        amalgamStates = states;
    }

    public void switchStates(Type state)
    {
        //Run current state's exit, create reference to new state, run new state's start
        currentState.stateExit(amalgamBrain);
        currentState = amalgamStates[state];
        currentState.stateStart(amalgamBrain);
    }
}
