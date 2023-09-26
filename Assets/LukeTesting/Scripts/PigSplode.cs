using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.AI;

public class PigSplode : MonoBehaviour
{
    private Rigidbody rb;
    private NavMeshAgent agent;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] ParticleSystem _explode;
    [SerializeField] ParticleSystem _bacon;
    [SerializeField] ParticleSystem _playerImpact;
    [SerializeField] private float _force = 1000;
    [SerializeField] private float _playerForce = 500;
    [SerializeField] private float _upForce = 500;
    [SerializeField] private float _radius = 20;
    [SerializeField] private float _camShakeIntesity = 1;
    [SerializeField] private float _camShakeTime = 1;
    [SerializeField] private float _destroyTime = 3;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        _soundManager = FindObjectOfType<SoundManager>();
    }

    //Explode pig on impact with the player
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Wagon" || other.gameObject.name == "Donkey")
        {
            if (other.gameObject.GetComponent<PlayerMovement>() == null) return;
            PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            PlayerInput player = other.gameObject.GetComponent<PlayerInput>();

            if (player._accelerationInput > 0 && playerMovement._rigidbodySpeed > 15 || Tailwhip(player, playerMovement))
            {
                agent.enabled = false;

                rb.AddExplosionForce(_force, this.transform.position, _radius, _upForce);
                ParticleSystem bacon = Instantiate(_bacon, this.transform);
                ParticleSystem explode = Instantiate(_explode, this.transform);
                CameraShake.Instance.ShakeCamera(_camShakeIntesity, _camShakeTime); 

                _soundManager.Play("PigSqueal");
                _soundManager.Play("Splatter");

                GetComponent<PoliceAI>().enabled = false;
                Destroy(this.gameObject, _destroyTime);
            }
            else
            {
                _soundManager.Play("PlayerHit");
                ParticleSystem impact = Instantiate(_playerImpact, other.transform);
                other.rigidbody.AddForce((other.transform.position - this.transform.position) * _playerForce, ForceMode.Impulse);
            }
        }
    }

    private bool Tailwhip(PlayerInput player, PlayerMovement playerMovement)
    {
        return player._tailWhip > 0 && playerMovement._rigidbodySpeed > 5;
    }
}
