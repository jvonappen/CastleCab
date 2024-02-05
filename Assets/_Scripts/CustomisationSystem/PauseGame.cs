using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private int _manager = 0;

    public void ManageGameState()
    {
        if (_manager == 0)
        {
            Pause();
            _manager = 1;
        }
        else
        {
            Continue();
            _manager = 0;
        }
    }

    private void Pause()
    {
        Time.timeScale = 0;
    }

    private void Continue()
    {
        Time.timeScale = 1;
    }
}
