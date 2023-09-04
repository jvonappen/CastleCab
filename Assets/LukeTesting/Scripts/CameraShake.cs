using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }   
    [SerializeField] private List<CinemachineVirtualCamera> _rigs;
    private float _timer;

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < 3; i++)
        {
            CinemachineVirtualCamera rig = GetComponent<CinemachineFreeLook>().GetRig(i);
            _rigs.Add(rig);
        }
    }

    public void ShakeCamera(float intesity, float time)
    {
        foreach (CinemachineVirtualCamera rig in _rigs)
        {
            rig.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intesity;
            rig.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = intesity;
        }
        _timer = time;
    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                //Time over
                foreach (CinemachineVirtualCamera rig in _rigs)
                {
                    rig.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
                    rig.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
                }
            }
        }
    }
}
