using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowOld : MonoBehaviour
{
    #region Variables
    Camera m_cam;

    [SerializeField] Transform m_lookAt;
    [SerializeField] Vector3 m_cameraOffset, m_lookAtOffset;
    [SerializeField] float m_speed, m_camSpeed;
    public float m_originalCamSpeed { get; private set; }
    public Vector3 m_originalCameraOffset { get; private set; }

    public Transform lookAt { get { return m_lookAt; } }
    public Vector3 camOffset { get { return m_cameraOffset; } set { m_cameraOffset = value; } }
    public Vector3 lookAtOffset { get { return m_lookAtOffset; } }

    public float camSpeed { get { return m_camSpeed; } set { m_camSpeed = value; } }

    public bool m_useOffsetOverride { get; set; }
    public Vector3 m_worldOffsetOverride { get; set; }
    public bool m_lockRotation { get; set; }
    public bool m_lockMovement { get; set; }

    bool m_isTweeningToOriginalCamPos;
    Vector3 m_tweenVelocity;


    float m_originalFOV;
    public float originalFOV { get { return m_originalFOV; } }
    float m_currentFOV;
    float m_targetFOV;
    float m_tweenSpeedFOV;

    #endregion

    public void SetOffsetWorldSpace()
    {
        m_useOffsetOverride = true;
        m_worldOffsetOverride = m_lookAt.position + m_lookAt.TransformDirection(m_cameraOffset) - m_lookAt.position;
    }

    public void TweenFOV(float _targetFOV, float _speed)
    {
        m_targetFOV = _targetFOV;
        m_tweenSpeedFOV = _speed;
    }

    public void TweenToOriginalCamPosition(float _duration)
    {
        m_isTweeningToOriginalCamPos = true;
        m_tweenVelocity = (m_originalCameraOffset - m_cameraOffset) / _duration;
    }




    private void Awake()
    {
        m_cam = GetComponent<Camera>();

        m_originalCamSpeed = m_camSpeed;
        m_originalCameraOffset = m_cameraOffset;

        m_currentFOV = m_cam.fieldOfView;
        m_originalFOV = m_currentFOV;
        m_targetFOV = m_currentFOV;

        if (m_lookAt.parent) m_lookAt.parent.BroadcastMessage("OnCameraSetTarget", this, SendMessageOptions.DontRequireReceiver);
        else m_lookAt.gameObject.BroadcastMessage("OnCameraSetTarget", this, SendMessageOptions.DontRequireReceiver);
    }

    private void OnValidate()
    {
        transform.position = m_lookAt.position + m_cameraOffset;
        transform.LookAt(m_lookAt.position + m_lookAtOffset);
    }

    private void Update()
    {
        Move();
        UpdateLerpFOV();
    }

    private void LateUpdate()
    {
        Rotate();
    }



    #region CameraFollow
    void Move()
    {
        if (!m_lockMovement)
        {
            if (m_isTweeningToOriginalCamPos)
            {
                m_cameraOffset += m_tweenVelocity * Time.deltaTime;

                if (Vector3.Distance(m_cameraOffset, m_originalCameraOffset) < 0.5f)
                {
                    m_cameraOffset = m_originalCameraOffset;
                    m_isTweeningToOriginalCamPos = false;
                }
            }

            Vector3 offset = m_useOffsetOverride ? m_worldOffsetOverride : m_lookAt.TransformDirection(m_cameraOffset);
            Vector3 moveVelocity = (m_lookAt.position + offset) - transform.position;
            transform.position += m_speed * Time.deltaTime * moveVelocity;
        }
    }

    void Rotate()
    {
        if (!m_lockRotation) 
        {
            Vector3 dir = (m_lookAt.position + m_lookAtOffset) - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            // slerp to the desired rotation over time
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, m_camSpeed * Time.deltaTime);
        }
    }

    void UpdateLerpFOV()
    {
        // Handles FOV lerping
        if (m_currentFOV != m_targetFOV)
        {
            if (m_currentFOV < m_targetFOV)
            {
                m_currentFOV += Time.deltaTime * m_tweenSpeedFOV;
                if (m_currentFOV > m_targetFOV) m_currentFOV = m_targetFOV;
            }
            else if (m_currentFOV > m_targetFOV)
            {
                m_currentFOV -= Time.deltaTime * m_tweenSpeedFOV;
                if (m_currentFOV < m_targetFOV) m_currentFOV = m_targetFOV;
            }

            m_cam.fieldOfView = m_currentFOV;
        }
    }
    #endregion
}
