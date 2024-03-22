using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] RectTransform m_fill;
    [SerializeField][Range(0, 1)] float m_progress = 0;
    public float progress { get { return m_progress; } set { m_progress = value; } }

    private void OnValidate() => UpdateProgress();
    public void UpdateProgress()
    {
        if (m_progress < 0) m_progress = 0;
        m_fill.localScale = new Vector3(m_progress, m_fill.localScale.y, m_fill.localScale.z);
    }
}
