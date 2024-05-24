using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SwitchUI2P : PlayerJoinedNotifier
{
    [SerializeField] GameObject m_defaultUI, m_2pUI;
    GameObject m_openMenu;
    public GameObject currentMenu { get { return m_openMenu; } }

    [SerializeField] float m_defaultCamSize, m_2pCamSize;

    [SerializeField] GameObject m_defaultSelectedButton, m_2pSelectedButton;

    [SerializeField] MultiplayerEventSystem m_multiplayerEventSystem;

    PlayerInput m_playerInput;

    public void Start()
    {
        UpdateUI();

        // Selects default button when first opened menu
        if (m_defaultUI) m_multiplayerEventSystem.SetSelectedGameObject(m_defaultSelectedButton);
        else m_multiplayerEventSystem.SetSelectedGameObject(m_2pSelectedButton);
    }

    public void OpenDefaultMenu()
    {
        UpdateUI();
        m_openMenu.GetComponent<MaintainOpenMenu>().OpenMenu();
    }

    public override void Awake()
    {
        base.Awake();
        m_playerInput = GetComponent<PlayerInput>();
    }

    public override void OnPlayerUpdated()
    {
        base.OnPlayerUpdated();
        UpdateUI(true);
    }

    public void UpdateUI(bool _isPlayerBehind = false)
    {
        if (GameManager.Instance)
        {
            int num = 2;
            if (_isPlayerBehind) num -= 1;

            if (GameManager.Instance.players.Count == num) Set2Player();
            else SetDefault();
        }
    }

    void Set2Player()
    {
        if (m_2pUI)
        {
            m_2pUI.SetActive(true);

            // Translate menu state to other canvas
            m_defaultUI.GetComponent<MaintainOpenMenu>().SwitchMenus();
            if (!SelectButtonTwin()) m_multiplayerEventSystem.SetSelectedGameObject(m_2pSelectedButton);

            m_defaultUI.SetActive(false);

            m_playerInput.camera.orthographicSize = m_2pCamSize;

            m_openMenu = m_2pUI;
        }
    }

    void SetDefault()
    {
        if (m_defaultUI)
        {
            m_defaultUI.SetActive(true);

            // Translate menu state to other canvas
            m_2pUI.GetComponent<MaintainOpenMenu>().SwitchMenus();
            if (!SelectButtonTwin()) m_multiplayerEventSystem.SetSelectedGameObject(m_defaultSelectedButton);

            m_2pUI.SetActive(false);

            m_playerInput.camera.orthographicSize = m_defaultCamSize;

            m_openMenu = m_defaultUI;
        }
    }

    bool SelectButtonTwin()
    {
        GameObject selectedObj = m_multiplayerEventSystem.currentSelectedGameObject;
        if (selectedObj && selectedObj.TryGetComponent(out ObjectTwin objTwinComponent))
        {
            m_multiplayerEventSystem.SetSelectedGameObject(null);

            GameObject twinButton = objTwinComponent.m_twin;
            if (twinButton)
            {
                m_multiplayerEventSystem.SetSelectedGameObject(twinButton);
                return true;
            }
            //else Debug.LogWarning("Twin button not set");
        }

        return false;
    }
}
