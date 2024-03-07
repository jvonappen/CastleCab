using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PointProgress : MonoBehaviour
{
    [SerializeField] int m_progress;
    public int progress { get { return m_progress; } }
    

    [SerializeField] Color m_progressColour, m_defaultColour;

    List<Image> m_points = new();
    void SetPoints() => m_points = GetComponentsInChildren<Image>().ToList();

    public void AddProgress()
    {
        if (m_progress < m_points.Count) m_progress++;
        UpdateProgress();
        Debug.Log("Clicked");
    }

    private void OnValidate()
    {
        SetPoints();
        UpdateProgress();

        // Clamps progress
        if (m_progress < 0) m_progress = 0;
        else if (m_progress > m_points.Count) m_progress = m_points.Count;
    }
    private void Awake() => SetPoints();

    void UpdateProgress()
    {
        for (int i = 0; i < m_points.Count; i++)
        {
            if (i < m_progress) m_points[i].color = m_progressColour;
            else m_points[i].color = m_defaultColour;
        }
    }
}
