using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
   
    private void Update()
    { //------------Temp restart Solution
        if (Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            ResetScene();
        }
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            QuitGame();
        }
    }
    public void ResetScene()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Debug.Log("Game Closed");
        Application.Quit();
    }
}
