using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCustomisation : MonoBehaviour
{
    [SerializeField] private GameObject _customisationCanvas;
    [SerializeField] private GameObject _customisationSetup;
    [SerializeField] private SwitchCamera _switchCamera;
    [SerializeField] private PauseGame _pauseGame;

    private PlayerInput _playerInput;
    private PlayerMovement _playerMovement;
    public bool _canTransfer;
    

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerMovement = GetComponent<PlayerMovement>();
        _customisationCanvas.SetActive(false);
    }

    private void Start()
    {
        _customisationSetup.SetActive(false); //do this after awake so customisation materials can be applied
    }

    private void Update()
    {
        if (_canTransfer && _playerInput._playerControls.Controls.Interact.WasPerformedThisFrame())
        {
            TransferPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PaintShop")
        {
            if (_customisationCanvas.activeSelf == false && _customisationSetup.activeSelf != true)
            {
                _customisationCanvas.SetActive(true);
                _canTransfer = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PaintShop")
        {
            _customisationCanvas.SetActive(false);
            _canTransfer = false;
        }
    }

    private void TransferPlayer()
    {
        _customisationSetup.SetActive(!_customisationSetup.activeSelf);
        _customisationCanvas.SetActive(!_customisationCanvas.activeSelf);
        _switchCamera.ManageCamera();
        _pauseGame.ManageGameState();
        _playerMovement.enabled = _customisationCanvas.activeSelf;
    }
}
