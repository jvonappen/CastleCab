using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectionSize2P : PlayerJoinedNotifier
{
    [SerializeField] float m_projectionSize2P;
    float m_originalProjectionSize;

    Camera m_cam;

    public override void Start()
    {
        base.Start();

        m_cam = GetComponent<Camera>();
        m_originalProjectionSize = m_cam.orthographicSize;

        if (GameManager.Instance.players.Count == 2) m_cam.orthographicSize = m_projectionSize2P;
    }

    public override void OnPlayerJoined(PlayerInput _player, List<PlayerInput> _players)
    {
        base.OnPlayerJoined(_player, _players);

        if (_players.Count == 2) m_cam.orthographicSize = m_projectionSize2P;
        else m_cam.orthographicSize = m_originalProjectionSize;
    }

    public override void OnPlayerLeft(PlayerInput _player, List<PlayerInput> _players)
    {
        base.OnPlayerLeft(_player, _players);

        if (_players.Count == 2) m_cam.orthographicSize = m_projectionSize2P;
        else m_cam.orthographicSize = m_originalProjectionSize;
    }
}
