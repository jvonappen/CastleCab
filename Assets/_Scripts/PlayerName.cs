using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TextMeshPro))]
public class PlayerName : MonoBehaviour
{
    [SerializeField] PlayerInput m_input;
    TextMeshPro m_display;

    [SerializeField] List<Color> m_playerColours;

    int m_playerNumber;
    string m_playerName;

    private void Awake()
    {
        m_display = GetComponent<TextMeshPro>();

        m_playerNumber = m_input.user.index + 1;
        m_playerName = "P" + m_playerNumber;

        m_display.color = m_playerColours[m_playerNumber - 1];
        m_display.text = m_playerName;
    }
}
