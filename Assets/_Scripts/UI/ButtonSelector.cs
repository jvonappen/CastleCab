using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    List<Image> m_images = new();
    private void Awake()
    {
        foreach (Transform child in transform) m_images.Add(child.GetComponent<Image>());
    }

    public void SelectButtonByIndex(int _index)
    {
        DeselectButtons();
        m_images[_index].enabled = true;
    }

    void DeselectButtons()
    {
        foreach (Image image in m_images) image.enabled = false;
    }
}
