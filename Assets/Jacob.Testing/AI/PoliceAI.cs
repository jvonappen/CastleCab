using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class PoliceAI : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [Tooltip("Distance the AI will look at the player.")]
    [SerializeField] private float awareRange;
    [Tooltip("The speed the animal moves. If left at 0, the speed will default to the navMesh speed.")]
    [SerializeField] private float movementSpeed = 3.5f;

    [Header("Wander")]
    [Tooltip("The center of the wandering radius. Use a fixed object to keep the wander area fixed. Use the object itself, to have complete free roam.")]
    [SerializeField] private Transform wanderTransform;
    [Tooltip("How far the AI will wander from the wander tranform.")]
    [SerializeField] private float wanderRange = 25;

    [Header("Dishonour Level One")]
    [Tooltip("Distance the AI will chase away the player.")]
    [SerializeField] private float chasingRange1 = 5;
    [Tooltip("Speed modifier for a level one Dishonour Level.")]
    [SerializeField] private float chaseSpeed1 = 5;
    [Tooltip("Distance the AI will search for the player.")]
    [SerializeField] private float searchRange1 = 5;

    [Header("Dishonour Level Two")]
    [Tooltip("Distance the AI will chase away the player.")]
    [SerializeField] private float chasingRange2 = 10;
    [Tooltip("Speed modifier for a level two Dishonour Level.")]
    [SerializeField] private float chaseSpeed2 = 10;
    [Tooltip("Distance the AI will search for the player.")]
    [SerializeField] private float searchRange2 = 10;

    [Header("Dishonour Level Three")]
    [Tooltip("Distance the AI will chase away the player.")]
    [SerializeField] private float chasingRange3 = 20;
    [Tooltip("Speed modifier for a level three Dishonour Level.")]
    [SerializeField] private float chaseSpeed3 = 15f;
    [Tooltip("Distance the AI will search for the player.")]
    [SerializeField] private float searchRange3 = 15;

    private NavMeshAgent agent;
    private Transform thisTransform;

    //Dishonour Stuff

    [Header("Debug")]
    [SerializeField] private float chasingRange0; //how long for chase
    [SerializeField] private float chaseSpeed0; // speed
    [SerializeField] private float searchRange0; //aka wander




    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        thisTransform = agent.transform;
    }

    private void Update()
    {
        DishonourEvaluate();
        //WanderEvaluate();
        //InRange(searchRange0, playerTransform, wanderTransform);
        
    }


    private void DishonourEvaluate()
    {
        if (Dishonour.dishonourLevel < Dishonour._oneStar)
        {
            DishonourZero();
        }
        if (Dishonour.dishonourLevel >= Dishonour._oneStar)
        {
            DishonourOne();
        }
        if (Dishonour.dishonourLevel >= Dishonour._twoStar)
        {
            DishonourTwo();
        }
        if (Dishonour.dishonourLevel >= Dishonour._threeStar)
        {
            DishonourThree();
        }
        InRange();
        agent.speed= searchRange0;
    }

    private void DishonourZero()   { chasingRange0 = 0;             chaseSpeed0 = movementSpeed; searchRange0 = wanderRange; }
    private void DishonourOne()    { chasingRange0 = chasingRange1; chaseSpeed0 = chaseSpeed1; searchRange0 = searchRange1; }
    private void DishonourTwo()    { chasingRange0 = chasingRange2; chaseSpeed0 = chaseSpeed2; searchRange0 = searchRange2; }
    private void DishonourThree()  { chasingRange0 = chasingRange3; chaseSpeed0 = chaseSpeed3; searchRange0 = searchRange3; }

    private void WanderAllOver()
    {
        float RD = agent.remainingDistance;
        float SD = agent.stoppingDistance;

        if (RD <= SD)
        {
            Vector3 point;
            if (FindRandomPoint(wanderTransform.position, searchRange0, out point)) //pass in centrepoint and radius of area
            {
                agent.SetDestination(point);
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

    private void WanderEvaluate()
    {
        WanderAllOver();
        if (agent.isStopped != true)
        {
            Debug.Log("Wandering Around");           
        }
        else
        {
            Debug.Log("Wandering Done");
            WanderAllOver();
        }
    }

    private void Chase()
    {

        float distance = Vector3.Distance(playerTransform.position, agent.transform.position);
        if (distance > chasingRange0)
        {
            agent.isStopped = false;
            agent.SetDestination(playerTransform.position);
            Debug.Log("isChasing State");
        }
        //else
        //{
        //   //agent.isStopped = true;
        //    Debug.Log("Chase complete");
        //    //WanderEvaluate();
        //}

    }

    private void InRange() //float range, Transform target, Transform origin
    {
        //searchRange0 = range;
        //playerTransform = target;
        //thisTransform = origin;

        float distance = Vector3.Distance(playerTransform.position, thisTransform.position);
        if (distance < chasingRange0)
        {
            Chase();
        }
        else if (distance > chasingRange0) { WanderAllOver(); }

    }



}
