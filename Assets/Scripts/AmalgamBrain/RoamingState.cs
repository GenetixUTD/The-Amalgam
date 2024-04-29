using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;

public class RoamingState : EmptyState
{
    private float SearchRange;
    private Transform centerPoint;
    private bool pointFound;

    private NavMeshAgent agent;

    public RoamingState()
    {

    }
    public override void stateStart(AmalgamCentralAI amalgamBrain)
    {
        //Define search range
        SearchRange = 100f;
        //Amalgam serves as the center of the search area
        //Debug.Log("fetching center point");
        centerPoint = amalgamBrain.gameObject.transform;
        agent = amalgamBrain.gameObject.GetComponent<NavMeshAgent>();
        pointFound = false;
        Debug.Log(Vector3.Distance(this.transform.position, amalgamBrain.playerTracker.transform.position));
    }

    public override int stateUpdate(AmalgamCentralAI amalgamBrain)
    {
        if (amalgamBrain.playerInSight)
        {
            return 5;
        }
        else if (amalgamBrain.interuptedEvent != null)
        {
            return 2;
        }
        if(!pointFound)
        {
            //Find suitable Roam Position
            Vector3 point;
            if(RandomPointOnNav(centerPoint.position, SearchRange, out point))
            {
                //Set roam position in navmeshagent
                agent.SetDestination(point);
                pointFound = true;
            }
        }
        // if the agent is close enough to the random point
        else if(agent.remainingDistance <= agent.stoppingDistance && pointFound)
        {
            //idle
            return 4;
        }
        //if agent is still looking for a point or is travelling to a point, remain in roaming state
        return 0;
    }

    public override void stateExit(AmalgamCentralAI amalgamBrain)
    {

    }

    public bool RandomPointOnNav(Vector3 centerPoint, float range, out Vector3 resultPoint)
    {
        //Find a random point in a sphere of searchrange radius around the amalgam
        Vector3 RandomSearchPoint = centerPoint + UnityEngine.Random.insideUnitSphere * range;
        NavMeshHit hit;
        // Can the amalgam travel to this point on the navmesh?
        if (NavMesh.SamplePosition(RandomSearchPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            //return that the amalgam has found a suitable position
            resultPoint = hit.position;
            return true;
        }
        // if not remain still and continue searching on next update
        resultPoint = Vector3.zero;
        return false;

    }
}
