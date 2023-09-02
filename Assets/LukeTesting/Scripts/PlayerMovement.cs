using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private float _speedInput = 0;
    [SerializeField] private Rigidbody _sphereRB;
    [SerializeField] private GameObject _wagon;
    [SerializeField] private GameObject[] _dustTrail;
    [SerializeField] private GameObject[] _boostTrail;
    [SerializeField] private GameObject[] _wheelTrail;
    [SerializeField] private GameObject[] _tailWhipParticles;
    [SerializeField] private float _forwardAcceleration = 500f;
    [SerializeField] private float _reverseAcceleration = 100f;
    [SerializeField] private float _turnStrength = 180f;
    [SerializeField] private float _gravityForce = 1.5f;
    [SerializeField] private float _dragOnGround = 2f;
    [SerializeField] private float maxTippingAngle = 45f;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Transform _groundRayPoint;
    [SerializeField] private float _groundRayLength = 2f;
    [SerializeField] private bool _grounded;
    [SerializeField] private ConfigurableJoint _joint;
    [SerializeField] private Rigidbody _wagonRB;
    [SerializeField] private float _tailWhipForce = 10;
    [SerializeField] private Transform[] _tailWhipPositions;
    [SerializeField] private Rigidbody _donkeyRB;
    private float _steeringTurnStrength;

    //boost
    [SerializeField] private float _boostMultiplier = 2;
    [SerializeField] private float _boostTurnStrength = 45;
    [SerializeField] private GameObject[] _speedParticles;
    [SerializeField] private CameraFOV _camera;
    private const float NORMAL_FOV = 40f;
    private const float BOOST_FOV = 50f;

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
    }

    private void Update()
    { 
        //get speed input 
        _speedInput = _playerInput._accelerationInput > 0 ? _forwardAcceleration : _reverseAcceleration;
        _speedInput *= _playerInput._accelerationInput;

        //play donkey and cart audio
        if (_playerInput._accelerationInput > 0)
        {
            _soundManager.Play("DonkeyTrott");
            _soundManager.Play("Wagon");

            //boost player speed and effects
            if (_playerInput._boost != 0 && _grounded) Boost(_boostMultiplier, BOOST_FOV, true, _boostTurnStrength);
            else Boost(1, NORMAL_FOV, false, _turnStrength);
        }
        else
        {
            Boost(1, NORMAL_FOV, false, _turnStrength);
            _soundManager.Stop("DonkeyTrott");
            _soundManager.Stop("Wagon");
        }

        //adjust wagon movement for reversing
        ReverseLockWagon();

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
            _sphereRB.drag = _dragOnGround;
            _sphereRB.AddForce(transform.forward * _speedInput);

            //steering
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _playerInput._steeringInput * _steeringTurnStrength * Time.deltaTime * _playerInput._accelerationInput, 0f));

            //play particles when moving on the ground
            if (_playerInput._accelerationInput > 0 && _grounded)
            {
                PlayParticles(_dustTrail, true);
                PlayParticles(_wheelTrail, true);
            }
            else
            {
                PlayParticles(_dustTrail, false);
                PlayParticles(_wheelTrail, false);
            }
        }
        else//add gravity when in air
        {
            //disable particles in air
            PlayParticles(_dustTrail, false);
            PlayParticles(_wheelTrail, false);
            _soundManager.Stop("DonkeyTrott");
            _soundManager.Stop("Wagon");

            _sphereRB.drag = 0.0f;
            _sphereRB.AddForce(Vector3.up * -_gravityForce * 100f);
        }

        //tailwhips
        if (CanTailWhip(1)) TailWhip(-_wagon.transform.right, _tailWhipPositions[0].position);
        //else PlayParticles(_tailWhipParticles, false);
        if (CanTailWhip(-1)) TailWhip(_wagon.transform.right, _tailWhipPositions[1].position);
        //else PlayParticles(_tailWhipParticles, false);

        //control in air
        float angle = Vector3.Angle(transform.up, Vector3.up);
        if (angle > maxTippingAngle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up), Mathf.InverseLerp(angle, 0, maxTippingAngle));
        }
    }

    private void Boost(float boostMultiplier, float camFOV, bool particlesVal, float turnStrength)
    {
        _speedInput *= boostMultiplier;
        _steeringTurnStrength = turnStrength;
        if (_camera != null) _camera.SetCameraFov(camFOV);
        PlayParticles(_speedParticles, particlesVal);
        PlayParticles(_boostTrail, particlesVal);
    }

    private void PlayParticles(GameObject[] particles, bool value)
    {
        if (particles != null)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].SetActive(value);
            }
        }
    }

    private bool CanTailWhip(float direction)
    {
        return _playerInput._tailWhip > 0 && _playerInput._steeringInput == direction && _grounded && _playerInput._accelerationInput > 0.5;
    }

    private void TailWhip(Vector3 direction, Vector3 pos)
    {
        _wagonRB.AddForceAtPosition(direction * _tailWhipForce, pos, ForceMode.Impulse);

        //CREATE PARTICLES FOR TAILWHIP
        //PlayParticles(_tailWhipParticles, true);
    }

    private void ReverseLockWagon()
    {
        _joint.angularYMotion = _playerInput._accelerationInput < 0 ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;
        _joint.angularXMotion = _playerInput._accelerationInput < 0 ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;
    }
}
