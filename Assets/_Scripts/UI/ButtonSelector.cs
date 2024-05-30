using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    [SerializeField] MultiplayerEventSystem m_eventSystem;
    GameObject m_selectedButton;

    List<Image> m_images = new();
    private void Awake()
    {
        foreach (Transform child in transform) m_images.Add(child.GetComponent<Image>());
    }

    public void SelectButtonByIndex(int _index)
    {
        DeselectButtons();
        m_images[_index].enabled = true;

        m_selectedButton = m_images[_index].gameObject;
    }

    void DeselectButtons()
    {
        foreach (Image image in m_images) image.enabled = false;
    }

    public void HoverSelected() { if (m_selectedButton && m_eventSystem) m_eventSystem.SetSelectedGameObject(m_selectedButton); }
}
