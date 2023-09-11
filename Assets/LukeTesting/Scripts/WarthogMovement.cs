using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarthogMovement : MonoBehaviour
{
    private WarthogInput _playerInput;

    [Header("PARTICLES")]
    [SerializeField] private ParticleSystem[] _dustTrail;
    [SerializeField] private ParticleSystem[] _boostTrail;
    [SerializeField] private GameObject[] _wheelTrail;
    [SerializeField] private ParticleSystem[] _speedParticles;

    [Header("ASSIGNABLE VARIABLES")]
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Transform _groundRayPoint;
    [SerializeField] private Rigidbody _sphereRB;
    [SerializeField] private GameObject _wagon;
    [SerializeField] private Transform[] _wheels;
    [SerializeField] private Animator _horseAnimator;
    [SerializeField] private Transform _mainCamera;

    [Header("AUTO ASSIGNED VARIABLES")]
    [SerializeField] private CameraFOV _camera;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private Rigidbody _donkeyRB;
    [SerializeField] private Rigidbody _wagonRB;
    [SerializeField] private ConfigurableJoint _joint;

    [Header("DRIVING VARIABLES")]
    [SerializeField] private float _speedInput = 0;
    [SerializeField] private float _forwardAcceleration = 500f;
    [SerializeField] private float _reverseAcceleration = 100f;
    [SerializeField] private float _turnStrength = 180f;
    [SerializeField] private float _gravityForce = 1.5f;
    [SerializeField] private float _dragOnGround = 3f;
    [SerializeField] private float _dragOnAcceleration = 10f;
    [SerializeField] private float _dragOnStop = 1.5f;
    [SerializeField] private float _dragNormal = 3f;
    [SerializeField] private bool _dragSet = false;
    [SerializeField] private float maxTippingAngle = 45f;
    [SerializeField] private float _groundRayLength = 2f;
    [SerializeField] private float _wheelForwardRotation = 4f;
    [SerializeField] private float _wheelBackRotation = -1f;
    [field: SerializeField] public bool _grounded { get; private set; }
    private float _steeringTurnStrength;

    [Header("BOOST VARIABLES")]
    [SerializeField] private float _boostMultiplier = 2;
    [SerializeField] private float _boostTurnStrength = 45;
    private const float NORMAL_FOV = 40f;
    private const float BOOST_FOV = 50f;

    //Aniamtion Variables
    private bool _stopped = true;
    private string _currentState;
    private const string Horse_Idle = "Idle";
    private const string Horse_Run = "Run";
    private const string Horse_Stop = "Stop";
    private const string Horse_Reverse = "Reverse";

    //freeze player for Jacob's dialogue system
    public bool freeze
    {
        get => _freeze;
        set
        {
            _freeze = value;
            _sphereRB.isKinematic = _freeze;
            _wagonRB.isKinematic = _freeze;
            _donkeyRB.isKinematic = _freeze;
        }
    }

    private bool _freeze;

    private void Awake()
    {
        _playerInput = GetComponent<WarthogInput>();
        _joint = _wagon.GetComponent<ConfigurableJoint>();
        _wagonRB = _wagon.GetComponent<Rigidbody>();
        _donkeyRB = this.GetComponent<Rigidbody>();
        _soundManager = FindObjectOfType<SoundManager>();
        _camera = FindObjectOfType<CameraFOV>();
    }

    private void Update()
    {
        //get speed input 
        _speedInput = _playerInput._accelerationInput > 0 ? _forwardAcceleration : _reverseAcceleration;
        _speedInput *= _playerInput._accelerationInput;

        //forward movement
        if (_playerInput._accelerationInput > 0 && _grounded)
        {
            if (!_stopped) _stopped = true;
            _soundManager.Play("DonkeyTrott");
            _soundManager.Play("Wagon");
            RotateWheels(_wheelForwardRotation);
            ChangeAnimatorState(Horse_Run);
            PlayParticles(_dustTrail);
            PlayTrail(_wheelTrail, true);

            //boost player speed and effects
            if (_playerInput._boost != 0 /*&& _grounded*/) Boost(_boostMultiplier, BOOST_FOV, true, _boostTurnStrength);
            else Boost(1, NORMAL_FOV, false, _turnStrength);
        }
        //backwards movement
        else if (_playerInput._accelerationInput < 0 && _grounded)
        {
            if (!_stopped) _stopped = true;
            RotateWheels(_wheelBackRotation);
            ChangeAnimatorState(Horse_Reverse);
        }
        //no acceleration
        else
        {
            if (_stopped)
            {
                ChangeAnimatorState(Horse_Stop);
                _stopped = false;
            }
            else if (!IsAnimationPlaying(_horseAnimator, Horse_Stop)) ChangeAnimatorState(Horse_Idle);

            StopParticles(_dustTrail);
            Boost(1, NORMAL_FOV, false, _turnStrength);
            _soundManager.Stop("DonkeyTrott");
            _soundManager.Stop("Wagon");
        }

        ReverseLockWagon(); //adjust wagon movement for reversing

        Debug.DrawRay(_groundRayPoint.position, -Vector3.up, Color.red);
    }

    private void FixedUpdate()
    {
        _grounded = false;
        RaycastHit hit;

        if (Physics.Raycast(_groundRayPoint.position, Vector3.down, out hit, _groundRayLength, _whatIsGround))
        {
            _grounded = true;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, Time.fixedDeltaTime * 10.0f);
        }

        if (_grounded)//control car on ground
        {
            PlayerDragMovement2(); //adds slow acceleration buildup and rolling stop

            _sphereRB.drag = _dragOnGround;
            _sphereRB.AddForce(transform.forward * _speedInput);

            //steering
            if (_playerInput._accelerationInput > 0)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _mainCamera.rotation, _steeringTurnStrength * Time.deltaTime * _playerInput._accelerationInput);
            }
            if (_playerInput._accelerationInput < 0)
            {
                Debug.Log("Backwards");
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _mainCamera.rotation, _steeringTurnStrength * Time.deltaTime * _playerInput._accelerationInput * _playerInput._accelerationInput);
            }
        }
        else//add gravity when in air
        {
            //disable particles and audio in air
            StopParticles(_dustTrail);
            PlayTrail(_wheelTrail, false);
            _soundManager.Stop("DonkeyTrott");
            _soundManager.Stop("Wagon");

            //apply gravity
            _sphereRB.drag = 0.0f;
            _sphereRB.AddForce(Vector3.up * -_gravityForce * 100f);
        }

        //control tipping in air
        float angle = Vector3.Angle(transform.up, Vector3.up);
        if (angle > maxTippingAngle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up), Mathf.InverseLerp(angle, 0, maxTippingAngle));
        }
    }

    public void Boost(float boostMultiplier, float camFOV, bool particlesVal, float turnStrength)
    {
        _speedInput *= boostMultiplier;
        _steeringTurnStrength = turnStrength;
        if (_camera != null) _camera.SetCameraFov(camFOV);
        if (_playerInput._boost != 0 && _grounded && _playerInput._accelerationInput > 0)
        {
            PlayParticles(_speedParticles);
            PlayParticles(_boostTrail);
            _soundManager.Play("Boost");
        }
        else
        {
            StopParticles(_speedParticles);
            StopParticles(_boostTrail);
            _soundManager.Stop("Boost");
        }
    }

    private void PlayParticles(ParticleSystem[] particles)
    {
        if (particles != null)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                if (!particles[i].isEmitting)
                {
                    particles[i].Play();
                }
            }
        }
    }

    private void StopParticles(ParticleSystem[] particles)
    {
        if (particles != null)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].isEmitting)
                {
                    particles[i].Stop();
                }
            }
        }
    }

    private void PlayTrail(GameObject[] trail, bool value)
    {
        if (trail != null)
        {
            for (int i = 0; i < trail.Length; i++)
            {
                trail[i].SetActive(value);
            }
        }
    }

    private void ReverseLockWagon()
    {
        _joint.angularYMotion = _playerInput._accelerationInput < 0 ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;
        _joint.angularXMotion = _playerInput._accelerationInput < 0 ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;
    }

    private void RotateWheels(float turnSPeed)
    {
        for (int i = 0; i < _wheels.Length; i++)
        {
            _wheels[i].transform.Rotate(turnSPeed * 360 * Time.deltaTime, 0, 0);
        }
    }

    private void ChangeAnimatorState(string newState)
    {
        if (newState == _currentState) return;
        //_horseAnimator.Play(newState);
        _horseAnimator.CrossFade(newState, 0.2f, 0);
        _currentState = newState;
    }

    bool IsAnimationPlaying(Animator animator, string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f ||
            animator.IsInTransition(0))
        {
            return true;
        }
        else return false;
    }

    private void PlayerDragMovement2()
    {
        //adjust drag to add for rolling stop
        if (_playerInput._accelerationInput > 0)
        {
            if (!_dragSet)
            {
                _dragOnGround = _dragOnAcceleration;
                _dragSet = true;
            }
            if (_dragOnGround > _dragNormal)
            {
                _dragOnGround -= Time.deltaTime * 3;
            }
            else _dragOnGround = _dragNormal;
        }
        else
        {
            _dragOnGround = _dragOnStop;
            _dragSet = false;
        }
    }
}
