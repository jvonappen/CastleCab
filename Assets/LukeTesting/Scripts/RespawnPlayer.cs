using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnPlayer : MonoBehaviour
{
    private PlayerControls _playerInput;
    [SerializeField] private bool _resetPlayerInput;
    [SerializeField] private bool _canResetPosition;
    [SerializeField] private GameObject _playerWagon;

    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Transform[] _flippedWagonPoint;
    [SerializeField] private float _groundRayLength = 4f;


    private void Awake()
    {
        _playerInput = new PlayerControls();
        _canResetPosition = false;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Controls.ResetPlayer.performed += OnResetPlayer;
        _playerInput.Controls.ResetPlayer.canceled += OnReleaseResetPlayer;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Update()
    {
        WagonFlipped();

        if (_resetPlayerInput && _canResetPosition)
        {
            ResetWagonPosition();
        }
    }

    private void ResetWagonPosition()
    {
        Vector3 wagonFix = new Vector3(0, _playerWagon.transform.position.y, 0);
        _playerWagon.transform.rotation = Quaternion.FromToRotation(_playerWagon.transform.position, wagonFix);
        _resetPlayerInput = false;
    }

    private void WagonFlipped()
    {
        for (int i = 0; i < _flippedWagonPoint.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(_flippedWagonPoint[i].position, _playerWagon.transform.up, out hit, _groundRayLength, _whatIsGround))
            {
                _canResetPosition = true;
                Debug.Log("cacked");
            }
            else
            {
                _canResetPosition = false;
            }
            Debug.DrawRay(_flippedWagonPoint[i].position, _playerWagon.transform.up * _groundRayLength, Color.red);
        }
    }

    private void OnResetPlayer(InputAction.CallbackContext context)
    {
        _resetPlayerInput = true;
    }

    private void OnReleaseResetPlayer(InputAction.CallbackContext context)
    {
        _resetPlayerInput = false;
    }
}
