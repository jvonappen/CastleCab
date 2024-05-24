using UnityEngine;
using UnityEngine.InputSystem;

public class ReadyUp : MonoBehaviour
{
    [SerializeField] PlayerInputHandler m_input;
    [SerializeField] SwitchUI2P m_switchUI2p;

    [SerializeField] GameObject m_readyText, m_notReadyText;
    bool m_isReady;

    private void OnEnable()
    {
        m_input.m_playerControls.Controls.Boost.performed += SetReady;
        m_input.m_playerControls.UI.Cancel.performed += CancelReady;
    }
    private void OnDisable()
    {
        m_input.m_playerControls.Controls.Boost.performed -= SetReady;
        m_input.m_playerControls.UI.Cancel.performed -= CancelReady;
    }

    void SetReady(InputAction.CallbackContext context)
    {
        m_notReadyText.SetActive(false);
        m_readyText.SetActive(true);
        SetReady(true);
    }
    void CancelReady(InputAction.CallbackContext context)
    {
        m_readyText.SetActive(false);
        m_notReadyText.SetActive(true);
        SetReady(false);
    }

    void SetReady(bool _isReady)
    {
        m_isReady = _isReady;

        // If a player isn't ready, dont continue through function
        foreach (PlayerCustomization player in FindObjectsOfType<PlayerCustomization>())
        {
            ReadyUp readyUp = player.GetComponent<SwitchUI2P>().currentMenu.GetComponentInChildren<ReadyUp>(true);
            if (!readyUp.m_isReady) return;
        }

        StartGame();
    }

    public void StartGame()
    {
        foreach (PlayerCustomization player in FindObjectsOfType<PlayerCustomization>()) player.StoreCustomizationsToPlayer(true);

        GameManager.Instance.LoadScene(GameManager.Instance.GetComponent<SceneToLoad>().sceneToLoad, true);
    }
}
