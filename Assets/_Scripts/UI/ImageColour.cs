using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageColour : MonoBehaviour
{
    Image m_image;
    private void Awake() => m_image = GetComponent<Image>();

    public void SetAlpha(float _value) => m_image.color = new(m_image.color.r, m_image.color.g, m_image.color.b, _value);
}
