using Cinemachine;
using System;
using System.Collections;
//using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;

    [Header("PARTICLES")]
    [SerializeField] private ParticleSystem[] _dustTrail;
    [SerializeField] private ParticleSystem[] _boostTrail;
    [SerializeField] private GameObject[] _wheelTrail;
    [SerializeField] private ParticleSystem[] _tailWhipParticles;
    [SerializeField] private ParticleSystem[] _speedParticles;
    [SerializeField] private ParticleSystem[] _burnoutParticles;
    [SerializeField] private ParticleSystem[] _chargedBurnoutParticles;

    [Header("ASSIGNABLE VARIABLES")]
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Transform _groundRayPoint;
    [SerializeField] private Rigidbody _sphereRB;
    [SerializeField] private GameObject _wagon;
    [SerializeField] private Transform[] _tailWhipPositions;
    [SerializeField] private Transform[] _wheels;
    [SerializeField] private Animator _horseAnimator;
    [SerializeField] private CinemachineFreeLook _recenetering;
    
    [Header("AUTO ASSIGNED VARIABLES")]
    [SerializeField] private CameraFOV _camera;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private Rigidbody _donkeyRB;
    [SerializeField] private Rigidbody _wagonRB;
    [SerializeField] private ConfigurableJoint _joint;

    [Header("DRIVING VARIABLES")]
    [SerializeField] private float _speedInput = 0;
    [field: SerializeField] public float _rigidbodySpeed { get; private set; }
    [SerializeField] private float _forwardAcceleration = 500f;
    [SerializeField] private float _reverseAcceleration = 100f;
    [SerializeField] private float _turnStrength = 180f;
    [SerializeField] private float _inAirTurnStrength = 90;
    [SerializeField] private float _tailWhipTurnStrength = 270;
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
    [SerializeField] private float _steeringTurnStrength;
    private float _steeringAccel;
    private bool _burnout = false;
    private bool _canBurnout = true;
    [SerializeField] private bool _tailWhipping = false;
    [SerializeField] private float _dragOnBurnoutRelease = 0;

    [Header("TAIL WHIP VARIABLES")]
    [SerializeField] private float _tailWhipForce = 10;

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
        _playerInput = GetComponent<PlayerInput>();
        _joint = _wagon.GetComponent<ConfigurableJoint>();
        _wagonRB = _wagon.GetComponent<Rigidbody>();
        _donkeyRB = this.GetComponent<Rigidbody>();
        _soundManager = FindObjectOfType<SoundManager>();
        _camera = FindObjectOfType<CameraFOV>();
        _recenetering = FindObjectOfType<CinemachineFreeLook>();
    }

    private void Update()
    {
        //get speed input
        if (_playerInput._accelerationInput > 0 && _playerInput._reverseInput < 0) _speedInput = 0;
        else if (_playerInput._accelerationInput > 0)
        {
            _speedInput = _forwardAcceleration;
            _speedInput *= _playerInput._accelerationInput;
        }
        else if (_playerInput._reverseInput < 0)
        {
            _speedInput = _reverseAcceleration;
            _speedInput *= _playerInput._reverseInput;
        }
        else _speedInput = 0;

        //_speedInput = _playerInput._accelerationInput > 0 ? _forwardAcceleration : _reverseAcceleration;
        if (_playerInput._accelerationInput > 0 && _playerInput._reverseInput < 0 && _grounded && _canBurnout && _rigidbodySpeed < 5) Burnout();
        //forward acceleration
        else if (_playerInput._accelerationInput > 0 && _grounded) ForwardAcceleration();
        //backwards movement
        else if (_playerInput._reverseInput < 0 && _grounded) BackwardAcceleration();
        ////turn on spot
        //else if (_playerInput._accelerationInput == 0 && _playerInput._steeringInput > 0 || _playerInput._steeringInput < 0 && _grounded)
        //no acceleration
        else NoAcceleration();

        ReverseLockWagon(); //adjust wagon lock for reversing

        Debug.DrawRay(_groundRayPoint.position, -Vector3.up, Color.red);
    }

    private void FixedUpdate()
    {
        _grounded = false;
        RaycastHit hit;
        _rigidbodySpeed = _sphereRB.velocity.magnitude;

        if (Physics.Raycast(_groundRayPoint.position, Vector3.down, out hit, _groundRayLength, _whatIsGround))
        {
            _grounded = true;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, Time.fixedDeltaTime * 10.0f);
        }

        PlayerDragMovement2(); //adds slow acceleration buildup and rolling stop

        if (_grounded)
        {
            //control car on ground when moving forward or reverseing
            if (_playerInput._accelerationInput > 0 || _playerInput._reverseInput < 0) AccelerationPhysics();
            //turn player on spot with steering input
            else if (_playerInput._accelerationInput == 0 && _playerInput._reverseInput == 0 && _playerInput._steeringInput > 0 || _playerInput._steeringInput < 0) TurnOnSpotPhysics();
        }
        //control player in air and apply gravity
        else InAirPhysics();

        //tailwhips
        if (CanTailWhip(1)) TailWhip(-_wagon.transform.right, _tailWhipPositions[0].position);
        else if (CanTailWhip(-1)) TailWhip(_wagon.transform.right, _tailWhipPositions[1].position);
        else
        {
            _tailWhipping = false;
            StopParticles(_tailWhipParticles);
        }

        //control tipping in air
        float angle = Vector3.Angle(transform.up, Vector3.up);
        if (angle > maxTippingAngle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up), Mathf.InverseLerp(angle, 0, maxTippingAngle));
        }
    }

    private void ForwardAcceleration()
    {
        if (_playerInput._reverseInput < 0) return;
        //boost of post burnout
        if (_burnout)
        {
            if (_canBurnout) _dragOnBurnoutRelease = _dragOnGround; //check what drag value is at on release of burnout
            _canBurnout = false;
            
            StartCoroutine(Takeoff());
            if (_dragOnBurnoutRelease == 3) Boost(_boostMultiplier, BOOST_FOV, true, _boostTurnStrength);
        }
        else
        {
            _recenetering.m_RecenterToTargetHeading.m_enabled = true;
            if (!_stopped) _stopped = true;
            _soundManager.Play("DonkeyTrott");
            _soundManager.Play("Wagon");
            RotateWheels(_wheelForwardRotation);
            ChangeAnimatorState(Horse_Run);
            PlayParticles(_dustTrail);
            StopParticles(_burnoutParticles);
            StopParticles(_chargedBurnoutParticles);
            PlayTrail(_wheelTrail, true);

            //boost player speed and effects
            if (_playerInput._boost != 0 && !_tailWhipping)
            {
                Debug.Log("Boosting");
                Boost(_boostMultiplier, BOOST_FOV, true, _boostTurnStrength);
            }
            else if (_tailWhipping)
            {
                Debug.Log("Tail whipping");
                Boost(1, NORMAL_FOV, false, _tailWhipTurnStrength);
            }
            else
            {
                Debug.Log("Normal");
                Boost(1, NORMAL_FOV, false, _turnStrength);
            }
        }
    }

    private void BackwardAcceleration()
    {
        _burnout = false;
        if (!_stopped) _stopped = true;
        StopParticles(_burnoutParticles);
        StopParticles(_chargedBurnoutParticles);
        RotateWheels(_wheelBackRotation);
        if (!IsAnimationPlaying(_horseAnimator, Horse_Stop)) ChangeAnimatorState(Horse_Reverse);
    }

    private void NoAcceleration()
    {
        _burnout = false;
        _recenetering.m_RecenterToTargetHeading.m_enabled = false;
        RotateWheels(_rigidbodySpeed * 0.1f);

        if (_stopped && _rigidbodySpeed > 10)
        {
            ChangeAnimatorState(Horse_Stop);
            _stopped = false;
        }
        else if (!IsAnimationPlaying(_horseAnimator, Horse_Stop) && _playerInput._steeringInput == 0) ChangeAnimatorState(Horse_Idle);

        StopParticles(_dustTrail);
        StopParticles(_burnoutParticles);
        StopParticles(_chargedBurnoutParticles);
        Boost(1, NORMAL_FOV, false, _turnStrength);
        _soundManager.Stop("DonkeyTrott");
        _soundManager.Stop("Wagon");
    }

    IEnumerator Takeoff()
    {
        _soundManager.Fade("Burnout");
        yield return new WaitForSeconds(1f);
      
        Boost(1, NORMAL_FOV, false, _turnStrength);
        _burnout = false;
        _canBurnout = true;
    }

    private void Burnout()
    {
        if (!_burnout) _dragOnGround = _dragOnAcceleration;
        _burnout = true;
        StopParticles(_speedParticles);
        if (!_stopped) _stopped = true;
        _soundManager.Play("Burnout");
        RotateWheels(_wheelForwardRotation);
        ChangeAnimatorState(Horse_Run);
        PlayParticles(_burnoutParticles);
        if (_dragOnGround <= 3) PlayParticles(_chargedBurnoutParticles);
    }

    private void AccelerationPhysics()
    {
        _sphereRB.drag = _dragOnGround;
        _sphereRB.AddForce(transform.forward * _speedInput);

        if (_playerInput._accelerationInput > 0 ) _steeringAccel = _playerInput._accelerationInput;
        else if (_playerInput._reverseInput < 0) _steeringAccel = _playerInput._reverseInput;

        //steering
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _playerInput._steeringInput * _steeringTurnStrength * Time.deltaTime * _steeringAccel, 0f));
    }

    private void TurnOnSpotPhysics()
    {
        ChangeAnimatorState(Horse_Run);
        _sphereRB.AddForce(transform.forward * 50);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _playerInput._steeringInput * 90 * Time.deltaTime, 0f));
    }

    private void InAirPhysics()
    {
        _recenetering.m_RecenterToTargetHeading.m_enabled = false;
        //disable particles and audio in air
        StopParticles(_dustTrail);
        PlayTrail(_wheelTrail, false);
        _soundManager.Stop("DonkeyTrott");
        _soundManager.Stop("Wagon");
        Boost(1, NORMAL_FOV, false, _turnStrength);
        _steeringTurnStrength = _inAirTurnStrength;

        //apply gravity
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _playerInput._steeringInput * _steeringTurnStrength * Time.deltaTime, 0f));

        _sphereRB.drag = 0.0f;
        _sphereRB.AddForce(Vector3.up * -_gravityForce * 100f);
    }

    public void Boost(float boostMultiplier, float camFOV, bool particlesVal, float turnStrength)
    {
        _speedInput *= boostMultiplier;
        _steeringTurnStrength = turnStrength;
        if (_camera != null) _camera.SetCameraFov(camFOV);
        if (!_canBurnout || _playerInput._boost != 0 && _grounded && _playerInput._accelerationInput > 0)
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

    private bool CanTailWhip(float direction)
    {
        if (direction > 0)
        {
            return _playerInput._tailWhip > 0 && _playerInput._steeringInput > 0.2 && _grounded && _playerInput._accelerationInput > 0.5;
        }
        else if (direction < 0)
        {
            return _playerInput._tailWhip > 0 && _playerInput._steeringInput < -0.2 && _grounded && _playerInput._accelerationInput > 0.5;
        }
        else
        {
            return false;
        }
    }

    private void TailWhip(Vector3 direction, Vector3 pos)
    {
        _tailWhipping = true;
        _wagonRB.AddForceAtPosition(direction * _tailWhipForce, pos, ForceMode.Impulse);
        PlayParticles(_tailWhipParticles);
    }

    private void ReverseLockWagon()
    {
        if (_playerInput._accelerationInput > 0 && _playerInput._reverseInput < 0)
        {
            _joint.angularYMotion = ConfigurableJointMotion.Limited;
            _joint.angularXMotion = ConfigurableJointMotion.Limited;
        }
        else
        {
            _joint.angularYMotion = _playerInput._reverseInput < 0 ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;
            _joint.angularXMotion = _playerInput._reverseInput < 0 ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;
        }
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
        if (!_grounded) return;
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
            //Debug.Log("drag on stop");
            if (_rigidbodySpeed < 4 && !_dragSet) _sphereRB.velocity *= 0.85f; //make adjustable value
            else _dragOnGround = _dragOnStop;
            _dragSet = false;
        }
    }
}
