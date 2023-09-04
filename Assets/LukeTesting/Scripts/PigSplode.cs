using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.AI;

public class PigSplode : MonoBehaviour
{
    private Rigidbody rb;
    private NavMeshAgent agent;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] ParticleSystem _explode;
    [SerializeField] ParticleSystem _bacon;
    [SerializeField] private float _force = 1000;
    [SerializeField] private float _upForce = 500;
    [SerializeField] private float _radius = 2;
    [SerializeField] private float _camShakeIntesity = 1;
    [SerializeField] private float _camShakeTime = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    //Explode pig on impact with the player
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Wagon" || other.gameObject.name == "Donkey")
        {
            agent.enabled = false;
            rb.AddExplosionForce(_force, this.transform.position, _radius, _upForce);
            ParticleSystem bacon = Instantiate(_bacon, this.transform);
            ParticleSystem explode = Instantiate(_explode, this.transform);
            CameraShake.Instance.ShakeCamera(_camShakeIntesity, _camShakeTime);

            //FIND AUDIO CLIPS
            //_soundManager.Play("MeatSplatter");
            //_soundManager.Play("Explode");

            GetComponent<PoliceAI>().enabled = false;
            Destroy(this.gameObject, 5);
        }
    }
}
