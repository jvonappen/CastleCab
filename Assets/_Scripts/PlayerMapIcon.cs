using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMapIcon : MonoBehaviour
{
    [SerializeField] private GameObject[] PlayerMapIcons;
    private int thisPlayerNumber;

    [SerializeField] PlayerInput m_input;

    private void Start()
    {
        thisPlayerNumber = m_input.user.index;
        PlayerMapIcons[thisPlayerNumber].SetActive(true);
    }
}
