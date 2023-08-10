using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCam : MonoBehaviour
{
    public static Camera miniMapCamera;

    [SerializeField] private Camera m_Camera;
    [SerializeField] private GameObject player;

    private Vector3 pos;

    private void Awake()
    {
        miniMapCamera = m_Camera;
        MapClamp.minimapCamTransform = m_Camera.transform;
    }

    private void LateUpdate()
    {
        pos = new Vector3(player.transform.position.x, m_Camera.transform.position.y, player.transform.position.z);
        m_Camera.transform.position = pos;    
    }

}
