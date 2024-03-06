using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    private int _index;
    private Vector3 _target;

    private NavMeshAgent agent;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        NextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, _target) < 1)
        {
            IterateIndex();
            NextWaypoint();
            
        }
        transform.LookAt(_target);
    }

    private void NextWaypoint()
    {
        _target = _waypoints[_index].position;
        agent.SetDestination(_target);
    }

    private void IterateIndex()
    {
        _index++;

        if(_index == _waypoints.Length) { _index = 0; }
    }
}
