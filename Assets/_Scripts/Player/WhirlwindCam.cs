using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlwindCam : MonoBehaviour
{
    [SerializeField] Transform m_target;
    [SerializeField] LayerMask m_collisionMask;

    Vector3 m_defaultDistanceFromPlayer;
    public void SetOffset() => m_defaultDistanceFromPlayer = transform.position - m_target.position;

    private void Update()
    {
        if (Physics.Raycast(m_target.position, transform.position - m_target.position, out RaycastHit hit, m_defaultDistanceFromPlayer.magnitude, m_collisionMask))
            transform.position = hit.point;
        else transform.position = m_target.position + m_defaultDistanceFromPlayer;
    }
}
