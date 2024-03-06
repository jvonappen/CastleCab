using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class WanderNode : Node
{
    private NavMeshAgent agent;
    private float range;
    private Transform centrePoint;

    public WanderNode(Transform centrePoint, NavMeshAgent agent, float range)
    {
        this.centrePoint = centrePoint;
        this.agent = agent;
        this.range = range;
    }

    public override NodeState Evaluate()
    {

        WanderAbout();
        if (agent.isStopped != true)
        {
            Debug.Log("Wandering Around");
            return NodeState.RUNNING;
        }
        else
        {
            WanderAbout();
            return NodeState.FAIL;
        }

    }

    public void WanderAbout()
    {
        float RD = agent.remainingDistance;
        float SD = agent.stoppingDistance;

        if (RD <= SD)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) //pass in centrepoint and radius of area
            {
                agent.SetDestination(point);
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}