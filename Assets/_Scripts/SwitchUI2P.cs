using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SwitchUI2P : PlayerJoinedNotifier
{
    [SerializeField] GameObject m_defaultUI, m_2pUI;
    [SerializeField] Camera m_defaultCam, m_2pCam;

    [SerializeField] GameObject m_defaultSelectedButton, m_2pSelectedButton;

    [SerializeField] MultiplayerEventSystem m_multiplayerEventSystem;

    PlayerInput m_playerInput;

    public override void Start()
    {
        base.Start();
        UpdateUI(true);
    }

    public override void Awake()
    {
        base.Awake();
        m_playerInput = GetComponent<PlayerInput>();
    }

    public override void OnPlayerUpdated()
    {
        base.OnPlayerUpdated();
        UpdateUI();
    }

    void UpdateUI(bool _isStart = false)
    {
        if (GameManager.Instance)
        {
            int num = 2;
            if (!_isStart) num -= 1;

            if (GameManager.Instance.players.Count == num) Set2Player();
            else SetDefault();
        }
    }

    void Set2Player()
    {
        m_2pUI.SetActive(true);
        m_defaultUI.SetActive(false);

        m_2pCam.rect = m_playerInput.camera.rect;
        m_playerInput.camera = m_2pCam;

        m_multiplayerEventSystem.playerRoot = m_2pUI;
        m_multiplayerEventSystem.SetSelectedGameObject(m_2pSelectedButton);
        
    }

    void SetDefault()
    {
        m_defaultUI.SetActive(true);
        m_2pUI.SetActive(false);

        m_defaultCam.rect = m_playerInput.camera.rect;
        m_playerInput.camera = m_defaultCam;

        m_multiplayerEventSystem.playerRoot = m_defaultUI;
        m_multiplayerEventSystem.SetSelectedGameObject(m_defaultSelectedButton);
    }
}
