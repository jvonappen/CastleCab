using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCam : MonoBehaviour
{
    public static Camera miniMapCamera;

    private Camera m_Camera;
    private GameObject player;

    private Vector3 pos;

    private void Awake()
    {
        player = PlayerData.player;
        m_Camera = this.gameObject.GetComponent<Camera>();
        miniMapCamera = m_Camera;
        MapClamp.minimapCamTransform = m_Camera.transform;
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            pos = new Vector3(player.transform.position.x, m_Camera.transform.position.y, player.transform.position.z);
            m_Camera.transform.position = pos;
        }
        if (player == null) { player = PlayerData.player; Debug.Log("ERROR: Minimap cam can't find player"); }
    }

}
