using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonRotationFixer : MonoBehaviour
{
    [SerializeField] Vector3 m_defaultPos;
    [SerializeField] Vector3 m_defaultRot;
    
    public void ResetPosAndRot()
    {
        transform.localPosition = m_defaultPos;
        transform.localEulerAngles = m_defaultRot;
    }
}
