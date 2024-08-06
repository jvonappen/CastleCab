using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
using URNTS;

public class InputManager : MonoBehaviour
{
    [SerializeField] GameObject m_playerPrefab;

    public static Action<PlayerInput, List<PlayerInput>> onPlayerJoined;
    public static Action<PlayerInput, List<PlayerInput>> onPlayerLeft;

    [SerializeField] bool m_randomiseSpawnpoint = true;
    [SerializeField] List<Transform> m_remainingSpawnPoints;

    [SerializeField] TrafficManager m_trafficManager;
    List<PlayerInput> m_players = new();

    [SerializeField] InputActionReference m_inputActionRef;
    InputAction joinAction;
    List<InputDevice> inputDevices = new();
    int joinedCount;

    bool m_canJoin = true;
    public void SetCanJoin(bool _canJoin) => m_canJoin = _canJoin;

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

    private void OnEnable()
    {
        // Bind joinAction to any button press.
        //joinAction =  new InputAction(binding: "/*/<button>");

        // Creates new InputAction with bindings copied from m_inputActionRef
        // (May cause an issue on switch when adding keyboard binding, we will find out when we test on dev kit)
        joinAction = new InputAction();
        foreach (InputBinding binding in m_inputActionRef.action.bindings) joinAction.AddBinding(binding);

        joinAction.started += OnJoinPressed;
        BeginJoining();
    }
    private void OnDisable()
    {
        joinAction.started -= OnJoinPressed;
        EndJoining();
    }

    static bool hasButtonPressedEvent = false;
    private void Awake()
    {
        if (!hasButtonPressedEvent)
        {
            InputSystem.onEvent.ForDevice<Keyboard>().SelectMany(GetPressedControls).Call(ButtonPressed);
            hasButtonPressedEvent = true;
        }
    }
    private IEnumerable<InputControl> GetPressedControls(InputEventPtr eventPtr)
    {
        if (eventPtr.type != StateEvent.Type && eventPtr.type != DeltaStateEvent.Type)
            yield break;
    
        foreach (var control in eventPtr.EnumerateControls(InputControlExtensions.Enumerate.IgnoreControlsInCurrentState))
        {
            if (control.IsPressed())
                continue;
    
            yield return control;
        }
    }

    void ButtonPressed(InputControl _inputControl)
    {
        string keyPressed = _inputControl.path.Replace("/Keyboard/", "");
        if (keyPressed == "rightBracket")
        {
            GameManager.Instance.ResetGame(false);
            StartJoinCooldown(0.1f);
        }
        else if (keyPressed == "leftBracket")
        {
            GameManager.Instance.ResetGame(true, true);
            StartJoinCooldown(0.1f);
        }
        else if (keyPressed == "tab")
        {
            if (SceneManager.GetActiveScene().name == "StartMenu") ReadyUp.StartGame();
            StartJoinCooldown(0.1f);
        }
    }
    void StartJoinCooldown(float _time)
    {
        m_canJoin = false;
        TimerManager.RunAfterTime(() => m_canJoin = true, _time);
    }

    void BeginJoining() => joinAction.Enable();
    void EndJoining() => joinAction.Disable();
    void OnJoinPressed(InputAction.CallbackContext _context)
    {
        Debug.Log("Joined");
        //if (_context.control.device is Keyboard) ButtonPressed(_context.control);
        JoinPlayer(_context.control.device);
    }
    public GameObject JoinPlayer(InputDevice _device)
    {
        if (!m_canJoin) return null;

        if (!(_device is Keyboard || _device is Gamepad)) return null;

        // Ensures the a paired device won't join a new player
        if (inputDevices.Contains(_device))
            return null;
        inputDevices.Add(_device);

        // Pairs to existing player if possible
        PlayerInputHandler[] players = new PlayerInputHandler[m_players.Count];
        for (int i = 0; i < m_players.Count; i++) players[i] = m_players[i].GetComponent<PlayerInputHandler>();
        if (PairDeviceToAvailablePlayer(players, _device, out GameObject existingPlayer)) return existingPlayer;
        
        // Spawns player and pairs input to them
        PlayerInput newPlayer = /*PlayerInput.*/Instantiate(m_playerPrefab).GetComponent<PlayerInput>();
        newPlayer.gameObject.name = "Player " + m_players.Count.ToString();
        newPlayer.GetComponent<PlayerInputHandler>().PairDevice(_device);

        Vector3 spawnPos = transform.position;
        if (m_remainingSpawnPoints.Count > 0) spawnPos = GetSpawnPoint();

        newPlayer.transform.position = spawnPos;

        joinedCount++;

        if (joinedCount == GetComponent<PlayerInputManager>().maxPlayerCount)
            EndJoining();

        return newPlayer.gameObject;
    }

