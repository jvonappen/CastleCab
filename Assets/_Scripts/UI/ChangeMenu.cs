using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

[Serializable]
public struct MenuEvent
{
    public UnityEvent onPreviousMenu;
    public UnityEvent onNextMenu;
}

public class ChangeMenu : MonoBehaviour
{
    [SerializeField] PlayerInputHandler m_playerInput;
    MultiplayerEventSystem m_eventSystem;

    [Space(10)]
    [SerializeField] GameObject m_nextMenu;
    [SerializeField] GameObject m_buttonToSelectNext;
    
    [Space(10)]
    [SerializeField] GameObject m_previousMenu;
    [SerializeField] GameObject m_buttonToSelectPrevious;

    [Space(10)]
    [SerializeField] MenuEvent m_events;

    public UnityEvent onPreviousMenu { get { return m_events.onPreviousMenu; } }
    public UnityEvent onNextMenu { get { return m_events.onNextMenu; } }

    private void Start()
    {
        m_eventSystem = m_playerInput.playerInput.uiInputModule.GetComponent<MultiplayerEventSystem>();
    }

    private void OnEnable()
    {
        if (m_playerInput.m_playerControls != null)
        {
            m_playerInput.m_playerControls.UI.Next.performed += Next;
            m_playerInput.m_playerControls.UI.Previous.performed += Previous;
        }
    }

    private void OnDisable()
    {
        m_playerInput.m_playerControls.UI.Next.performed -= Next;
        m_playerInput.m_playerControls.UI.Previous.performed -= Previous;
    }

    void Next(InputAction.CallbackContext context)
    {
        if (m_nextMenu)
        {
            m_nextMenu.SetActive(true);

            m_eventSystem.SetSelectedGameObject(m_buttonToSelectNext);

            gameObject.SetActive(false);

            onNextMenu?.Invoke();
        }
    }

    void Previous(InputAction.CallbackContext context)
    {
        if (m_previousMenu)
        {
            m_previousMenu.SetActive(true);

            m_eventSystem.SetSelectedGameObject(m_buttonToSelectPrevious);

            gameObject.SetActive(false);

            onPreviousMenu?.Invoke();
        }
    }
}
