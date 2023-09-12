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
    [SerializeField] private float _force = 1000;
    [SerializeField] private float _playerForce = 500;
    [SerializeField] private float _upForce = 500;
    [SerializeField] private float _radius = 20;
    [SerializeField] private float _camShakeIntesity = 1;
    [SerializeField] private float _camShakeTime = 1;

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
            if (other.gameObject.GetComponent<PlayerMovement>() == null) return; // change to mario kart movement or warthog movement for other scenes
            PlayerInput player = other.gameObject.GetComponent<PlayerInput>();

            if (player._tailWhip > 0 || player._boost > 0)
            {
                agent.enabled = false;
                

                rb.AddExplosionForce(_force, this.transform.position, _radius, _upForce);
                ParticleSystem bacon = Instantiate(_bacon, this.transform);
                ParticleSystem explode = Instantiate(_explode, this.transform);
                CameraShake.Instance.ShakeCamera(_camShakeIntesity, _camShakeTime); //Issue caused the remaing code not to execute. Probably Cams not hooked up right - Jacob...  "Nah" - Luke

                //FIND AUDIO CLIPS
                _soundManager.Play("PigSqueal");
                _soundManager.Play("Splatter");

                GetComponent<PoliceAI>().enabled = false;
                Destroy(this.gameObject, 5);
            }
            else
            {
                _soundManager.Play("PlayerHit");
                other.rigidbody.AddForce((other.transform.position - this.transform.position) * _playerForce, ForceMode.Impulse);
            }
        }
    }
}
