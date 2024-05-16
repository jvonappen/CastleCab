using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Linq;

#region Material structs
public struct MaterialInformation
{
    public MaterialInformation(DyeData _main, DyeData _secondary, DyeData _tertiary)
    {
        m_mainDye = _main;
        m_secondaryDye = _secondary;
        m_tertiaryDye = _tertiary;
    }

    DyeData m_mainDye, m_secondaryDye, m_tertiaryDye;

    public DyeData mainDye { get { return m_mainDye; } }
    public DyeData secondaryDye { get { return m_secondaryDye; } }
    public DyeData tertiaryDye { get { return m_tertiaryDye; } }
}

public struct HorseMatInformation
{
    public HorseMatInformation(DyeData _base, DyeData _hair, DyeData _tail, DyeData _nose, DyeData _feet, DyeData _pattern, Texture2D _patternStyle, SkinData _skinData)
    {
        m_baseDye = _base;
        m_hairDye = _hair;
        m_tailDye = _tail;
        m_noseDye = _nose;
        m_feetDye = _feet;
        m_patternDye = _pattern;
        m_pattern = _patternStyle;
        m_skinData = _skinData;
    }

    DyeData m_baseDye, m_hairDye, m_tailDye, m_noseDye, m_feetDye, m_patternDye;
    Texture2D m_pattern; 
    SkinData m_skinData;
    
    public DyeData baseDye { get { return m_baseDye; } }
    public DyeData hairDye { get { return m_hairDye; } }
    public DyeData tailDye { get { return m_tailDye; } }
    public DyeData noseDye { get { return m_noseDye; } }
    public DyeData feetDye { get { return m_feetDye; } }
    public DyeData patternDye { get { return m_patternDye; } }
    public Texture2D pattern { get { return m_pattern; } }
    public SkinData skinData { get { return m_skinData; } }
}
#endregion

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
            DyeData mainDye = _selector.colourSelector.GetDye("Main");
            DyeData secondaryDye = _selector.colourSelector.GetDye("Secondary");
            DyeData tertiaryDye = _selector.colourSelector.GetDye("Tertiary");

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
    public PlayerData(GameObject _obj, InputDevice _device, List<ModelCustomization> _modelCustomizations, HorseMatInformation _horseMat)
    {
        m_player = _obj;
        m_device = _device;
        m_modelCustomizations = _modelCustomizations;
        m_horseMat = _horseMat;
    }
    #endregion

    #region Variables
    GameObject m_player;
    InputDevice m_device;
    List<ModelCustomization> m_modelCustomizations;
    HorseMatInformation m_horseMat;

    public GameObject player { get { return m_player; } set { m_player = value; } }
    public InputDevice device { get { return m_device; } }
    public List<ModelCustomization> modelCustomizations { get { return m_modelCustomizations; } }
    public HorseMatInformation horseMat { get { return m_horseMat; } }
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
    public void ClearPlayers() => m_players.Clear();

    public void AddPlayer(GameObject _player)
    {
        PlayerInput playerInput = _player.GetComponent<PlayerInput>();
        InputDevice device = playerInput.devices[0];

        PlayerData data = new(_player, device, new(), new());

        // If no devices in playerData match the new device, it is added as a new player
        if (!m_players.Any(existingData => existingData.device == device)) m_players.Add(data);
    }
    public PlayerData GetPlayerData(InputDevice _device) => m_players.FirstOrDefault(item => item.device == _device);
    public void SetPlayerData(InputDevice _device, PlayerData _data) => m_players[m_players.FindIndex(item => item.device == _device)] = _data;

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
        FindObjectOfType<PlayerInputManager>().DisableJoining();

        foreach (PlayerData data in players) data.player.GetComponent<CustomisationSpawner>().StartCustomization();
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

                m_players[i] = new(player, m_players[i].device, m_players[i].modelCustomizations, m_players[i].horseMat);

                foreach (ModelSelector modelSelector in player.GetComponentsInChildren<ModelSelector>())
                {
                    ModelCustomization foundItem = m_players[i].modelCustomizations.FirstOrDefault(item => item.typeIndex == modelSelector.m_typeIndex);
                    modelSelector.PreviewObjectByIndex(foundItem.index);

                    if (foundItem.mat.mainDye.colour != null) modelSelector.colourSelector.SetDye("Main", foundItem.mat.mainDye);
                    if (foundItem.mat.secondaryDye.colour != null) modelSelector.colourSelector.SetDye("Secondary", foundItem.mat.secondaryDye);
                    if (foundItem.mat.tertiaryDye.colour != null) modelSelector.colourSelector.SetDye("Tertiary", foundItem.mat.tertiaryDye);
                }

                player.GetComponentInChildren<HorseColourSelector>().SetDyes(m_players[i].horseMat);
            }
        }
        
    }
}
