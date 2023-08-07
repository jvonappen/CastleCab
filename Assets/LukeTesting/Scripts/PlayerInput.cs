using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerControls _playerInput;
    [SerializeField] private float _accelerationInput;
    [SerializeField] private float _steeringInput;
    [SerializeField] private float _jumpInput;

<<<<<<< Updated upstream
    [SerializeField] private float _moveAcceleration = 50f;
    [SerializeField] private float _maxSpeed = 15f;
    [SerializeField] private float _drag = 0.98f;
    [SerializeField] private float _steerAngle = 5f;
    [SerializeField] private float _traction = 1f;
    [SerializeField] private float _reverseSpeed = 5f;
=======
    [SerializeField] private Rigidbody _sphereRB;
    [SerializeField] private float _forwardAcceleration = 8f;
    [SerializeField] private float _reverseAcceleration = 4f;
    [SerializeField] private float _maxSpeed = 50f;
    [SerializeField] private float _turnStrength = 180f;
    [SerializeField] private float _gravityForce = 10f;
    [SerializeField] private float _dragOnGround = 3f;
    [SerializeField] private float maxTippingAngle = 45f;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Transform _groundRayPoint;
    [SerializeField] private float _groundRayLength = 0.5f;
    private bool _grounded;
    private float _speedInput;
>>>>>>> Stashed changes
    
    private Vector3 _moveSpeed;

    private Rigidbody _playerRb;
    private bool _grounded;
    private float _groundRayLength = 2f;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] Transform _groundRayPoint;
    [SerializeField] private float _jumpHeight = 10f;


    private void Awake()
    {
        _playerInput = new PlayerControls();
<<<<<<< Updated upstream
        _playerRb = GetComponent<Rigidbody>();
=======
        //_sphereRB.transform.parent = null;
>>>>>>> Stashed changes
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Controls.Acceleration.performed += OnAccelerate;
        _playerInput.Controls.Acceleration.canceled += OnReleaseAccelerate;
        _playerInput.Controls.Steering.performed += OnSteering;
        _playerInput.Controls.Steering.canceled += OnReleaseSteering;
        _playerInput.Controls.Jump.performed += OnJump;
        _playerInput.Controls.Jump.canceled += OnReleaseJump;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Update()
    {
<<<<<<< Updated upstream
        _grounded = false;
        RaycastHit hit;

        if (Physics.Raycast(_groundRayPoint.position, -transform.up, out hit, _groundRayLength, _whatIsGround))
        {
            _grounded = true;
        }

        if (_grounded)
        {
            //Moving
            _moveSpeed += transform.forward * _moveAcceleration * _accelerationInput * Time.deltaTime;
            transform.position += _moveSpeed * Time.deltaTime; //continues moving even when key is not pressed

            //Steering
            transform.Rotate(Vector3.up * _steeringInput, _moveSpeed.magnitude * _steerAngle * Time.deltaTime);

            //Drag
            _moveSpeed *= _drag;
            if (_accelerationInput > 0)
            {
                _moveSpeed = Vector3.ClampMagnitude(_moveSpeed, _maxSpeed);
            }
            else //Slow speed for reveresing
            {
                _moveSpeed = Vector3.ClampMagnitude(_moveSpeed, _reverseSpeed);
            }

            //Traction
            _moveSpeed = Vector3.Lerp(_moveSpeed.normalized, transform.forward, _traction * Time.deltaTime) * _moveSpeed.magnitude;
        }
=======
        //Moving
        _speedInput = 0f;
        _speedInput = _accelerationInput > 0 ? _forwardAcceleration : _reverseAcceleration;
        _speedInput *= 1000f * _accelerationInput;

        //Steering
        if (_grounded)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _steeringInput * _turnStrength * Time.deltaTime * _accelerationInput, 0f));
        }

        //Update positon
        //transform.position = _sphereRB.transform.position;
        Debug.DrawRay(_groundRayPoint.position, -Vector3.up, Color.red);
    }

    private void FixedUpdate()
    {
        _grounded = false;
        RaycastHit hit;

        if(Physics.Raycast(_groundRayPoint.position, Vector3.down, out hit, _groundRayLength, _whatIsGround))
        {
            _grounded = true;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, Time.fixedDeltaTime * 10.0f);
        }

        if(_grounded)//control car on ground
        {
            _sphereRB.drag = _dragOnGround;
            _sphereRB.AddForce(transform.forward * _speedInput);
            _groundRayLength = 0.5f;
        }
        else//add gravity when in air
        {
            _sphereRB.drag = 0.0f;
            _sphereRB.AddForce(Vector3.up * -_gravityForce * 100f);
            _groundRayLength = 1.5f;
        }

        float angle = Vector3.Angle(transform.up, Vector3.up);
        if (angle > maxTippingAngle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up), Mathf.InverseLerp(angle, 0, maxTippingAngle));
        }

>>>>>>> Stashed changes
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

    private void OnJump(InputAction.CallbackContext value)
    {
        _jumpInput = value.ReadValue<float>();
    }

    private void OnReleaseJump(InputAction.CallbackContext value)
    {
        _jumpInput = 0;
    }
}
