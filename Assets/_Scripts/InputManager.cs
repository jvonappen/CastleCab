using System;
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
        }
    }

    bool PairDeviceToAvailablePlayer(PlayerInputHandler[] players, InputControl _inputControl, InputEventPtr _inputEventPtr)
    {
        foreach (PlayerInputHandler playerInput in players)
        {
            if (playerInput.PairDevice(_inputControl, _inputEventPtr)) return true;
        }

        return false;
    }

    public static void SwitchPlayerInput(PlayerInput _currentInput, PlayerInput _newInput)
    {
        // Gets current device and user
        InputDevice device = _currentInput.devices[0];

        // Disables old input and enables new one
        _currentInput.enabled = false;
        _newInput.enabled = true;

        // Pairs device and user to new input
        _newInput.SwitchCurrentControlScheme(device);
    }

    public static void EnableSplitscreen()
    {
        PlayerInputManager manager = PlayerInputManager.instance;
        manager.splitScreen = true;

        for (int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            GameObject player = GameManager.Instance.players[i];
            GameObject customizationMenu = player.GetComponent<CustomizationSpawner>().customizationMenu;
            Camera cam = customizationMenu.GetComponentInChildren<Camera>();

            int playerIndex = customizationMenu.GetComponent<PlayerInput>().user.index + 1;

            #region Cam size & pos
            Vector2 size = Vector2.one;
            if (manager.playerCount == 2) size = new(0.5f, 1);
            else if (manager.playerCount == 3 || manager.playerCount == 4) size = new(0.5f, 0.5f);

            Vector2 pos = Vector2.zero;
            if (manager.playerCount > 2 && playerIndex == 1) pos = new(0, 0.5f);
            else if (playerIndex == 2)
            {
                if (manager.playerCount == 2) pos = new(0.5f, 0);
                else pos = new(0.5f, 0.5f);
            }
            else if (playerIndex == 4) pos = new(0.5f, 0);
            #endregion

            cam.rect = new(pos.x, pos.y, size.x, size.y);

            customizationMenu.GetComponent<SwitchUI2P>().OnPlayerUpdated();
        }
    }
}
