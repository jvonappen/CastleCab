using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    [SerializeField] private float _speedInput = 0;
    [SerializeField] private float _boostMultiplier = 2;
    [SerializeField] private Rigidbody _sphereRB;
    [SerializeField] private GameObject _wagon;
    [SerializeField] private ParticleSystem[] _dustTrail;
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

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _joint = _wagon.GetComponent<ConfigurableJoint>();
        _wagonRB = _wagon.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _grounded = false;

        //get speed input 
        _speedInput = _playerInput._accelerationInput > 0 ? _forwardAcceleration : _reverseAcceleration;
        _speedInput *= _playerInput._accelerationInput;
        //boost
        if (_playerInput._boost != 0) _speedInput *= _boostMultiplier;
        else _speedInput *= 1;

        //Adjust wagon movement for reversing
        //_joint.angularYMotion = _playerInput._accelerationInput < 0 ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;
        //_joint.angularXMotion = _playerInput._accelerationInput < 0 ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;

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
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _playerInput._steeringInput * _turnStrength * Time.deltaTime * _playerInput._accelerationInput, 0f));

            //play particles
            if (_playerInput._accelerationInput > 0 && _grounded) PlayDustParticles();
            else StopDustParticles();
        }
        else//add gravity when in air
        {
            StopDustParticles();

            _sphereRB.drag = 0.0f;
            _sphereRB.AddForce(Vector3.up * -_gravityForce * 100f);
        }

        //tailwhips
        if (_playerInput._tailWhip > 0 && _playerInput._steeringInput > 0 && _grounded)
        {
            TailWhip(-_wagon.transform.right, _tailWhipPositions[0].position);
        }
        if (_playerInput._tailWhip > 0 && _playerInput._steeringInput < 0 && _grounded)
        {
            TailWhip(_wagon.transform.right, _tailWhipPositions[1].position);
        }

        //control in air
        float angle = Vector3.Angle(transform.up, Vector3.up);
        if (angle > maxTippingAngle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up), Mathf.InverseLerp(angle, 0, maxTippingAngle));
        }
    }

    private void PlayDustParticles()
    {
        for (int i = 0; i < _dustTrail.Length; i++)
        {
            _dustTrail[i].Play();
        }
    }

    private void StopDustParticles()
    {
        for (int i = 0; i < _dustTrail.Length; i++)
        {
            _dustTrail[i].Stop();
        }
    }

    private void TailWhip(Vector3 direction, Vector3 pos)
    {
        _wagonRB.AddForceAtPosition(direction * _tailWhipForce, pos, ForceMode.Impulse);
    }
}
