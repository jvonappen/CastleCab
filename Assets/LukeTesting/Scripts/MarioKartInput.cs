using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarioKartInput : MonoBehaviour
{
    [Header("INPUT VARIABLES FOR DEBUGGING, DO NOT TOUCH")]
    private MarioKartControls _playerControls;
    [field: SerializeField] public float _accelerationInput { get; private set; }
    [field: SerializeField] public float _steeringInput { get; private set; }
    [field: SerializeField] public float _boost { get; private set; }

    private void Awake()
    {
        _playerControls = new MarioKartControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
        _playerControls.Controls.Acceleration.performed += OnAccelerate;
        _playerControls.Controls.Acceleration.canceled += OnReleaseAccelerate;
        _playerControls.Controls.Steering.performed += OnSteering;
        _playerControls.Controls.Steering.canceled += OnReleaseSteering;
        _playerControls.Controls.Boost.performed += OnBoost;
        _playerControls.Controls.Boost.canceled += OnReleaseBoost;
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Update()
    {
        _accelerationInput = _playerControls.Controls.Acceleration.ReadValue<float>();
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
        if (value.ReadValue<float>() > 0) _steeringInput = 1;
        if (value.ReadValue<float>() < 0) _steeringInput = -1;

        //_steeringInput = value.ReadValue<float>();
    }

    private void OnReleaseSteering(InputAction.CallbackContext value)
    {
        _steeringInput = 0;
    }

    private void OnBoost(InputAction.CallbackContext value)
    {
        _boost = value.ReadValue<float>();
    }

    private void OnReleaseBoost(InputAction.CallbackContext value)
    {
        _boost = 0;
    }
}
