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

    [SerializeField] private float _moveAcceleration = 50f;
    [SerializeField] private float _maxSpeed = 15f;
    [SerializeField] private float _drag = 0.98f;
    [SerializeField] private float _steerAngle = 5f;
    [SerializeField] private float _traction = 1f;
    [SerializeField] private float _reverseSpeed = 5f;
    
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
        _playerRb = GetComponent<Rigidbody>();
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

    private void FixedUpdate()
    {
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
