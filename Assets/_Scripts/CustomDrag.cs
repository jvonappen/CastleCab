using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDrag : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float m_dragX = 1, m_dragY = 1, m_dragZ = 1;

    public float dragX { set { m_dragX = value; } get { return m_dragX; } }
    public float dragY { set { m_dragY = value; } get { return m_dragY; } }
    public float dragZ { set { m_dragZ = value; } get { return m_dragZ; } }

    private void Awake() => rb = GetComponent<Rigidbody>();

    private void FixedUpdate()
    {
        Vector3 vel = rb.velocity;

        vel.x *= m_dragX;
        vel.y *= m_dragY;
        vel.z *= m_dragZ;

        rb.velocity = vel;
        //if (m_dragX != 0) rb.velocity = new Vector3(vel.x * (1 - m_dragX * Time.fixedDeltaTime), vel.y, vel.z);
        //if (m_dragY != 0) rb.velocity = new Vector3(vel.x, vel.y * (1 - m_dragY * Time.fixedDeltaTime), vel.z);
        //if (m_dragZ != 0) rb.velocity = new Vector3(vel.x, vel.y, vel.z * (1 - m_dragZ * Time.fixedDeltaTime));
    }
}
