using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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

    List<PlayerData> m_playerData = new();

    [SerializeField] List<GameObject> m_players;
    List<InputDevice> m_devices = new();
    public List<GameObject> players { get { return m_players; } }
    public void AddPlayer(GameObject _player)
    {
        m_players.Add(_player);

        InputDevice device = _player.GetComponent<PlayerInput>().devices[0];
        if (!m_devices.Contains(device)) m_devices.Add(device);

        m_playerData.Add(new(_player, device, new()) );
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

        foreach (GameObject player in players) player.GetComponent<CustomizationSpawner>().StartCustomization();
        InputManager.EnableSplitscreen();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "StartMenu")
        {
            InputManager inputManager = FindObjectOfType<InputManager>();

            foreach (InputDevice device in m_devices) inputManager.JoinUser(device);
        }
        
    }
}
