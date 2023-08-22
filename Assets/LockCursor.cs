using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCursor : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            if (Cursor.lockState != CursorLockMode.Locked) HelperSwitchCursor();

        if (Input.GetKey(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
            if (Cursor.lockState == CursorLockMode.Locked) HelperSwitchCursor();
    }

    private void HelperSwitchCursor()
    {
        if (Cursor.visible)
        {
            if (Application.isFocused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
