using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform m_lookAt;
    [SerializeField] Vector3 m_cameraOffset, m_lookAtOffset;
    [SerializeField] float m_speed;

    private void OnValidate()
    {
        transform.position = m_lookAt.position + m_cameraOffset;
        transform.LookAt(m_lookAt.position + m_lookAtOffset);
    }

    private void Update()
    {
        Vector3 moveVelocity = (m_lookAt.position + m_lookAt.TransformDirection(m_cameraOffset)) - transform.position;
        transform.position += m_speed * Time.deltaTime * moveVelocity;
    }

    private void LateUpdate()
    {
        transform.LookAt(m_lookAt.position + m_lookAtOffset);
    }
}
