using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOV : MonoBehaviour
{
    private CinemachineFreeLook _cam;
    private float _targetFOV;
    private float _fov;

    private void Awake()
    {
        _cam = GetComponent<CinemachineFreeLook>();
        _targetFOV = _cam.m_Lens.FieldOfView;
        _fov = _targetFOV;
    }

    private void Update()
    {
        float fovSpeed = 4f;
        _fov = Mathf.Lerp(_fov, _targetFOV, Time.deltaTime * fovSpeed);
        _cam.m_Lens.FieldOfView = _fov;
    }

    public void SetCameraFov(float targetFov)
    {
        _targetFOV = targetFov;
    }
}
