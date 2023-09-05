using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerControls _playerControls;

    [Header("INPUT VARIABLES FOR DEBUGGING, DO NOT TOUCH")]
    [SerializeField] private SoundManager _soundManager;
    [field: SerializeField] public float _accelerationInput { get; private set; }
    [field: SerializeField] public float _steeringInput { get; private set; }
    [field: SerializeField] public float _tailWhip { get; private set; }
    [field: SerializeField] public float _boost { get; private set; }

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
        if (value.ReadValue<float>() > 0) _steeringInput = 1;
        if (value.ReadValue<float>() < 0) _steeringInput = -1;

        //_steeringInput = value.ReadValue<float>();
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
        _soundManager.Play("Boost");
    }

    private void OnReleaseBoost(InputAction.CallbackContext value)
    {
        _boost = 0;
    }
}
