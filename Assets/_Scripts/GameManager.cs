using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Linq;

public struct CustomizeModelData
{
    #region Constructor
    public CustomizeModelData(int _typeIndex, int _index, Material _mat)
    {
        m_typeIndex = _typeIndex;
        m_index = _index;
        m_mat = _mat;
    }
    #endregion

    #region Variables
    int m_typeIndex;

    int m_index;
    Material m_mat;

    public int typeIndex { get { return m_typeIndex; } }
    public int index { get { return m_index; } }
    public Material mat { get { return m_mat; } }
    #endregion
}

public struct PlayerData
{
    #region Constructor
    public PlayerData(GameObject _obj, InputDevice _device, CustomizeModelData _customizationData)
    {
        m_player = _obj;
        m_device = _device;
        m_customizationData = _customizationData;
    }
    #endregion

    #region Variables
    GameObject m_player;
    InputDevice m_device;
    CustomizeModelData m_customizationData;

    public GameObject player { get { return m_player; } set { m_player = value; } }
    public InputDevice device { get { return m_device; } }
    public CustomizeModelData customizeModelData { get { return m_customizationData; } }
    #endregion
}

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;

    void CreateSingleton()
    {
        if (Instance) Destroy(gameObject);
        else
        {
            Instance = this;

            // Set DontDestroyOnLoad
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    List<PlayerData> m_players = new();
    public List<PlayerData> players { get { return m_players; } }

    public void AddPlayer(GameObject _player)
    {
        InputDevice device = _player.GetComponent<PlayerInput>().devices[0];
        PlayerData data = new(_player, device, new());

        // If no devices in playerData match the new device, it is added as a new player
        if (!m_players.Any(existingData => existingData.device == device)) m_players.Add(data);
    }

    static bool m_isCustomizing;
    static public bool isCustomizing { get { return m_isCustomizing; } }
    static public void SetCustomizing(bool _isCustomizing) => m_isCustomizing = _isCustomizing;


    #region Gold
    [SerializeField] int m_gold;
    public int gold { get { return m_gold; } }
    public void SetGold(int _goldAmount)
    {
        int oldVal = m_gold;
        m_gold = _goldAmount;

        onGoldChanged?.Invoke(oldVal, _goldAmount);
    }

    public void AddGold(int _goldToAdd)
    {
        int oldVal = m_gold;
        m_gold += _goldToAdd;

        onGoldChanged?.Invoke(oldVal, m_gold);
    }

    

    public Action<int, int> onGoldChanged;
    #endregion

    public Color m_affordColour, m_notAffordColour;

    private void OnValidate()
    {
        onGoldChanged?.Invoke(m_gold, m_gold);
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void Awake()
    {
        CreateSingleton();

        onGoldChanged?.Invoke(m_gold, m_gold);
    }

    public void LoadScene(string _sceneName) => SceneManager.LoadScene(_sceneName);

    public void OpenCustomization()
    {
        m_isCustomizing = true;

        foreach (PlayerData data in players) data.player.GetComponent<CustomizationSpawner>().StartCustomization();
        InputManager.EnableSplitscreen();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "StartMenu")
        {
            InputManager inputManager = FindObjectOfType<InputManager>();

            // Pairs users to existing (or new if neccesary) 'PlayerInput's, and replaces player reference
            for (int i = 0; i < m_players.Count; i++)
            {
                GameObject player = inputManager.JoinUser(m_players[i].device);
                m_players[i] = new(player, m_players[i].device, m_players[i].customizeModelData);
            }
        }
        
    }
}
