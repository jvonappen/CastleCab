using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using Cinemachine;
using Cinemachine.PostFX;
using Cinemachine.PostFX.Editor;
using Cinemachine.Utility;
using UnityEngine.Rendering;

public class PoliceAI : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    [Header("Wander")]
    [Tooltip("The center of the wandering radius. Use a fixed object to keep the wander area fixed. Use the object itself, to have complete free roam.")]
    [SerializeField] private Transform wanderTransform;
    [Tooltip("How far the AI will wander from the wander tranform.")]
    [SerializeField] private float wanderRange = 25;
    [Tooltip("The normal speed the gaurd moves. If left at 0, the speed will default to the navMesh speed.")]
    [SerializeField] private float movementSpeed = 3.5f;

    [SerializeField] private GuardChaseData data;

    private NavMeshAgent agent;
    private Transform thisTransform;

    /* Cinemachine Volume Settings are stinky poo holes*/
    [Header("Arrest Behaviour Settings")]
    [SerializeField] private float increaseRate_ArrestVignette;
    [SerializeField] private float decreaseRate_ArrestVignette;
    [SerializeField] private Cinemachine.VcamTargetPropertyAttribute _vignetteSettings; 
    private float currentVignetteValue;
    public static bool vignetteHit = false;

    //[Header("Debug - Rotation")]
    //[SerializeField] private Transform _target;
    //[SerializeField] private float _rotateSpeed = 1f;
    //private Quaternion rotationGoal;
    //private Vector3 _rotateDirection;
    //private Coroutine _lookCoroutine;


    [Header("Debug")]
    [SerializeField] private float chasingRange0; //how long for chase
    [SerializeField] private float chaseSpeed0; // speed
    [SerializeField] private float searchRange0; //aka wander
    [SerializeField] private bool sirenToggle;
    [SerializeField] private bool inPursuit = false;

    

    private Transform playerTransform;
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
        if (agent.enabled)
        {
            DishonourEvaluate();
        }


    }


    private void DishonourEvaluate()
    {
        InRange();
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


        agent.speed = chaseSpeed0;
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

                transform.LookAt(point);

    
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
            Vector3 lookAtDonkey = new Vector3(_playerTransform.position.x, agent.transform.position.y, _playerTransform.position.z);
            agent.SetDestination(_playerTransform.position);

            agent.transform.LookAt(lookAtDonkey);
           
            DishonourIncrease();
            //Debug.Log("WeeWoo");
        }
    }
    private void InRange()
    {
        float distance = Vector3.Distance(_playerTransform.position, thisTransform.position);
        if (distance < chasingRange0)
        {
            Chase();
            inPursuit = true;
        }
        if (distance > chasingRange0) 
        {
            
            WanderAllOver(); 
            inPursuit = false; 

        }
    }

    private void DishonourIncrease()
    {
     Dishonour.dishonourLevel += Time.deltaTime * Dishonour._dishonourDepletionRef + 1;
    }

    private void VignetteHitEffect()
    {
        
    }

    //private void SmoothRotate()
    //{
    //    _rotateDirection = (_target.position - thisTransform.position).normalized;
    //    rotationGoal = Quaternion.LookRotation(_rotateDirection);
    //    thisTransform.rotation = Quaternion.Slerp(thisTransform.rotation, rotationGoal, _rotateSpeed);
    //}

    //private void SmoothRotate()
    //{
    //    if(_lookCoroutine != null)
    //    {
    //        StopCoroutine( _lookCoroutine );
    //    }
    //    _lookCoroutine = StartCoroutine(SmoothLookAt());
    //}

    //private IEnumerator SmoothLookAt()
    //{ 
    //    Quaternion lookRotation = Quaternion.LookRotation(_target.position - thisTransform.position);
    //    float time = 0;
    //    while(time < 1)
    //    {
    //        transform.rotation = Quaternion.Slerp(thisTransform.rotation, lookRotation, time);
    //        time += Time.deltaTime * _rotateSpeed;
    //        yield return null;
    //    }
    //}

}
