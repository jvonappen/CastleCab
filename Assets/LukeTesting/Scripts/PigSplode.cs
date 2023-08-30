using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.AI;

public class PigSplode : MonoBehaviour
{
    private Rigidbody rb;
    private NavMeshAgent agent;
    [SerializeField] ParticleSystem _explode;
    [SerializeField] ParticleSystem _bacon;
    [SerializeField] private float _force = 1000;
    [SerializeField] private float _upForce = 500;
    [SerializeField] private float radius = 2;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Wagon" || other.gameObject.name == "Donkey")
        {
            agent.enabled = false;
            rb.AddExplosionForce(_force, this.transform.position, radius, _upForce);
            ParticleSystem bacon = Instantiate(_bacon, this.transform);
            ParticleSystem explode = Instantiate(_explode, this.transform);
            
            GetComponent<PoliceAI>().enabled = false;
            Destroy(this.gameObject, 5);
        }
    }
}
