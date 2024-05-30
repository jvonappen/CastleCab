using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
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

        if (m_playerInput.m_playerControls != null)
        {
            m_playerInput.m_playerControls.UI.Next.performed += Next;
            m_playerInput.m_playerControls.UI.Previous.performed += Previous;
        }

        onSetMenu?.Invoke();
    }

    private void OnDisable()
    {
        m_playerInput.m_playerControls.UI.Next.performed -= Next;
        m_playerInput.m_playerControls.UI.Previous.performed -= Previous;
    }

    public void SetMenu()
    {
        m_playerInput.GetComponent<PlayerCustomization>().StoreCustomizationsToPlayer(true);

        gameObject.SetActive(true);
        m_eventSystem.SetSelectedGameObject(m_buttonToSelect);

        onSetMenu?.Invoke();
    }

    void Next(InputAction.CallbackContext context) => NextMenu();
    public void NextMenu()
    {
        if (m_nextMenu)
        {
            m_nextMenu.SetMenu();
            gameObject.SetActive(false);
        }
    }

    void Previous(InputAction.CallbackContext context) => PreviousMenu();
    public void PreviousMenu()
    {
        if (m_previousMenu)
        {
            m_previousMenu.SetMenu();
            gameObject.SetActive(false);
        }
    }
}
