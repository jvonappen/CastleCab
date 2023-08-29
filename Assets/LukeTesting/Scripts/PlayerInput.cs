using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerInput : MonoBehaviour
{
    private PlayerControls _playerControls;
    [SerializeField] public float _accelerationInput { get; private set; }
    [SerializeField] public float _steeringInput { get; private set; }
    [SerializeField] public float _tailWhip { get; private set; }
    [SerializeField] public float _boost { get; private set; }

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
        _playerControls.Controls.Acceleration.performed += OnAccelerate;
        _playerControls.Controls.Acceleration.canceled += OnReleaseAccelerate;
        _playerControls.Controls.Steering.performed += OnSteering;
        _playerControls.Controls.Steering.canceled += OnReleaseSteering;
        _playerControls.Controls.TailWhip.performed += OnTailWhip;
        _playerControls.Controls.TailWhip.canceled += OnReleaseTailWhip;
        _playerControls.Controls.Boost.performed += OnBoost;
        _playerControls.Controls.Boost.canceled += OnReleaseBoost;
    }

    private void OnDisable()
    {
        _playerControls.Disable();
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

    private void OnTailWhip(InputAction.CallbackContext value)
    {
        _tailWhip = value.ReadValue<float>();
    }

    private void OnReleaseTailWhip(InputAction.CallbackContext value)
    {
        _tailWhip = 0;
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
