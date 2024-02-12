using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] RectTransform m_fill;
    [SerializeField][Range(0, 1)] float m_progress = 0;

    private void OnValidate() => UpdateProgress();
    public void UpdateProgress()
    {
        m_fill.localScale = new Vector3(m_progress, m_fill.localScale.y, m_fill.localScale.z);
    }
}
