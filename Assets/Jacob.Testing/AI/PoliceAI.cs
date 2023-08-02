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
    [SerializeField] private float awareRange = 5;
    [Tooltip("The speed the animal moves. If left at 0, the speed will default to the navMesh speed.")]
    [SerializeField] private float normalMovementSpeed = 3.5f;

    //chase, speed, wander
    private float startChasingRange = 0;
    private float startSpeed;
    private float startWanderRange;
    private float startAwareRange;
   
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

    [Header("UI Notification")]
    [Tooltip("Optional GUI to be displayed above the AI.")]
    [SerializeField] private bool usingGUI = true;
    [SerializeField] private GameObject alertUI;
    [SerializeField] private GameObject chaseUI;

    private NavMeshAgent agent;
    private Node topNode;



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {     
        SetMovementSpeed();
        BuildAnimalBehaviourTree();
        BaseValues();
    }



    private void BuildAnimalBehaviourTree() /* Tree works from bottom up */
    {
        //Wander
        WanderNode wanderNode = new WanderNode(wanderTransform, agent, wanderRange); //use 'transform' to set center on agent.

        //Alert and Chase
        ChaseNode chaseNode = new ChaseNode(playerTransform, agent);
        AlertNode awareNode = new AlertNode(agent, this, playerTransform);
        RangeNode chasingRangeNode = new RangeNode(startChasingRange, playerTransform, transform, chaseUI);
        RangeNode awareRangeNode = new RangeNode(awareRange, playerTransform, transform, alertUI);
        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode, });
        Sequence awareSequence = new Sequence(new List<Node> { awareRangeNode, awareNode });

        //Top Selector
        topNode = new Selector(new List<Node> { chaseSequence, awareSequence, wanderNode });
        //--------------------------------------------------------------------------------------------------------------
    }

    private void Update()
    {
        topNode.Evaluate();
        DishonourEvaluate();
    }

    private void SetMovementSpeed()
    {
        if (normalMovementSpeed > 0)
        {
            agent.speed = normalMovementSpeed;
        }
        else { normalMovementSpeed = agent.speed; }
    }

    private void DishonourEvaluate()
    {
        if(Dishonour.dishonourLevel > Dishonour._oneStar)
        {
            startChasingRange = chasingRange1;
            normalMovementSpeed = chaseSpeed1;
            wanderRange = searchRange1;
        }
        if (Dishonour.dishonourLevel > Dishonour._twoStar)
        {
            startChasingRange = chasingRange2;
            normalMovementSpeed = chaseSpeed2;
            wanderRange = searchRange2;
        }
        if (Dishonour.dishonourLevel > Dishonour._threeStar)
        {
            startChasingRange = chasingRange3;
            normalMovementSpeed = chaseSpeed3;
            awareRange = searchRange3;
        }
        if(Dishonour.dishonourLevel < Dishonour._oneStar)
        {
            startChasingRange = 0;
            normalMovementSpeed = startSpeed;
            wanderRange = startWanderRange;
        }
    }

    private void BaseValues()
    {
        startChasingRange = 0;
        startWanderRange = wanderRange;
        startSpeed = normalMovementSpeed;
    }
}
