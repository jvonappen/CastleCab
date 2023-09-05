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
    [SerializeField] private Transform _playerTransform;
    //[SerializeField] private GameObject piggySiren;
    //[Tooltip("Distance the AI will look at the player.")]
    //[SerializeField] private float awareRange;

    [Header("Wander")]
    [Tooltip("The center of the wandering radius. Use a fixed object to keep the wander area fixed. Use the object itself, to have complete free roam.")]
    [SerializeField] private Transform wanderTransform;
    [Tooltip("How far the AI will wander from the wander tranform.")]
    [SerializeField] private float wanderRange = 25;
    [Tooltip("The normal speed the gaurd moves. If left at 0, the speed will default to the navMesh speed.")]
    [SerializeField] private float movementSpeed = 3.5f;

    //[Header("Dishonour Level One")]
    //[Tooltip("Distance the AI will chase away the player.")]
    //[SerializeField] private float chasingRange1 = 5;
    //[Tooltip("Speed modifier for a level one Dishonour Level.")]
    //[SerializeField] private float chaseSpeed1 = 5;
    //[Tooltip("Distance the AI will search for the player.")]
    //[SerializeField] private float searchRange1 = 5;

    //[Header("Dishonour Level Two")]
    //[Tooltip("Distance the AI will chase away the player.")]
    //[SerializeField] private float chasingRange2 = 10;
    //[Tooltip("Speed modifier for a level two Dishonour Level.")]
    //[SerializeField] private float chaseSpeed2 = 10;
    //[Tooltip("Distance the AI will search for the player.")]
    //[SerializeField] private float searchRange2 = 10;

    //[Header("Dishonour Level Three")]
    //[Tooltip("Distance the AI will chase away the player.")]
    //[SerializeField] private float chasingRange3 = 20;
    //[Tooltip("Speed modifier for a level three Dishonour Level.")]
    //[SerializeField] private float chaseSpeed3 = 15f;
    //[Tooltip("Distance the AI will search for the player.")]
    //[SerializeField] private float searchRange3 = 15;

    [SerializeField] private GuardChaseData data;

    private NavMeshAgent agent;
    private Transform thisTransform;

    [Header("Debug")]
    [SerializeField] private float chasingRange0; //how long for chase
    [SerializeField] private float chaseSpeed0; // speed
    [SerializeField] private float searchRange0; //aka wander
    [SerializeField] private bool sirenToggle;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        thisTransform = agent.transform;
    }
    private void Start()
    {
        _playerTransform = PlayerData.player.transform;
    }

    private void Update()
    {
        if(agent.enabled)
        {
            DishonourEvaluate();
        }      
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
    private void DishonourOne()    { chasingRange0 = data.chasingRange1; chaseSpeed0 = data.chaseSpeed1; searchRange0 = data.searchRange1; }
    private void DishonourTwo()    { chasingRange0 = data.chasingRange2; chaseSpeed0 = data.chaseSpeed2; searchRange0 = data.searchRange2; }
    private void DishonourThree()  { chasingRange0 = data.chasingRange3; chaseSpeed0 = data.chaseSpeed3; searchRange0 = data.searchRange3; }

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
    private void Chase()
    {
        float distance = Vector3.Distance(_playerTransform.position, agent.transform.position);
        if (distance < chasingRange0)
        {
            agent.isStopped = false;
            agent.SetDestination(_playerTransform.position);           
            DishonourIncrease();
            //Debug.Log("isChasing State");

        }
    }
    private void InRange()
    {
        float distance = Vector3.Distance(_playerTransform.position, thisTransform.position);
        if (distance < chasingRange0)
        {
            Chase();
            
        }
        if (distance > chasingRange0) { WanderAllOver(); }
    }

    private void DishonourIncrease()
    {
     Dishonour.dishonourLevel += Time.deltaTime * Dishonour._dishonourDepletionRef + 1;
    }

    //private void TurnOnSiren()
    //{
    //    if(playSiren == true) { piggySiren.SetActive(true); }
    //    else if(playSiren == false) { piggySiren.SetActive(!false); }
    //}
}
