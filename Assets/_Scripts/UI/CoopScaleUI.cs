using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CoopScaleUI : PlayerJoinedNotifier
{
    [SerializeField] Vector3 m_newScale;
    Vector3 m_originalScale;

    [SerializeField] int m_playersUntilActive = 3;


    public void Start()
    {
        m_originalScale = transform.localScale;
        if (GameManager.Instance.players.Count > m_playersUntilActive - 1) transform.localScale = m_newScale;
    }

    public override void OnPlayerJoined(PlayerInput _player, List<PlayerInput> _players)
    {
        base.OnPlayerJoined(_player, _players);
        if (GameManager.Instance.players.Count > m_playersUntilActive - 2) transform.localScale = m_newScale;
    }

    public override void OnPlayerLeft(PlayerInput _player, List<PlayerInput> _players)
    {
        base.OnPlayerLeft(_player, _players);
        if (GameManager.Instance.players.Count < m_playersUntilActive + 1) transform.localScale = m_originalScale;

    }
}
