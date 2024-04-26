using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera m_defaultCam;
    CinemachineBrain m_brain;

    private void Awake()
    {
        m_brain = GetComponent<CinemachineBrain>();
    }
}
