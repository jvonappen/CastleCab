using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentObjectWorldspace : MonoBehaviour
{
    [SerializeField] Transform m_parent;

    Vector3 m_offset;
    private void Awake() => m_offset = transform.position - m_parent.position;
    private void Update()
    {
        transform.position = m_parent.position + m_offset;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, m_parent.eulerAngles.y, transform.eulerAngles.z);
    }
}
