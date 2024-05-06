using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotate : MonoBehaviour
{
    [SerializeField] Rigidbody m_rb;
    [SerializeField] float m_speedMultiplier;

    private void Update() => transform.Rotate(m_rb.velocity.magnitude * m_speedMultiplier * 360 * Time.deltaTime, 0, 0);
}
