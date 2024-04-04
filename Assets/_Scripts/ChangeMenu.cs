using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

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

    private void Start()
    {
        m_playerInput.m_playerControls.UI.Next.performed += Next;
        m_playerInput.m_playerControls.UI.Previous.performed += Previous;

        m_eventSystem = m_playerInput.playerInput.uiInputModule.GetComponent<MultiplayerEventSystem>();
    }

    private void OnEnable()
    {
        m_playerInput.m_playerControls.UI.Next.performed += Next;
        m_playerInput.m_playerControls.UI.Previous.performed += Previous;
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
        }
    }

    void Previous(InputAction.CallbackContext context)
    {
        if (m_previousMenu)
        {
            m_previousMenu.SetActive(true);

            m_eventSystem.SetSelectedGameObject(m_buttonToSelectPrevious);

            gameObject.SetActive(false);
        }
    }
}
