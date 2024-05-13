using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReturnToMenu : MonoBehaviour
{
    bool m_hasInitialized;

    PlayerControls m_playerControls;

    [SerializeField] GameObject loadingScreenUI;
    private void Start()
    {
        loadingScreenUI.SetActive(false);

        m_playerControls = GetComponent<PlayerInputHandler>().m_playerControls;
        m_hasInitialized = true;

        m_playerControls.Controls.ReturnMenu.performed += OpenMenu;
    }

    private void OnEnable()
    { 
        if (m_hasInitialized)
        {
            
            m_playerControls.Controls.ReturnMenu.performed += OpenMenu;

          
        }
                     
    }
    private void OnDisable() => m_playerControls.Controls.ReturnMenu.performed -= OpenMenu;

    void OpenMenu(InputAction.CallbackContext context)
    {
        GameManager manager = GameManager.Instance;

        loadingScreenUI.SetActive(true);
        WagonData.playerNumber = 0;
        Debug.Log("player number = " + WagonData.playerNumber);

        manager.ClearPlayers();
        manager.LoadScene("StartMenu");
    }
}