    #region DeprecatedPlayerJoining
    //private void OnEnable() => InputUser.onUnpairedDeviceUsed += UnpairedDeviceUsed;
    //private void OnDisable() => InputUser.onUnpairedDeviceUsed -= UnpairedDeviceUsed;

    //public void UnpairedDeviceUsed(InputControl _inputControl, InputEventPtr _inputEventPtr) => JoinUser(_inputControl.device, _inputEventPtr);
    //public GameObject JoinUser(InputDevice _device, InputEventPtr _eventPtr = new())
    //{
    //    Debug.Log("Unpaired used");
    //    if (_device.name == "Mouse") return null;
    //
    //    if (_device is Keyboard && _eventPtr != new InputEventPtr())
    //    {
    //        Keyboard controls = (Keyboard)_device;
    //        if (controls.backspaceKey.ReadValueFromEvent(_eventPtr) == 1) // Reset button
    //        {
    //            GameManager.Instance.ResetGame(false);
    //            return null;
    //        }
    //        else if (controls.tabKey.ReadValueFromEvent(_eventPtr) == 1) // Force begin-play button
    //        {
    //            if (SceneManager.GetActiveScene().name == "StartMenu")
    //            {
    //                ReadyUp.StartGame();
    //            }
    //            
    //            return null;
    //        }
    //    }
    //
    //    // Disables joining
    //    if (!m_canJoin) return null;
    //
    //    PlayerInputHandler[] players = new PlayerInputHandler[m_players.Count];
    //    for (int i = 0; i < m_players.Count; i++) players[i] = m_players[i].GetComponent<PlayerInputHandler>();
    //
    //    if (PairDeviceToAvailablePlayer(players, _device, out GameObject existingPlayer)) return existingPlayer;
    //    else
    //    {
    //        GameObject newPlayer = Instantiate(m_playerPrefab);
    //        newPlayer.name = "Player " + (players.Length + 1).ToString();
    //
    //        Vector3 spawnPos = transform.position;
    //        if (m_remainingSpawnPoints.Count > 0) spawnPos = GetSpawnPoint();
    //
    //        newPlayer.transform.position = spawnPos;
    //
    //        newPlayer.GetComponent<PlayerInputHandler>().PairDevice(_device);
    //
    //        return newPlayer;
    //    }
    //}
    #endregion

    bool PairDeviceToAvailablePlayer(PlayerInputHandler[] players, InputDevice _device, out GameObject _player)
    {
        foreach (PlayerInputHandler playerInput in players)
        {
            if (playerInput.PairDevice(_device))
            {
                _player = playerInput.gameObject;
                return true;
            }
        }

        _player = null;
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
            GameObject player = GameManager.Instance.players[i].player;
            GameObject customizationMenu = player.GetComponent<CustomisationSpawner>().customizationMenu;
            if (customizationMenu)
            {
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

                customizationMenu.GetComponent<SwitchUI2P>().UpdateUI();
            }
            
        }
    }

    Vector3 GetSpawnPoint()
    {
        int index = 0;
        if (m_randomiseSpawnpoint) index = UnityEngine.Random.Range(0, m_remainingSpawnPoints.Count - 1);

        Vector3 spawnPos;
        if (m_remainingSpawnPoints[index])
        {
            spawnPos = m_remainingSpawnPoints[index].position;
            m_remainingSpawnPoints[index].gameObject.SetActive(false);
            m_remainingSpawnPoints.RemoveAt(index);
        }
        else spawnPos = transform.position;

        return spawnPos;
    }
}
