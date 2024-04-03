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

    void SetPositionAtScreenPoint(PlayerInput _player, List<PlayerInput> _players) => SetPositionAtScreenPoint();
    void SetPositionAtScreenPoint()
    {
        float camWidth = Screen.width * m_cam.rect.width;
        float camHeight = Screen.height * m_cam.rect.height;

        float camPosX = 0;
        if (m_cam.rect.x != 0) camPosX = m_cam.rect.x * Screen.width;
        float camPosY = 0;
        if (m_cam.rect.y != 0) camPosY = m_cam.rect.y * Screen.height;

        Vector3 screenPos = new Vector3(camPosX, camPosY, m_screenPoint.z);
        if (m_screenPoint.x != 0) screenPos += Vector3.right * (camWidth * m_screenPoint.x);
        if (m_screenPoint.y != 0) screenPos += Vector3.up * (camHeight * m_screenPoint.y);

        transform.position = m_cam.ScreenToWorldPoint(screenPos);
    }
}
