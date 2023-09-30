using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class VillagerAI : MonoBehaviour
{

    [Tooltip("The speed the animal moves. If left at 0, the speed will default to the navMesh speed.")]
    [SerializeField] private float movementSpeed = 3.5f;

    [Header("Wander")]
    [Tooltip("The center of the wandering radius. Use a fixed object to keep the wander area fixed. Use the object itself, to have complete free roam.")]
    [SerializeField] private Transform wanderTransform;
    [Tooltip("How far the AI will wander from the wander tranform.")]
    [SerializeField] private float wanderRange = 25;

    private NavMeshAgent agent;
    private Transform thisTransform;

    private void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        thisTransform = this.agent.transform;
        agent.speed = movementSpeed;
    }

    private void Update()
    {
        if (agent.enabled == true)
        {
            Wander();
        }
        
    }

    private void Wander()
    {
        float RD = this.agent.remainingDistance;
        float SD = this.agent.stoppingDistance;

        if (RD <= SD)
        {
            Vector3 point;
            if (FindRandomPoint(wanderTransform.position, wanderRange, out point)) //pass in centrepoint and radius of area
            {
                this.agent.SetDestination(point);
                thisTransform.LookAt(point);
                thisTransform.Rotate(point);
                thisTransform.forward = point;
            }
        }
    }

    bool FindRandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range; //random point in a sphere 
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
