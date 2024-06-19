using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SetImageToPlayerColour : MonoBehaviour
{
    [SerializeField] PlayerInput m_input;
    [SerializeField] Image m_image;

    [SerializeField] List<Color> m_playerColours;

    private void Awake()
    {
        m_image.color = m_playerColours[m_input.user.index];
    }
}
