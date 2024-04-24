using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SetPositionOnCamera : MonoBehaviour
{
    [SerializeField] Camera m_cam;
    [Tooltip("Value between (0,0) and (1,1). Min represents bottom left and max represents top right")] [SerializeField] Vector3 m_positionOnCamera;

    #region Delegates
    private void OnEnable()
    {
        ScreenSize.onWindowResize += SetPositionAtScreenPoint;

        InputManager.onPlayerJoined += SetPositionAtScreenPoint;
        InputManager.onPlayerLeft += SetPositionAtScreenPoint;

        TimerManager.RunAfterTime(SetPositionAtScreenPoint, 0.1f);
    }

    private void OnDisable()
    {
        ScreenSize.onWindowResize -= SetPositionAtScreenPoint;

        InputManager.onPlayerJoined -= SetPositionAtScreenPoint;
        InputManager.onPlayerLeft -= SetPositionAtScreenPoint;
    }
    #endregion

    void SetPositionAtScreenPoint(PlayerInput _player, List<PlayerInput> _players)
    {
        SetPositionAtScreenPoint();
        //TimerManager.RunAfterTime(SetPositionAtScreenPoint, 0.1f);
    }
    void SetPositionAtScreenPoint()
    {
        float camWidth = Screen.width * m_cam.rect.width;
        float camHeight = Screen.height * m_cam.rect.height;

        float camPosX = 0;
        if (m_cam.rect.x != 0) camPosX = m_cam.rect.x * Screen.width;
        float camPosY = 0;
        if (m_cam.rect.y != 0) camPosY = m_cam.rect.y * Screen.height;

        Vector3 screenPos = new Vector3(camPosX, camPosY, m_positionOnCamera.z);
        if (m_positionOnCamera.x != 0) screenPos += Vector3.right * (camWidth * m_positionOnCamera.x);
        if (m_positionOnCamera.y != 0) screenPos += Vector3.up * (camHeight * m_positionOnCamera.y);

        transform.position = m_cam.ScreenToWorldPoint(screenPos);
    }
}
