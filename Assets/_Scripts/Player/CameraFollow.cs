using Cinemachine;
using UnityEngine;
using DG.Tweening;
//using DG.Tweening.Plugins.Options;
//using DG.Tweening.Core;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraFollow : MonoBehaviour
{
    //[SerializeField] Camera m_camera;
    [SerializeField] CinemachineVirtualCamera m_airControlCam;
    [SerializeField] CinemachineVirtualCamera m_whirlwindCam;
    public Transform whirlwindCam { get { return m_whirlwindCam.transform; } }

    CinemachineVirtualCamera m_cam;
    
    public Transform m_target { get { return m_cam.m_Follow; } }

    float m_originalFOV;
    public float originalFOV { get { return m_originalFOV; } }
    float m_targetFOV;
    float m_tweenSpeedFOV;

    bool m_isWhirlwind;

    private void Awake()
    {
        m_cam = GetComponent<CinemachineVirtualCamera>();

        m_originalFOV = m_cam.m_Lens.FieldOfView;
        m_targetFOV = m_cam.m_Lens.FieldOfView;
    }

    private void Update()
    {
        UpdateLerpFOV();
        if (!m_isWhirlwind) UpdateWhirlwindCamPos();
    }

    public void TweenFOV(float _targetFOV, float _speed)
    {
        m_targetFOV = _targetFOV;
        m_tweenSpeedFOV = _speed;
    }

    void UpdateLerpFOV()
    {
        // Handles FOV lerping
        if (m_cam.m_Lens.FieldOfView != m_targetFOV)
        {
            if (m_cam.m_Lens.FieldOfView < m_targetFOV)
            {
                m_cam.m_Lens.FieldOfView += Time.deltaTime * m_tweenSpeedFOV;
                if (m_cam.m_Lens.FieldOfView > m_targetFOV) m_cam.m_Lens.FieldOfView = m_targetFOV;
            }
            else if (m_cam.m_Lens.FieldOfView > m_targetFOV)
            {
                m_cam.m_Lens.FieldOfView -= Time.deltaTime * m_tweenSpeedFOV;
                if (m_cam.m_Lens.FieldOfView < m_targetFOV) m_cam.m_Lens.FieldOfView = m_targetFOV;
            }
        }
    }

    public void TweenTargetRotation(Vector3 _newRot, float _duration) => m_target.DOLocalRotate(_newRot, _duration);

    public void SetAirControl() => m_airControlCam.Priority = 12;

    public void StopAirControl() => m_airControlCam.Priority = 10;

    void UpdateWhirlwindCamPos() => m_whirlwindCam.transform.SetPositionAndRotation(transform.position, transform.rotation);

    public void StartWhirlwind()
    {
        m_isWhirlwind = true;

        m_whirlwindCam.Priority = 12;
    }

    public void StopWhirlwind()
    {
        TimerManager.RunAfterTime(() => { m_isWhirlwind = false; }, 0.6f);
        
        m_whirlwindCam.Priority = 10;
    }
}
