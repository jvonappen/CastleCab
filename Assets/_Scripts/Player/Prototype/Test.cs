using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    [SerializeField] Transform m_test;

    PlayerInput m_playerInput;

    private void Start()
    {
        m_playerInput = GetComponent<PlayerInput>();

        m_playerInput.m_playerControls.Controls.Boost.performed += Test1;
    }

    void Test1(InputAction.CallbackContext context)
    {
        m_test.eulerAngles += new Vector3(0, 75, 0);
    }
}
