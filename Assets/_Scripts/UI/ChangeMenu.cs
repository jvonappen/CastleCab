using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.UI;

public class ChangeMenu : MonoBehaviour
{
    [SerializeField] PlayerInputHandler m_playerInput;
    MultiplayerEventSystem m_eventSystem;

    [SerializeField] ChangeMenu m_nextMenu, m_previousMenu;
    [SerializeField] GameObject m_buttonToSelect;

    public UnityEvent onSetMenu;


    private void OnEnable()
    {
        m_eventSystem ??= m_playerInput.playerInput.uiInputModule.GetComponent<MultiplayerEventSystem>();

        onSetMenu?.Invoke();
    }

    public void SetMenu()
    {
        m_playerInput.GetComponent<PlayerCustomization>().StoreCustomizationsToPlayer(true);

        gameObject.SetActive(true);
        m_eventSystem.SetSelectedGameObject(m_buttonToSelect);
        TimerManager.RunAfterTime(() => m_eventSystem.SetSelectedGameObject(m_buttonToSelect), 0.1f);

        onSetMenu?.Invoke();
    }

    public void NextMenu()
    {
        if (m_nextMenu)
        {
            m_nextMenu.SetMenu();
            gameObject.SetActive(false);
        }
    }

    public void PreviousMenu()
    {
        if (m_previousMenu)
        {
            m_previousMenu.SetMenu();
            gameObject.SetActive(false);
        }
    }
}
