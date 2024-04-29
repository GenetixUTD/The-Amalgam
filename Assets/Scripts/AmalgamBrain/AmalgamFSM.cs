using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AmalgamFSM : MonoBehaviour
{
    public AmalgamCentralAI amalgamBrain;
    public List<EmptyState> amalgamStates;

    public EmptyState currentState;

    private void Start()
    {
        //Fetch all Amalgam Commands
        amalgamBrain = GetComponent<AmalgamCentralAI>();
        //Start in Roaming State
        //Debug.Log("state start");
    }

    private void Update()
    {
        if (currentState == null)
        {
            currentState = amalgamStates.First();
            Debug.Log(currentState.GetType());
            currentState.stateStart(amalgamBrain);
        }
        //Run state's update and store output (output will be next state)
        int temp = currentState.stateUpdate(amalgamBrain);

        //If the stored state is different -> switch states to new state
        if (temp != amalgamStates.IndexOf(currentState))
        {
            switchStates(temp);
        }
    }

    public void GrabAllStates(List<EmptyState> states)
    {
        //Fetch dictionary of all states
        amalgamStates = states;
    }

    public void switchStates(int state)
    {
        //Run current state's exit, create reference to new state, run new state's start
        currentState.stateExit(amalgamBrain);
        currentState = amalgamStates[state];
        currentState.stateStart(amalgamBrain);
    }
}
