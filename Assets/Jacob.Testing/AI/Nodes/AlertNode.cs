using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;


public class AlertNode : Node
{
    private NavMeshAgent agent;
    private PoliceAI police;
    private Transform target;

    private Vector3 currentVelocity;
    private float smoothDamp;

    public AlertNode(NavMeshAgent agent, PoliceAI police, Transform target)
    {
        this.agent = agent;
        this.police = police;
        this.target = target;
        smoothDamp = 1f;
    }

    public override NodeState Evaluate()
    {
        agent.isStopped = true;
        Vector3 direction = target.position - police.transform.position;
        Vector3 currentDirection = Vector3.SmoothDamp(police.transform.forward, direction, ref currentVelocity, smoothDamp);
        Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
        police.transform.rotation = rotation;
        agent.SetDestination(agent.transform.position);
        agent.isStopped = false;
        Debug.Log("is Aware");
        return NodeState.RUNNING;

    }

}