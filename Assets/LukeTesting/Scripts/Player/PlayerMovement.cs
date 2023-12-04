using Cinemachine;
using DG.Tweening;
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
    [SerializeField] private GameObject _speedIntroParticles;
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
    [SerializeField] private GameObject _globalVolume;
    
    [Header("AUTO ASSIGNED VARIABLES")]
    [SerializeField] private CameraFOV _camera;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private Rigidbody _donkeyRB;
    [SerializeField] private Rigidbody _wagonRB;
    [SerializeField] private ConfigurableJoint _joint;
    [SerializeField] private CinemachineFreeLook _recenetering;
    [SerializeField] private Water _bubbles;
    [SerializeField] private BurnoutSlider _burnoutSlider;

    [Header("DRIVING VARIABLES")]
    [SerializeField] private float _speedInput = 0;
    [field: SerializeField] public float _rigidbodySpeed { get; private set; }
    [SerializeField] private float _forwardAcceleration = 500f;
    [SerializeField] private float _reverseAcceleration = 100f;
    [SerializeField] private float _onSpotAcceleration = 50f;
    [SerializeField] private float _wheelForwardRotation = 2f;
    [SerializeField] private float _wheelSpinModifier = 10f;
    [SerializeField] private float _wheelReverseRotation = -1f;
    private float _directionalAcceleration;
    
    [Header("TURN STRENGTH VARIABLES")]
    [SerializeField] private float _steeringTurnStrength;
    [SerializeField] private float _turnStrength = 120f;
    [SerializeField] private float _inAirTurnStrength = 270;
    [SerializeField] private float _tailWhipTurnStrength = 270;
    [SerializeField] private float _boostTurnStrength = 45;
    [SerializeField] private float _onSpotTurnStrength = 90;
    [SerializeField] private float _backflipTurnStrength = 430;
    [SerializeField] private float _barrelrollTurnStrength = 450;

    [Header("RIGIDBODY DRAG VARIABLES")]
    [SerializeField] private float _dragOnGround = 3f;
    [SerializeField] private float _dragOnAcceleration = 10f;
    [SerializeField] private float _dragOnStop = 1.5f;
    [SerializeField] private float _dragNormal = 3f;
    [SerializeField] private float _dragOnBurnoutRelease = 0;
    [SerializeField] private bool _dragSet = false;

    [Header("IN AIR VARIABLES")]
    [SerializeField] private float _gravityForce = 1.5f;
    [SerializeField] private float maxTippingAngle = 45f;
    [SerializeField] private float _groundRayLength = 2f;
    [SerializeField] private float _inAirRayLength = 10f;
    [field: SerializeField] public bool _grounded { get; private set; }
    [field: SerializeField] public bool _noMoreTricksGrounded { get; private set; }
    
    [Header("BURNOUT VARIABLES")]
    [SerializeField] private bool _burnout = false;
    [SerializeField] private bool _canBurnout = true;
    
    [Header("TAIL WHIP VARIABLES")]
    [SerializeField] private float _tailWhipForce = 10;
    [SerializeField] private bool _tailWhipRight = false;
    [SerializeField] private bool _tailWhipLeft = false;

    [Header("BOOST VARIABLES")]
    [SerializeField] private float _boostMultiplier = 2;
    private Coroutine _burnoutBoost;
    private const float NORMAL_FOV = 40f;
    private const float BOOST_FOV = 50f;

    //Aniamtion Variables
    [SerializeField] private float _animSpeed = 0;
    private bool _stopped = true;
    private string _currentState;
    private const string Horse_Idle = "Idle";
    private const string Horse_Run = "Run";
    private const string Horse_Stop = "Stop";
    private const string Horse_Reverse = "Reverse";
    private const string Horse_Blend_Tree = "Movement Blend Tree";

    private bool isInSlowdownZone = false;
    private bool hasBurst = false;
    [SerializeField] private bool _canBackflip = false;
    [SerializeField] private bool _backflipComplete = false;
    [SerializeField] private bool _barrelrollComplete = false;
    [SerializeField] private bool _canBarrelroll = false;
    [SerializeField] private float _backflipTimer = 0.8f;
    [SerializeField] private float _backflipReset = 0.8f;
    [SerializeField] private float _barrelRollReset = 0.8f;
    [SerializeField] private float _barrelrollTimer = 0.8f;
    [SerializeField] private float jumpPadForce = 1; // Adjust the force as needed

    public bool freeze  //freeze player for Jacob's dialogue system
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
        //Assign necessary variables
        _playerInput = GetComponent<PlayerInput>();
        _joint = _wagon.GetComponent<ConfigurableJoint>();
        _wagonRB = _wagon.GetComponent<Rigidbody>();
        _donkeyRB = this.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _burnoutSlider.ResetSlider();
    }

    private void Update()
    {
        //get and assign acceleration values for each driving state
        if (_playerInput._accelerationInput > 0 && _playerInput._reverseInput < 0 && _grounded) //burnout acceleration on ground
        {
            _speedInput = 0;
            _steeringTurnStrength = _turnStrength;
            if (_canBurnout && _rigidbodySpeed < 5) Burnout();
        }
        else if (_playerInput._accelerationInput > 0 && _grounded) //forward acceleration on ground
        {
            _speedInput = _playerInput._boost != 0 && BoostBar.canBoost ? _forwardAcceleration * _boostMultiplier : _forwardAcceleration * _playerInput._accelerationInput; //boost

            _animSpeed = Mathf.InverseLerp(_dragOnAcceleration + 20, _dragNormal, _dragOnGround) * _playerInput._accelerationInput; //slowly speed up animation
            Mathf.Clamp(_animSpeed, 0, 1);
            _horseAnimator.SetFloat("Speed", _animSpeed);

            if (_tailWhipLeft || _tailWhipRight)
            {
                if (_playerInput._boost == 0) _steeringTurnStrength = _tailWhipTurnStrength;
                else _steeringTurnStrength = _playerInput._boost != 0 ? _boostTurnStrength : _turnStrength;
            }
            else _steeringTurnStrength = _playerInput._boost != 0 ? _boostTurnStrength : _turnStrength; 
            ForwardAcceleration();
        }
        else if (_playerInput._reverseInput < 0 && _grounded) //reverse acceleration on ground
        {
            _burnoutSlider.ResetSlider();
            _speedInput = _reverseAcceleration;
            _speedInput *= _playerInput._reverseInput;
            _steeringTurnStrength = _turnStrength;
            ReverseAcceleration();
        }
        else if (_playerInput._accelerationInput == 0 && _playerInput._reverseInput == 0 && _playerInput._steeringInput != 0) //turning on spot acceleration
        {
            _burnoutSlider.ResetSlider();
            _speedInput = _onSpotAcceleration;
            _steeringTurnStrength = _onSpotTurnStrength;
            _horseAnimator.SetFloat("Speed", 0.5f);
            Boost(NORMAL_FOV, false); //turn off boost
        }
        else //no acceleration or in air
        {
            _burnoutSlider.ResetSlider();
            _speedInput = 0;
            _steeringTurnStrength = _inAirTurnStrength;
            _horseAnimator.SetFloat("Speed", 0f);
            NoAcceleration();
        }

        ReverseLockWagon(); //adjust wagon lock for reversing
        Debug.DrawRay(_groundRayPoint.position, -Vector3.up, Color.red); //DEBUG: for ground check
    }

    private void FixedUpdate()
    {
        _rigidbodySpeed = _sphereRB.velocity.magnitude; //DEBUG: display rigidbody speed in inspector

        GroundCheck(); //check if player is on gorund
        PlayerDragMovement(); //adds slow acceleration buildup and slow rolling stop
        //if (_backflipComplete || _barrelrollComplete) PlayerRotationCorrection(); //control tipping in air

        if (_grounded)
        {
            if (_playerInput._accelerationInput != 0 || _playerInput._reverseInput != 0) //physics controls on acceleration
            {
                AccelerationPhysics();
            }
            else if (_playerInput._steeringInput != 0) //physics controls when turning on spot
            {
                TurnOnSpotPhysics();
            }
            ResetTrickTimers();
        }
        else //physics controls in air
        {
            InAirPhysics();
            AirTimeGroundCheck();

            //tricks
            if (_playerInput._backflip != 0 && _backflipComplete == false && !_canBarrelroll && !_noMoreTricksGrounded) _canBackflip = true;
            if (_canBackflip) Backflip();
            if (_playerInput._barrelRoll != 0 && _barrelrollComplete == false && !_canBackflip && !_noMoreTricksGrounded) _canBarrelroll = true;
            if (_canBarrelroll) BarrelRoll();
        }

        //tailwhips
        CanTailWhip();
        if (_tailWhipRight) TailWhip(-_wagon.transform.right, _tailWhipPositions[0].position); //tailwhip right
        else if (_tailWhipLeft) TailWhip(_wagon.transform.right, _tailWhipPositions[1].position); //tailwhip left
        else
        {
            StopParticles(_tailWhipParticles);
            _soundManager.Fade("TailWhip");
        }
    }

    private void ForwardAcceleration()
    {
        if (_playerInput._reverseInput < 0) return; // kill forward momentum on reverse
        
        if (_burnout) //post burnout process
        {
            if (_canBurnout) _dragOnBurnoutRelease = _dragOnGround; //check what drag value is at on release of burnout
            _canBurnout = false;
            RotateWheels(_wheelForwardRotation);
            _burnoutSlider.ResetSlider();

            _burnoutBoost = StartCoroutine(Takeoff());
            if (_dragOnBurnoutRelease <= 3) //boost out of burnout if drag is cooked to 3
            {
                if (_steeringTurnStrength != _boostTurnStrength) _steeringTurnStrength = _boostTurnStrength;
                _speedInput = _forwardAcceleration * _boostMultiplier;
                Boost(BOOST_FOV, true);
            }
        }
        else //normal acceleration
        {
            _recenetering.m_RecenterToTargetHeading.m_enabled = true;
            if (!_stopped) _stopped = true;

            //kill effects
            StopParticles(_burnoutParticles);
            StopParticles(_chargedBurnoutParticles);

            //start effects
            if (_playerInput._boost != 0 && BoostBar.canBoost)
            {
                Boost(BOOST_FOV, true);
            }
            else Boost(NORMAL_FOV, false);
            _soundManager.Play("DonkeyTrott");
            _soundManager.Play("Wagon");
            RotateWheels(_rigidbodySpeed / _wheelSpinModifier);
            if (!IsAnimationPlaying(_horseAnimator, Horse_Stop)) ChangeAnimatorState(Horse_Blend_Tree);//ChangeAnimatorState(Horse_Run);
            if (!_bubbles._underWater) PlayParticles(_dustTrail);
            else StopParticles(_dustTrail);
            PlayTrail(_wheelTrail, true);
        }
    }

    private void ReverseAcceleration()
    {
        _burnout = false;
        if (!_stopped) _stopped = true;

        //kill effects
        if (!_burnout)_soundManager.Stop("Burnout");
        StopParticles(_burnoutParticles);
        StopParticles(_chargedBurnoutParticles);

        //start effects
        RotateWheels(_wheelReverseRotation);
        if (!IsAnimationPlaying(_horseAnimator, Horse_Stop)) ChangeAnimatorState(Horse_Reverse);
    }

    private void NoAcceleration()
    {
        _burnout = false;
        _recenetering.m_RecenterToTargetHeading.m_enabled = false;
        if (_canBurnout) Boost(NORMAL_FOV, false);

        //kill effects 
        if (_burnoutBoost != null) //post burnout process
        {
            StopCoroutine(Takeoff());
            if (_canBurnout) Boost(NORMAL_FOV, false);
            _burnout = false;
            _canBurnout = true;
            _burnoutBoost = null;
            _soundManager.Fade("Burnout");
        }

        if (!_burnout) _soundManager.Fade("Burnout");
        RotateWheels(_rigidbodySpeed * 0.1f); //slow wheel rotaion by rigidbody speed when not accelerating

        //play stopping animation once then change to idle when finished(speed dependent)
        if (_stopped && _rigidbodySpeed > 10)
        {
            ChangeAnimatorState(Horse_Stop);
            _stopped = false;
        }
        //else ChangeAnimatorState(Horse_Blend_Tree);
        else if (!IsAnimationPlaying(_horseAnimator, Horse_Stop) /*&& _playerInput._steeringInput == 0*/) ChangeAnimatorState(Horse_Blend_Tree);

        //kill effects
        StopParticles(_dustTrail);
        StopParticles(_burnoutParticles);
        StopParticles(_chargedBurnoutParticles);
        _soundManager.Stop("DonkeyTrott");
        _soundManager.Stop("Wagon");
    }

    IEnumerator Takeoff()
    {
        //kill effects
        StopParticles(_chargedBurnoutParticles);
        _soundManager.Fade("Burnout");
        if (_playerInput._accelerationInput == 0) yield break;

        //reset effects and values after a second
        yield return new WaitForSeconds(1f);
        _burnoutBoost = null;
        //Boost(NORMAL_FOV, false); //turn off boost
        _burnout = false;
        _canBurnout = true;
    }

    private void Burnout()
    {
        if (!_burnout) _dragOnGround = _dragOnAcceleration;
        _burnout = true;
        _burnoutSlider.BurnoutCharge(_dragOnGround);

        //kill effects
        StopParticles(_speedParticles);
        if (!_stopped) _stopped = true;
        _soundManager.Play("Burnout");
        RotateWheels(_wheelForwardRotation);
        _horseAnimator.SetFloat("Speed", 0.5f);
        ChangeAnimatorState(Horse_Blend_Tree);
        PlayParticles(_burnoutParticles);
        if (_dragOnGround <= 3) PlayParticles(_chargedBurnoutParticles);
    }

    private void AccelerationPhysics()
    {
        _sphereRB.drag = _dragOnGround;
        _sphereRB.AddForce(transform.forward * _speedInput);

        if (_playerInput._accelerationInput > 0 ) _directionalAcceleration = _playerInput._accelerationInput;
        else if (_playerInput._reverseInput < 0) _directionalAcceleration = _playerInput._reverseInput;

        //steering
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _playerInput._steeringInput * _steeringTurnStrength * Time.deltaTime * _directionalAcceleration, 0f));
    }

    private void TurnOnSpotPhysics()
    {
        _sphereRB.AddForce(transform.forward * _speedInput);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _playerInput._steeringInput * _steeringTurnStrength * Time.deltaTime, 0f));
        Boost(NORMAL_FOV, false); //turn off boost
    }

    private void InAirPhysics()
    {
        _recenetering.m_RecenterToTargetHeading.m_enabled = false;
        //disable particles and audio in air
        StopParticles(_dustTrail);
        PlayTrail(_wheelTrail, false);
        _soundManager.Stop("DonkeyTrott");
        _soundManager.Stop("Wagon");
        Boost(NORMAL_FOV, false); //turn off boost
        _steeringTurnStrength = _inAirTurnStrength;

        //apply gravity
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _playerInput._steeringInput * _steeringTurnStrength * Time.deltaTime, 0f));

        _sphereRB.drag = 0.0f;
        _sphereRB.AddForce(Vector3.up * -_gravityForce * 100f);
    }

    private void Backflip()
    {
        _backflipTimer -= Time.deltaTime;
        if (_canBackflip)
        {
            transform.Rotate(-_backflipTurnStrength * Time.deltaTime, 0, 0, Space.Self);
            if (_backflipTimer <= 0)
            {
                _backflipComplete = true;
                //add points to system
                _canBackflip = false;
            }
        }
    }

    private void BarrelRoll()
    {
        _barrelrollTimer -= Time.deltaTime;
        if (_canBarrelroll)
        {
            transform.Rotate(0, 0, -_barrelrollTurnStrength * Time.deltaTime, Space.Self);
            if (_barrelrollTimer <= 0)
            {
                _barrelrollComplete = true;
                //add points to system
                _canBarrelroll = false;
            }
        }
    }
    private void ResetTrickTimers()
    {
        if (_backflipTimer != _backflipReset) _backflipTimer = _backflipReset;
        _backflipComplete = false;
        _canBackflip = false;
        if (_barrelrollTimer != _barrelRollReset) _barrelrollTimer = _barrelRollReset;
        _barrelrollComplete = false;
        _canBarrelroll = false;
    }
    public void Boost(float camFOV, bool particlesVal)
    {
        if (_camera != null) _camera.SetCameraFov(camFOV);

        SoftJointLimit limit = new SoftJointLimit();

        if (!_canBurnout || _playerInput._boost != 0 && _grounded && _playerInput._accelerationInput > 0 && particlesVal && BoostBar.canBoost)
        {
            PlayParticles(_speedParticles);
            PlayParticles(_boostTrail);
            _soundManager.Play("Boost");
            _globalVolume.SetActive(true); //set motion blur

            //tighten wagon movement on boost
            limit.limit = 5f;
            _joint.angularYLimit = limit;
        }
        else
        {
            StopParticles(_speedParticles);
            StopParticles(_boostTrail);
            _soundManager.Stop("Boost");
            _globalVolume.SetActive(false);

            //allow wagon wiggle 
            limit.limit = 45f;
            _joint.angularYLimit = limit;
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
                if (_bubbles._underWater) trail[i].SetActive(false);
                else trail[i].SetActive(value);
            }
        }
    }

    private void CanTailWhip()
    {
        if (_playerInput._tailWhip > 0 && _grounded && _playerInput._accelerationInput > 0.5 && !_burnout)
        {
            if (_playerInput._steeringInput > 0.2)
            {
                _tailWhipRight = true;
            }
            else if (_playerInput._steeringInput < -0.2)
            {
                _tailWhipLeft = true;
            }
            else
            {
                _tailWhipRight = false;
                _tailWhipLeft = false;
            }
        }
        else
        {
            _tailWhipRight = false;
            _tailWhipLeft = false;
        }
    }

    private void TailWhip(Vector3 direction, Vector3 pos)
    {
        _wagonRB.AddForceAtPosition(direction * _tailWhipForce, pos, ForceMode.Impulse);
        PlayParticles(_tailWhipParticles);
        _soundManager.Play("TailWhip");
    }

    private void ReverseLockWagon()
    {
        _joint.angularYMotion = _playerInput._reverseInput < 0 && !_burnout ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;
        _joint.angularXMotion = _playerInput._reverseInput < 0 && !_burnout ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;
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
        else
        {
            return false;
        }
    }

    private void PlayerDragMovement()
    {
        if (!_grounded) return;
        if (_playerInput._accelerationInput > 0) //going forward
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
        else //releasing accelerator
        {
            if (_rigidbodySpeed < 4 && !_dragSet) _sphereRB.velocity *= 0.85f; //make adjustable value
            else _dragOnGround = _dragOnStop;
            _dragSet = false;
        }
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(_groundRayPoint.position, Vector3.down, out hit, _groundRayLength, _whatIsGround))
        {
            _grounded = true;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, Time.fixedDeltaTime * 10.0f);
        }
        else { _grounded = false; }
    }

    private void AirTimeGroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _inAirRayLength, _whatIsGround))
        {
            _noMoreTricksGrounded = true;
        }
        else { _noMoreTricksGrounded = false; }
    }

    private void PlayerRotationCorrection()
    {
        float angle = Vector3.Angle(transform.up, Vector3.up);
        if (angle > maxTippingAngle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up), Mathf.InverseLerp(angle, 0, maxTippingAngle));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SpeedRamps")
        {
            isInSlowdownZone = true;
            SpeedUpPlayer();
        }
        else if (other.gameObject.tag == "Mud")
        {
            SlowDownPlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "SpeedRamps")
        {
            isInSlowdownZone = false;
            RestoreOriginalSpeed();
        }
        else if (other.gameObject.tag == "Mud")
        {
            RestoreOriginalSpeed();
        }
    }

    private void SpeedUpPlayer()
    {
        _forwardAcceleration = 1500;
    }

    private void RestoreOriginalSpeed()
    {
        _forwardAcceleration = 500;
    }

    private void SlowDownPlayer()
    {
        _forwardAcceleration = 350;
    }
}
