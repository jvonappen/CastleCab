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

    //[SerializeField] // Temp
    //[Space(10)]
    //GameObject m_currentSelectedButton;

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
        if (!m_2pUI.activeSelf)
        {
            m_2pUI.SetActive(true);

            // Translate menu state to other canvas
            m_defaultUI.GetComponent<MaintainOpenMenu>().SwitchMenus();
            if (!SelectButtonTwin()) m_multiplayerEventSystem.SetSelectedGameObject(m_2pSelectedButton);

            m_defaultUI.SetActive(false);

            m_2pCam.rect = m_playerInput.camera.rect;
            m_playerInput.camera = m_2pCam;
        }
    }

    void SetDefault()
    {
        if (!m_defaultUI.activeSelf)
        {
            m_defaultUI.SetActive(true);

            // Translate menu state to other canvas
            m_2pUI.GetComponent<MaintainOpenMenu>().SwitchMenus();
            if (!SelectButtonTwin()) m_multiplayerEventSystem.SetSelectedGameObject(m_defaultSelectedButton);

            m_2pUI.SetActive(false);

            m_defaultCam.rect = m_playerInput.camera.rect;
            m_playerInput.camera = m_defaultCam;
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
            else Debug.LogWarning("Twin button not set");
        }

        return false;
    }
}
