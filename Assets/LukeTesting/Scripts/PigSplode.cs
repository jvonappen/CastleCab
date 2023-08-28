using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PigSplode : MonoBehaviour
{
    private Rigidbody rb;
    private NavMeshAgent agent;
    [SerializeField] ParticleSystem _explode;
    [SerializeField] private float _force = 20;
    [SerializeField] private float _upForce = 100;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Wagon")
        {
            Debug.Log("Launch");
            agent.enabled = false;
            rb.AddExplosionForce(_force, this.transform.position, 3, _upForce);
            Instantiate(_explode, transform.position, Quaternion.identity);
            GetComponent<PoliceAI>().enabled = false;
        }
    }
}
