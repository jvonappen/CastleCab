using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCustomisation : MonoBehaviour
{
    private PlayerInput _playerInput;
    [SerializeField] private GameObject _customisationCanvas;
    [SerializeField] private GameObject _customisationSetup;
    public bool _canTransfer;
    private SwitchCamera _switchCamera;

    private void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
        _customisationCanvas.SetActive(false);
        _switchCamera = GetComponent<SwitchCamera>();
        _customisationSetup.SetActive(false);
    }

    private void Update()
    {
        if (_canTransfer && _playerInput._interact)
        {
            TransferPlayer();
            _canTransfer = false;
        }
        else if (_customisationSetup.activeSelf == true &&  _playerInput._interact)
        {
            TransferPlayer();
            _canTransfer = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _customisationCanvas.SetActive(true);
            _canTransfer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _customisationCanvas.SetActive(false);
            _canTransfer = false;
        }
    }

    public void TransferPlayer()
    {
        _customisationSetup.SetActive(!_customisationSetup.activeSelf);
        _switchCamera.ManageCamera();
    }
}
