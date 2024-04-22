using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using URNTS;

public class InputManager : MonoBehaviour
{
    [SerializeField] GameObject m_playerPrefab;
    bool m_firstPlayerSpawned;

    public static Action<PlayerInput, List<PlayerInput>> onPlayerJoined;
    public static Action<PlayerInput, List<PlayerInput>> onPlayerLeft;

    [SerializeField] bool m_randomiseSpawnpoint = true;
    [SerializeField] List<Transform> m_remainingSpawnPoints;

    [SerializeField] TrafficManager m_trafficManager;
    List<PlayerInput> m_players = new();

    public void OnPlayerJoined(PlayerInput _player)
    {
        m_players.Add(_player);

        onPlayerJoined?.Invoke(_player, m_players);

        if (m_trafficManager)
        {
            if (_player.TryGetComponent(out PlayerMovement playerMovement)) m_trafficManager.AddPlayer(playerMovement.horse);
        }
    }

    public void OnPlayerLeft(PlayerInput _player)
    {
        m_players.Remove(_player);
    
        onPlayerLeft?.Invoke(_player, m_players);
    
        if (m_trafficManager)
        {
            if (_player.TryGetComponent(out PlayerMovement playerMovement)) m_trafficManager.RemovePlayer(playerMovement.horse);
        }
    }

    private void Awake()
    {
        InputUser.onUnpairedDeviceUsed += UnpairedDeviceUsed;
    }

    Vector3 GetSpawnPoint()
    {
        int index = 0;
        if (m_randomiseSpawnpoint) index = UnityEngine.Random.Range(0, m_remainingSpawnPoints.Count - 1);

        Vector3 spawnPos = m_remainingSpawnPoints[index].position;
        m_remainingSpawnPoints.RemoveAt(index);

        return spawnPos;
    }

    public void UnpairedDeviceUsed(InputControl _inputControl, InputEventPtr _inputEventPtr)
    {
        PlayerInputHandler[] players = FindObjectsOfType<PlayerInputHandler>();

        if (!PairDeviceToAvailablePlayer(players, _inputControl, _inputEventPtr))
        {
            GameObject player = Instantiate(m_playerPrefab);

            Vector3 spawnPos = transform.position;
            if (m_remainingSpawnPoints.Count > 0) spawnPos = GetSpawnPoint();

            player.transform.position = spawnPos;

            player.GetComponent<PlayerInputHandler>().PairDevice(_inputControl, _inputEventPtr);
            //PairDeviceToAvailablePlayer(players, _inputControl, _inputEventPtr);
        }

        //if (!m_firstPlayerSpawned) m_firstPlayerSpawned = true;
        //else
        //{
        //    GameObject player = Instantiate(m_playerPrefab);
        //
        //    Vector3 spawnPos = transform.position;
        //    if (m_remainingSpawnPoints.Count > 0) spawnPos = GetSpawnPoint();
        //
        //    player.transform.position = spawnPos;
        //}
        //
        //foreach (PlayerInputHandler playerInput in players)
        //{
        //    if (playerInput.PairDevice(_inputControl, _inputEventPtr)) return;
        //}
    }

    bool PairDeviceToAvailablePlayer(PlayerInputHandler[] players, InputControl _inputControl, InputEventPtr _inputEventPtr)
    {
        foreach (PlayerInputHandler playerInput in players)
        {
            if (playerInput.PairDevice(_inputControl, _inputEventPtr)) return true;
        }

        return false;
    }
}
