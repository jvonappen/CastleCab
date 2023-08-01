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

    //[Header("Dishonour ")]
    //[Tooltip("Both starting and Max value.")]
    //[SerializeField] private float startingDishonour = 100;
    //[Tooltip("The value at which the AI deems it important to replenish this need.")]
    //[SerializeField] private float minHungerThreshold = 40;
    //[Tooltip("The value the AI will complete refilling this need.")]
    //[SerializeField] private float maxHungerThreshold = 100;
    //[Tooltip("The rate this need will deplete over time.")]
    //[SerializeField] private float hungerDepletionRate = 0.5f;
    //[Tooltip("Rate at which this need will replenish.")]
    //[SerializeField] private float hungerReplenishmentRate = 5;
    //[Tooltip("List of spots the AI can find food to refill Hunger need.")]
    //[SerializeField] private TargetSpots[] availableFoodSpots;

    //[Header("Alert")]
    [Tooltip("Distance the AI will chase away the player.")]
    [SerializeField] private float chasingRange = 3;
    [Tooltip("Distance the AI will become alert to the player.")]
    [SerializeField] private float awareRange = 5;
    //[Tooltip("The Player")]
    [SerializeField] private Transform playerTransform;

    //[Header("Wander")]
    [Tooltip("The center of the wandering radius. Use a fixed object to keep the wander area fixed. Use the animal itself, to have complete free roam.")]
    [SerializeField] private Transform wanderTransform;
    [Tooltip("How far the AI will wander from the wander tranform.")]
    [SerializeField] private float wanderRange = 25;

    [Header("UI Notification")]
    [Tooltip("Optional GUI to be displayed above the AI.")]
    [SerializeField] private bool usingGUI = true;
    [SerializeField] private GameObject alertUI;
    [SerializeField] private GameObject chaseUI;

    private NavMeshAgent agent;
    private Node topNode;

    [Tooltip("The speed the animal moves. If left at 0, the speed will default to the navMesh speed.")]
    [SerializeField] private float normalMovementSpeed = 3.5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        
        SetMovementSpeed();
        BuildAnimalBehaviourTree();
        //ResetChecks();
    }



    private void BuildAnimalBehaviourTree() /* Tree works from bottom up */
    {
        //Wander
        WanderNode wanderNode = new WanderNode(wanderTransform, agent, wanderRange); //use 'transform' to set center on agent.

        //Alert and Chase
        ChaseNode chaseNode = new ChaseNode(playerTransform, agent);
        AlertNode awareNode = new AlertNode(agent, this, playerTransform);
        RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTransform, transform, chaseUI);
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
    }

    private void SetMovementSpeed()
    {
        if (normalMovementSpeed > 0)
        {
            agent.speed = normalMovementSpeed;
        }
        else { normalMovementSpeed = agent.speed; }

    }
}
