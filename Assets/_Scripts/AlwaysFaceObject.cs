using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{
    MainCamera,
    Other
}

public class AlwaysFaceObject : MonoBehaviour
{
    [SerializeField] TargetType m_targetType;
    [SerializeField] [ConditionalEnumHide("m_targetType", 1)] Transform m_target;
    [SerializeField] bool m_useDirection, m_isInverse, m_onUpdate;

    private void Awake()
    {
        m_target = Camera.main.transform;

        UpdateRotation();
    }
    private void Update() { if (m_onUpdate) UpdateRotation(); }

    void UpdateRotation()
    {
        if (!m_useDirection) transform.LookAt(m_target);
        else transform.forward = m_target.forward;

        if (m_isInverse) transform.forward = -transform.forward;
    }
}
