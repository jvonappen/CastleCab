using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Linq;

public struct MaterialInformation
{
    public MaterialInformation(SO_Dye _main, SO_Dye _secondary, SO_Dye _tertiary)
    {
        m_mainDye = _main;
        m_secondaryDye = _secondary;
        m_tertiaryDye = _tertiary;
    }

    SO_Dye m_mainDye, m_secondaryDye, m_tertiaryDye;

    public SO_Dye mainDye { get { return m_mainDye; } }
    public SO_Dye secondaryDye { get { return m_secondaryDye; } }
    public SO_Dye tertiaryDye { get { return m_tertiaryDye; } }
}

public struct ModelCustomization
{
    #region Constructor
    public ModelCustomization(int _typeIndex, int _index, MaterialInformation _mat)
    {
        m_typeIndex = _typeIndex;
        m_index = _index;
        m_mat = _mat;
    }

    public ModelCustomization(ModelSelector _selector)
    {
        m_typeIndex = _selector.m_typeIndex;
        m_index = _selector.GetSelectedIndex();

        Material material = _selector.GetMat();
        if (material)
        {
            SO_Dye mainDye = _selector.colourSelector.mainDye;
            SO_Dye secondaryDye = _selector.colourSelector.secondaryDye;
            SO_Dye tertiaryDye = _selector.colourSelector.tertiaryDye;

            m_mat = new(mainDye, secondaryDye, tertiaryDye);
        }
        else m_mat = new();
    }
    #endregion

    #region Variables
    int m_typeIndex;

    int m_index;
    MaterialInformation m_mat;

    public int typeIndex { get { return m_typeIndex; } }
    public int index { get { return m_index; } }
    public MaterialInformation mat { get { return m_mat; } }
    #endregion
}

public struct PlayerData
{
    #region Constructor
    public PlayerData(GameObject _obj, InputDevice _device, List<ModelCustomization> _modelCustomizations)
    {
        m_player = _obj;
        m_device = _device;
        m_modelCustomizations = _modelCustomizations;
    }
    #endregion

    #region Variables
    GameObject m_player;
    InputDevice m_device;
    List<ModelCustomization> m_modelCustomizations;

    public GameObject player { get { return m_player; } set { m_player = value; } }
    public InputDevice device { get { return m_device; } }
    public List<ModelCustomization> modelCustomizations { get { return m_modelCustomizations; } }
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
        PlayerInput playerInput = _player.GetComponent<PlayerInput>();
        InputDevice device = playerInput.devices[0];

        PlayerData data = new(_player, device, new());

        // If no devices in playerData match the new device, it is added as a new player
        if (!m_players.Any(existingData => existingData.device == device)) m_players.Add(data);
    }
    public PlayerData GetPlayerData(InputDevice _device) => m_players.FirstOrDefault(item => item.device == _device);
    public void SetPlayerData(InputDevice _device, PlayerData _data) => m_players[m_players.FindIndex(item => item.device == _device)] = _data;

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

                m_players[i] = new(player, m_players[i].device, m_players[i].modelCustomizations);

                foreach (ModelSelector modelSelector in player.GetComponentsInChildren<ModelSelector>())
                {
                    ModelCustomization foundItem = m_players[i].modelCustomizations.FirstOrDefault(item => item.typeIndex == modelSelector.m_typeIndex);
                    modelSelector.SelectObjectByIndex(foundItem.index);

                    modelSelector.colourSelector.SetMainDye(foundItem.mat.mainDye);
                    modelSelector.colourSelector.SetSecondaryDye(foundItem.mat.secondaryDye);
                    modelSelector.colourSelector.SetTertiaryDye(foundItem.mat.tertiaryDye);
                }
            }
        }
        
    }
}
