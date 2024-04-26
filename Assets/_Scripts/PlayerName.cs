using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class PlayerName : MonoBehaviour
{
    PlayerInputHandler m_input;
    TextMeshPro m_display;

    [SerializeField] List<Color> m_playerColours;

    int m_playerNumber;
    string m_playerName;

    private void Start()
    {
        m_input = GetComponentInParent<PlayerInputHandler>();
        m_display = GetComponent<TextMeshPro>();

        m_playerNumber = m_input.playerInput.user.index + 1;
        m_playerName = "P" + m_playerNumber;

        m_display.color = m_playerColours[m_playerNumber - 1];
        m_display.text = m_playerName;
    }
}
