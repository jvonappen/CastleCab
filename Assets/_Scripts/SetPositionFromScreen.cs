using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SetPositionFromScreen : MonoBehaviour
{
    [SerializeField] Camera m_cam;
    [SerializeField] Vector3 m_screenPoint;

    private void Awake()
    {
        ScreenSize.onWindowResize += SetPositionAtScreenPoint;

        InputManager.onPlayerJoined += SetPositionAtScreenPoint;
        InputManager.onPlayerLeft += SetPositionAtScreenPoint;

        SetPositionAtScreenPoint();
    }

    void SetPositionAtScreenPoint(PlayerInput _player, List<PlayerInput> _players) // TODO
    {
        Camera playerCamera = _player.camera;

        Vector3 screenPos = Vector3.zero;
        //if (m_screenPoint.x != 0) screenPos += Vector3.right * (Screen.width * m_screenPoint.x);
        //if (m_screenPoint.y != 0) screenPos += Vector3.up * (Screen.height * m_screenPoint.y);
        //
        //screenPos += Vector3.forward * m_screenPoint.z;

        screenPos *= playerCamera.pixelWidth;

        //if (_players.Count > 1) screenPos /= 2;
        //if (_players.Count > 2) screenPos /= 2;
        //
        //Debug.Log(screenPos);

        transform.position = m_cam.ScreenToWorldPoint(screenPos);
    }
    void SetPositionAtScreenPoint()
    {
        Vector3 screenPos = Vector3.zero;
        if (m_screenPoint.x != 0) screenPos += Vector3.right * (Screen.width * m_screenPoint.x);
        if (m_screenPoint.y != 0) screenPos += Vector3.up * (Screen.height * m_screenPoint.y);

        screenPos += Vector3.forward * m_screenPoint.z;

        Debug.Log(screenPos);

        transform.position = m_cam.ScreenToWorldPoint(screenPos);
    }
}
