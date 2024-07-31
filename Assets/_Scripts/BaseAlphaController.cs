using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BaseAlphaController : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] float m_alpha = 1;

    [Header("Debug")]
    public bool updateButton;
    public List<TextMeshProUGUI> m_tmpRenderers = new();
    public List<Image> m_images = new();

    private void OnValidate() => UpdateRenderers();
    private void Awake() => UpdateRenderers();

    void UpdateRenderers()
    {
        updateButton = false;
        m_tmpRenderers = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        m_images = GetComponentsInChildren<Image>().ToList();

        UpdateAlpha();
    }

    public void SetAlpha(float _alpha)
    {
        m_alpha = _alpha;
        UpdateAlpha();
    }

    public void UpdateAlpha()
    {
        for (int i = 0; i < m_tmpRenderers.Count; i++)
        {
            m_tmpRenderers[i].alpha = m_alpha;
        }

        for (int i = 0; i < m_images.Count; i++)
        {
            m_images[i].color = new Color(m_images[i].color.r, m_images[i].color.g, m_images[i].color.b, m_alpha);
        }
    }

    public void FadeOut(float _duration)
    {
        TimerManager.RunUntilTime((float counter, float duration) =>
        {
            SetAlpha(1 - (counter/duration));
            if (counter >= duration) SetAlpha(0);
        }, _duration);
    }
}
