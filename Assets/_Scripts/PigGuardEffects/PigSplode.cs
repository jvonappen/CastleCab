using JigglePhysics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private Freeze _freezer;
    [SerializeField] private Material _matFlash;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private JiggleRigBuilder _jiggles;
    [SerializeField] private List<Material> _materials;
    [SerializeField] private Component[] _skinnedMeshRenderers;
    [SerializeField] private ParticleSystem _wham;
    [SerializeField] private float _force = 1000;
    [SerializeField] private float _playerForce = 500;
    [SerializeField] private float _upForce = 500;
    [SerializeField] private float _radius = 20;
    [SerializeField] private float _camShakeIntesity = 1;
    [SerializeField] private float _camShakeTime = 1;
    [SerializeField] private float _destroyTime = 3;
    [SerializeField] private Transform _whamPos;
    [SerializeField] private List<GameObject> _whams;
    [SerializeField] private bool _collisionOccured = false;
    [SerializeField] private float _resetTimer = 0.5f;
    private float _whamTimer = 0;
    [Space]
    [SerializeField] private int _goldRemoved = 5;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        _soundManager = FindObjectOfType<SoundManager>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _freezer = FindObjectOfType<Freeze>();
        _skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        _jiggles = GetComponent<JiggleRigBuilder>();
        _jiggles.enabled = false;
    }

    //Explode pig on impact with the player
    private void OnCollisionEnter(Collision other)
    {
        if (_collisionOccured) return;
        if (other.gameObject.name == "Wagon" || other.gameObject.name == "Donkey")
        {
            if (other.gameObject.GetComponent<PlayerMovement>() == null) return;
            PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            PlayerInput player = other.gameObject.GetComponent<PlayerInput>();

            if (player._accelerationInput > 0 && playerMovement._rigidbodySpeed > 1500 /*Increased to stop boost kills - Jacob*/ || Tailwhip(player, playerMovement))
            {
                _collisionOccured = true;
                _capsuleCollider.enabled = false;
                agent.enabled = false;
                rb.AddExplosionForce(_force, this.transform.position, _radius, _upForce);
                ParticleSystem bacon = Instantiate(_bacon, this.transform);
                ParticleSystem explode = Instantiate(_explode, this.transform);
                CameraShake.Instance.ShakeCamera(_camShakeIntesity, _camShakeTime);
                
                _freezer.Freezer();
                FlashDamage();

                _soundManager.Play("PigSqueal");
                _soundManager.Play("Splatter");
                if (!_freezer._isFrozen) _jiggles.enabled = true;

                // Police Dishonor Level Increase
                Dishonour.dishonourLevel = Dishonour.dishonourLevel + 40;

                GetComponent<PoliceAI>().enabled = false;
                Destroy(this.gameObject, _destroyTime);
            }
            else
            {
                //locate wham pos on player
                if (other.gameObject.name == "Donkey" && _whamPos == null)
                {
                    Transform whamPos = other.transform.Find("WhamPos");
                    _whamPos = whamPos;
                }

                //_soundManager.Play("PlayerHit");
                AudioManager.Instance.PlayGroupAudio("GuardPunch");
                ParticleSystem impact = Instantiate(_playerImpact, other.transform);
                other.rigidbody.AddForce((other.transform.position - this.transform.position) * _playerForce, ForceMode.Impulse);

                if(Dishonour.dishonourLevel > 0) //Only takes gold if there is a wanted level
                {
                    //Remove Gold from Player
                    DollarDisplay.dollarValue = DollarDisplay.dollarValue - _goldRemoved;
                    //Remove Dishonour Level p/Hit
                    Dishonour.dishonourLevel = Dishonour.dishonourLevel - _goldRemoved;
                }

                ParticleSystem wham = Instantiate(_wham, _whamPos);
                _whams.Add(wham.gameObject); //add to list to be destroyed
                _collisionOccured = true;
            }
        }
    }

    private void Update()
    {
        if (_whams.Count > 0) //destroy instantiated whams
        {
            foreach (var wham in _whams.ToList())
            {
                Destroy(wham, 1);
                if (wham == null) _whams.Remove(wham);
            }
        }

        //reset wham
        if (_collisionOccured)
        {
            _whamTimer -= Time.deltaTime;
            if (_whamTimer <= 0)
            {
                _collisionOccured = false;
                _capsuleCollider.enabled = true;
            }
        }
        if (_whamTimer <= 0) _whamTimer = _resetTimer;

    }

    private bool Tailwhip(PlayerInput player, PlayerMovement playerMovement)
    {
        return player._tailWhip > 0 && playerMovement._rigidbodySpeed > 5;
    }

    private void FlashDamage()
    {
        foreach (SkinnedMeshRenderer mesh in _skinnedMeshRenderers)
        {
            _materials.Add(mesh.material);
            mesh.material = _matFlash;
            
        }
        Invoke("ResetMaterial", _freezer._duration);
    }

    void ResetMaterial()
    {
        int i = 0;
        foreach(SkinnedMeshRenderer mesh in _skinnedMeshRenderers)
        {
            mesh.material = _materials[i];
            i++;
        }
    }

    
}
