using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReadyUp : MonoBehaviour
{
    [SerializeField] PlayerInputHandler m_input;
    //[SerializeField] string m_sceneToLoad;

    private void OnEnable() => m_input.m_playerControls.Controls.Boost.performed += StartGame;
    private void OnDisable() => m_input.m_playerControls.Controls.Boost.performed -= StartGame;

    void StartGame(InputAction.CallbackContext context)
    {
        GameManager.Instance.LoadScene(GameManager.Instance.GetComponent<SceneToLoad>().sceneToLoad);
        //GameManager.Instance.LoadScene(m_sceneToLoad);
    }
}
