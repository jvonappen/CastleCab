using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerControls _playerInput;
    [SerializeField] private float _accelerationInput;
    [SerializeField] private float _steeringInput;

    [SerializeField] private Rigidbody _sphereRB;
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
    private float _speedInput;

    private void Awake()
    {
        _playerInput = new PlayerControls();
        _sphereRB.transform.parent = null;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Controls.Acceleration.performed += OnAccelerate;
        _playerInput.Controls.Acceleration.canceled += OnReleaseAccelerate;
        _playerInput.Controls.Steering.performed += OnSteering;
        _playerInput.Controls.Steering.canceled += OnReleaseSteering;
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

        //Steering
        if (_grounded)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, _steeringInput * _turnStrength * Time.deltaTime * _accelerationInput, 0f));
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
            _groundRayLength = 1.5f;
        }
        else//add gravity when in air
        {
            _sphereRB.drag = 0.0f;
            _sphereRB.AddForce(Vector3.up * -_gravityForce * 100f);
            _groundRayLength = 1.5f;
        }

        //control in air
        float angle = Vector3.Angle(transform.up, Vector3.up);
        if (angle > maxTippingAngle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up), Mathf.InverseLerp(angle, 0, maxTippingAngle));
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
}
