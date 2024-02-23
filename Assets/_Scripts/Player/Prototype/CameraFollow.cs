using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
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

    public void SetOffsetWorldSpace()
    {
        m_useOffsetOverride = true;
        m_worldOffsetOverride = m_lookAt.position + m_lookAt.TransformDirection(m_cameraOffset) - m_lookAt.position;
    }

    private void Awake()
    {
        m_originalCamSpeed = m_camSpeed;
        m_originalCameraOffset = m_cameraOffset;

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

    private void LateUpdate()
    {
        if (!m_lockRotation) //transform.LookAt(m_lookAt.position + m_lookAtOffset); - old code, keep until cam isnt buggy
        {
            Vector3 dir = (m_lookAt.position + m_lookAtOffset) - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            // slerp to the desired rotation over time
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, m_camSpeed * Time.deltaTime);
        }
    }

    public void TweenToOriginalCamPosition(float _duration)
    {
        m_isTweeningToOriginalCamPos = true;
        m_tweenVelocity = (m_originalCameraOffset - m_cameraOffset) / _duration;
    }
}
