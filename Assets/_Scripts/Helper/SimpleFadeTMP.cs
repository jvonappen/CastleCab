using TMPro;
using UnityEngine;

public enum FadeEndAction
{
    None,
    Destroy,
    Inactive,
}

[RequireComponent(typeof(TextMeshPro))]
public class SimpleFadeTMP : MonoBehaviour
{
    [SerializeField] float m_timeToFade = 2;
    [SerializeField] FadeEndAction m_fadeEndAction;

    TextMeshPro m_display;
    private void Awake() { m_display = GetComponent<TextMeshPro>(); }

    int m_updateCounter = 0;
    float m_totalFrameTime;
    private void Update()
    {
        m_totalFrameTime += Time.deltaTime;

        if (m_updateCounter >= 7)
        {
            if (m_display.color.a > 0)
            {
                m_display.color = new Color(m_display.color.r, m_display.color.g, m_display.color.b, m_display.color.a - m_totalFrameTime / m_timeToFade);
            }
            else
            {
                if (m_fadeEndAction == FadeEndAction.Destroy) Destroy(gameObject);
                else if (m_fadeEndAction == FadeEndAction.Inactive) gameObject.SetActive(false);
            }

            m_totalFrameTime = 0;
            m_updateCounter = 0;
        }
        else m_updateCounter++;
    }

    public static void Begin(GameObject _go, float _timeToFade, FadeEndAction _fadeEndAction)
    {
        SimpleFadeTMP simpleFade;
        if (!_go.TryGetComponent(out simpleFade)) simpleFade = _go.AddComponent<SimpleFadeTMP>();

        simpleFade.m_timeToFade = _timeToFade;
        simpleFade.m_fadeEndAction = _fadeEndAction;
    }
}