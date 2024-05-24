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

        if (GameManager.Instance.players.Count != 2) TimerManager.RunAfterTime(SetPositionAtScreenPoint, 0.1f);
        else TimerManager.RunAfterTime(SetPosition2pUpgrade, 0.1f);
    }

    private void OnDisable()
    {
        ScreenSize.onWindowResize -= SetPositionAtScreenPoint;

        InputManager.onPlayerJoined -= SetPositionAtScreenPoint;
        InputManager.onPlayerLeft -= SetPositionAtScreenPoint;
    }
    #endregion

    void SetPositionAtScreenPoint(PlayerInput _player, List<PlayerInput> _players) => SetPositionAtScreenPoint();
    public void SetPositionAtScreenPoint() => SetPositionAtPoint(m_positionOnCamera);

    public void SetPositionAtPoint(Vector3 _point)
    {
        float camWidth = Screen.width * m_cam.rect.width;
        float camHeight = Screen.height * m_cam.rect.height;

        float camPosX = 0;
        if (m_cam.rect.x != 0) camPosX = m_cam.rect.x * Screen.width;
        float camPosY = 0;
        if (m_cam.rect.y != 0) camPosY = m_cam.rect.y * Screen.height;

        Vector3 screenPos = new Vector3(camPosX, camPosY, _point.z);
        if (_point.x != 0) screenPos += Vector3.right * (camWidth * _point.x);
        if (_point.y != 0) screenPos += Vector3.up * (camHeight * _point.y);

        transform.position = m_cam.ScreenToWorldPoint(screenPos);
    }

    public void SetPositionReady() => SetPositionAtPoint(new(0.66f, 0.5f, 3));
    public void SetPosition2pCustomise() => SetPositionAtPoint(new(0.69f, 0.66f, 3));
    public void SetPositionCentre() => SetPositionAtPoint(new(0.5f, 0.5f, 3));
    public void SetPosition2pUpgrade() => SetPositionAtPoint(new(0.75f, 0.5f, 3));
}
