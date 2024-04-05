using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTweener : MonoBehaviour
{
    [SerializeField] Vector3 m_desiredScale;
    [SerializeField] float m_duration = 0.5f;

    [SerializeField] AnimationCurve m_animationCurve;

    Vector3 m_originalScale;

    private void Awake()
    {
        m_originalScale = transform.localScale;
        TimerManager.RunUntilTime(IncreaseScale, m_duration);
    }

    void IncreaseScale(float _counter, float _time)
    {
        transform.localScale = Vector3.Lerp(m_originalScale, m_desiredScale, m_animationCurve.Evaluate(_counter / _time));
        if (_counter >= _time) TimerManager.RunUntilTime(DecreaseScale, m_duration);
    }

    void DecreaseScale(float _counter, float _time)
    {
        transform.localScale = Vector3.Lerp(m_desiredScale, m_originalScale, m_animationCurve.Evaluate(_counter / _time));
        if (_counter >= _time) TimerManager.RunUntilTime(IncreaseScale, m_duration);
    }
}
