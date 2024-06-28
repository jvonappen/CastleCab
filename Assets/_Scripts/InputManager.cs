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
    [SerializeField] GameObject m_menuPlayerPrefab, m_playerPrefab;
    GameObject GetPlayerPrefab()
    {
        if (SceneManager.GetActiveScene().name == "StartMenu") return m_menuPlayerPrefab;
        else return m_playerPrefab;
    }

    public static Action<PlayerInput, List<PlayerInput>> onPlayerJoined;
    public static Action<PlayerInput, List<PlayerInput>> onPlayerLeft;

    [SerializeField] bool m_randomiseSpawnpoint = true;
    [SerializeField] List<Transform> m_remainingSpawnPoints;
    Spawnpoints m_spawnpoints;

    /*[SerializeField] */TrafficManager m_trafficManager;
    List<PlayerInput> m_players = new();

    InputAction joinAction;
    List<InputDevice> inputDevices = new();
    public void ClearInputDevices() => inputDevices.Clear();
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

    void OnSceneLoaded(Scene _scene, LoadSceneMode _loadSceneMode)
    {
        m_trafficManager = FindObjectOfType<TrafficManager>();
        m_spawnpoints = FindObjectOfType<Spawnpoints>();

        TimerManager.RunAfterTime(() =>
        {
            if (_scene.name != "StartMenu") EnableSplitscreen();
        }, 0.1f);

        ClearInputDevices();
        GameManager manager = GameManager.Instance;
        if (manager)
        {
            foreach (PlayerData data in manager.players)
            {
                inputDevices.Add(data.device);
            }
        }
        //ClearInputDevices();

        m_canJoin = true;
    }

    //private void OnEnable()
    //{
    //    // Bind joinAction to any button press.
    //    joinAction = new InputAction(binding: "/*/<button>");
    //
    //    joinAction.started += OnJoinPressed;
    //    BeginJoining();
    //}
    //private void OnDisable()
    //{
    //    joinAction.started -= OnJoinPressed;
    //    EndJoining();
    //}

    public static InputManager Instance;

    //static bool hasButtonPressedEvent = false;
    private void Awake()
    {
        if (Instance) Destroy(gameObject);
        else 
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);

            m_trafficManager = FindObjectOfType<TrafficManager>();
            m_spawnpoints = FindObjectOfType<Spawnpoints>();
            SceneManager.sceneLoaded += OnSceneLoaded;

            InputSystem.onEvent/*.ForDevice<Keyboard>()*/.SelectMany(GetPressedControls).Call(ButtonPressed);
        }

        //if (!hasButtonPressedEvent)
        //{
        //    InputSystem.onEvent/*.ForDevice<Keyboard>()*/.SelectMany(GetPressedControls).Call(ButtonPressed);
        //    hasButtonPressedEvent = true;
        //}
        
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

    public void TempDisableJoining()
    {
        m_canJoin = false;
        //TimerManager.RunAfterTime(() => { m_canJoin = true; Debug.Log("Can join"); }, 0.1f);
    }

    void ButtonPressed(InputControl _inputControl)
    {
        if (_inputControl.device is Keyboard)
        {
            string keyPressed = _inputControl.path.Replace("/Keyboard/", "");
            if (keyPressed == "backspace")
            {
                GameManager.Instance.ResetGame(false);
                Debug.Log("Reset");
                //m_canJoin = false;
                //TimerManager.RunAfterTime(() => { m_canJoin = true; Debug.Log("Can join"); }, 0.1f);
            }
            else if (keyPressed == "tab")
            {
                if (SceneManager.GetActiveScene().name == "StartMenu")
                {
                    ReadyUp.StartGame();
                }

                m_canJoin = false;
                TimerManager.RunAfterTime(() => m_canJoin = true, 0.1f);
            }
            else JoinPlayer(_inputControl.device);
        }
        else if (_inputControl.device is Gamepad)
        {
            if (!_inputControl.path.Contains("Stick"))
            {
                Debug.Log(_inputControl.path);
                JoinPlayer(_inputControl.device);
            }
        }
    }

    //void BeginJoining() => joinAction.Enable();
    //void EndJoining() => joinAction.Disable();
    //void OnJoinPressed(InputAction.CallbackContext _context)
    //{
    //    //if (_context.control.device is Keyboard) ButtonPressed(_context.control);
    //    JoinPlayer(_context.control.device);
    //    Debug.Log(_context.control.path);
    //}
    public GameObject JoinPlayer(InputDevice _device)
    {
        Debug.Log("Player attempt join | can join: " + m_canJoin);
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
        PlayerInput newPlayer = /*PlayerInput.*/Instantiate(GetPlayerPrefab()).GetComponent<PlayerInput>();
        newPlayer.gameObject.name = "Player " + m_players.Count.ToString();
        newPlayer.GetComponent<PlayerInputHandler>().PairDevice(_device);

        Vector3 spawnPos = transform.position;
        if (m_spawnpoints && m_spawnpoints.remainingSpawnpoints /*m_remainingSpawnPoints*/.Count > 0) spawnPos = GetSpawnPoint();

        newPlayer.transform.position = spawnPos;

        joinedCount++;

        //if (joinedCount == GetComponent<PlayerInputManager>().maxPlayerCount) EndJoining();

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
        if (/*m_randomiseSpawnpoint*/m_spawnpoints.randomiseSpawnpoint) index = UnityEngine.Random.Range(0, m_spawnpoints.remainingSpawnpoints/*m_remainingSpawnPoints*/.Count - 1);

        Vector3 spawnPos;
        if (/*m_remainingSpawnPoints*/m_spawnpoints.remainingSpawnpoints[index])
        {
            spawnPos = /*m_remainingSpawnPoints*/m_spawnpoints.remainingSpawnpoints[index].position;
            /*m_remainingSpawnPoints*/ m_spawnpoints.remainingSpawnpoints[index].gameObject.SetActive(false);
            /*m_remainingSpawnPoints*/ m_spawnpoints.remainingSpawnpoints.RemoveAt(index);
        }
        else spawnPos = transform.position;

        return spawnPos;
    }
}
