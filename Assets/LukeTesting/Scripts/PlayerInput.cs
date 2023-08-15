using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerInput : MonoBehaviour
{
    private PlayerControls _playerInput;
    [SerializeField] private float _accelerationInput;
    [SerializeField] private float _steeringInput;
    [SerializeField] private float _rightTailWhip;
    [SerializeField] private float _leftTailWhip;

    [SerializeField] private Rigidbody _sphereRB;
    [SerializeField] private GameObject _wagon;
    [SerializeField] private ParticleSystem[] _dustTrail;
    [SerializeField] private float _forwardAcceleration = 8f;
    [SerializeField] private float _reverseAcceleration = 4f;
    [SerializeField] private float _turnStrength = 180f;
    [SerializeField] private float _gravityForce = 10f;
    [SerializeField] private float _dragOnGround = 3f;
    [SerializeField] private float maxTippingAngle = 45f;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Transform _groundRayPoint;
    [SerializeField] private float _groundRayLength = 0.5f;
    [SerializeField] private bool _grounded;
    [SerializeField] private ConfigurableJoint _joint;
    [SerializeField] private Rigidbody _wagonRB;
    [SerializeField] private float _tailWhipForce;
    [SerializeField] private Transform[] _tailWhipPositions;
    private float _speedInput;

    private void Awake()
    {
        _playerInput = new PlayerControls();
        _sphereRB.transform.parent = null;
        _joint = _wagon.GetComponent<ConfigurableJoint>();
        _wagonRB = _wagon.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Controls.Acceleration.performed += OnAccelerate;
        _playerInput.Controls.Acceleration.canceled += OnReleaseAccelerate;
        _playerInput.Controls.Steering.performed += OnSteering;
        _playerInput.Controls.Steering.canceled += OnReleaseSteering;
        _playerInput.Controls.TailWhipRight.performed += OnRightTailWhip;
        _playerInput.Controls.TailWhipRight.canceled += OnReleaseRightTailWhip;
        _playerInput.Controls.TailWhipLeft.performed += OnLeftTailWhip;
        _playerInput.Controls.TailWhipLeft.canceled += OnReleaseLeftTailWhip;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Update()
    {
        _grounded = false;
        RaycastHit hit;

        if (Physics.Raycast(_groundRayPoint.position, -transform.up, out hit, _groundRayLength, _whatIsGround))
        {
            _grounded = true;
        }

        //Moving
        _speedInput = 0f;
        _speedInput = _accelerationInput > 0 ? _forwardAcceleration : _reverseAcceleration;
        _speedInput *= 1000f * _accelerationInput;

        //Adjust wagon movement for reversing
        _joint.angularYMotion = _accelerationInput < 0 ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;
        _joint.angularXMotion = _accelerationInput < 0 ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;

        //Adjust Particles
        if(_accelerationInput > 0 && _grounded)
        {
            //PlayDustParticles();
        }
        else
        {
            StopDustParticles();
        } 

        //Update positon
        transform.position = _sphereRB.transform.position;
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
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _steeringInput * _turnStrength * Time.deltaTime * _accelerationInput, 0f));
        }
        else//add gravity when in air
        {
            StopDustParticles();

            _sphereRB.drag = 0.0f;
            _sphereRB.AddForce(Vector3.up * -_gravityForce * 100f);
        }

        if (_leftTailWhip > 0)
        {
            _wagonRB.AddForceAtPosition(-_wagon.transform.right * _tailWhipForce, _tailWhipPositions[0].position, ForceMode.Impulse);
            Debug.Log("Left");
        }
        if (_rightTailWhip > 0)
        {
            _wagonRB.AddForceAtPosition(_wagon.transform.right * _tailWhipForce, _tailWhipPositions[1].position, ForceMode.Impulse);
            Debug.Log("right");
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

    private void OnAccelerate(InputAction.CallbackContext value)
    {
        _accelerationInput = value.ReadValue<float>();
    }

    private void OnReleaseAccelerate(InputAction.CallbackContext value)
    {
        _accelerationInput = 0;
    }

    private void OnSteering(InputAction.CallbackContext value)
    {
        _steeringInput = value.ReadValue<float>();
    }

    private void OnReleaseSteering(InputAction.CallbackContext value)
    {
        _steeringInput = 0;
    }

    private void OnRightTailWhip(InputAction.CallbackContext obj)
    {
        _rightTailWhip = obj.ReadValue<float>();
    }

    private void OnReleaseRightTailWhip(InputAction.CallbackContext obj)
    {
        _rightTailWhip = 0;
    }

    private void OnLeftTailWhip(InputAction.CallbackContext obj)
    {
        _leftTailWhip = obj.ReadValue<float>();
    }

    private void OnReleaseLeftTailWhip(InputAction.CallbackContext obj)
    {
        _leftTailWhip = 0;
    }
}
