using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputOld : MonoBehaviour
{
    //[Header("INPUT VARIABLES FOR DEBUGGING, DO NOT TOUCH")]
    public PlayerControls _playerControls {  get; private set; }
    [field: SerializeField] public float _accelerationInput { get; private set; }
    [field: SerializeField] public float _reverseInput { get; private set; }
    [field: SerializeField] public float _steeringInput { get; private set; }
    [field: SerializeField] public float _tailWhip { get; private set; }
    [field: SerializeField] public float _boost { get; private set; }
    [field: SerializeField] public float _backflip { get; private set; }
    [field: SerializeField] public float _barrelRoll { get; private set; }
    [field: SerializeField] public bool _interact { get; private set; }



    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
        _playerControls.Controls.Acceleration.performed += OnAccelerate;
        _playerControls.Controls.Acceleration.canceled += OnReleaseAccelerate;
        _playerControls.Controls.Reverse.performed += OnReverse;
        _playerControls.Controls.Reverse.canceled += OnReleaseReverse;
        _playerControls.Controls.Steering.performed += OnSteering;
        _playerControls.Controls.Steering.canceled += OnReleaseSteering;
        _playerControls.Controls.Drift.performed += OnTailWhip;
        _playerControls.Controls.Drift.canceled += OnReleaseTailWhip;
        _playerControls.Controls.Boost.performed += OnBoost;
        _playerControls.Controls.Boost.canceled += OnReleaseBoost;
        _playerControls.Controls.Backflip.performed += OnBackflip;
        _playerControls.Controls.Backflip.canceled += OnReleaseBackflip;
        _playerControls.Controls.BarrelRoll.performed += OnBarrelRoll;
        _playerControls.Controls.BarrelRoll.canceled += OnReleaseBarrelRoll;
        _playerControls.Controls.Interact.started += OnInteract;
        _playerControls.Controls.Interact.canceled += OnReleaseInteract;
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Update()
    {
        _steeringInput = _playerControls.Controls.Steering.ReadValue<float>();
    }

    private void OnAccelerate(InputAction.CallbackContext value)
    {
        _accelerationInput = value.ReadValue<float>();
    }

    private void OnReleaseAccelerate(InputAction.CallbackContext value)
    {
        _accelerationInput = 0;
    }

    private void OnReverse(InputAction.CallbackContext value)
    {
        _reverseInput = value.ReadValue<float>();
    }

    private void OnReleaseReverse(InputAction.CallbackContext value)
    {
        _reverseInput = 0;
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
        _boost = 1;
    }

    private void OnReleaseBoost(InputAction.CallbackContext value)
    {
        _boost = 0;
    }

    private void OnBackflip(InputAction.CallbackContext value)
    {
        _backflip = 1;
    }

    private void OnReleaseBackflip(InputAction.CallbackContext value)
    {
        _backflip = 0;
    }

    private void OnBarrelRoll(InputAction.CallbackContext value)
    {
        _barrelRoll = 1;
    }

    private void OnReleaseBarrelRoll(InputAction.CallbackContext value)
    {
        _barrelRoll = 0;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started) _interact = true;
    }

    private void OnReleaseInteract(InputAction.CallbackContext context)
    {
        if (context.canceled) _interact = false;
    }
}
