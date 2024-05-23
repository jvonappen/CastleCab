using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

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

    private float RD;
    private float SD;


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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wagon" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "NPC")
        {
            agent.enabled = false;

            TimerManager.RunAfterTime(() =>
            {
                RD = 1;
                agent.enabled = true;
                agent = this.gameObject.GetComponent<NavMeshAgent>();
                thisTransform = this.agent.transform;
                agent.speed = movementSpeed;
            }, 6);
        }

    }

    private void Wander()
    {
        RD = this.agent.remainingDistance;
        SD = this.agent.stoppingDistance;

        if (RD <= 2)
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
