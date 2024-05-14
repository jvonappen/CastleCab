using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using DG.Tweening;

public class PointProgress : MonoBehaviour
{
    [SerializeField] protected int m_progress;
    public int progress { get { return m_progress; } }
    

    [SerializeField] Color m_progressColour, m_defaultColour;

    protected List<Image> m_points = new();
    public int totalPoints { get { return m_points.Count; } }
    void SetPoints()
    {
        m_points.Clear();
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Image image)) m_points.Add(image);
        }

        UpdateProgress();
    }

    [SerializeField] bool m_expandLastSelected;
    [SerializeField][ConditionalHide("m_expandLastSelected")] float m_defaultScale = 1, m_expandScaleMulti = 1.5f, m_tweenDuration = 0.2f;

    TweenerCore<Vector3, Vector3, VectorOptions> m_scaleTween;

    public virtual void AddProgress()
    {
        if (m_progress < m_points.Count) m_progress++;
        UpdateProgress();
    }

    public virtual void RemoveProgress()
    {
        if (m_progress > 0) m_progress--;
        UpdateProgress();
    }

    public virtual void SetProgress(int _progress)
    {
        if (m_progress <= m_points.Count) m_progress = _progress;
        else m_progress = m_points.Count;
        UpdateProgress();
    }

    protected virtual void OnValidated() { }

    public void OnValidate()
    {
        SetPoints();

        // Clamps progress
        if (m_progress < 0) m_progress = 0;
        else if (m_progress > m_points.Count) m_progress = m_points.Count;

        OnValidated();
    }
    private void Awake() => SetPoints();

    protected void UpdateProgress()
    {
        m_scaleTween.Kill();

        for (int i = 0; i < m_points.Count; i++)
        {
            if (i < m_progress)
            {
                m_points[i].color = m_progressColour;

                // Updates last selected point for custom functionality
                if (i+1 == m_progress) UpdateLastSelectedPoint(m_points[i]);
                else ResetScale(m_points[i]);
            }
            else
            {
                m_points[i].color = m_defaultColour;
                ResetScale(m_points[i]);
            }
        }
    }

    void ResetScale(Image _image)
    {
        Transform target = _image.transform;
        if (target.localScale != Vector3.one * m_defaultScale) target.DOScale(Vector3.one * m_defaultScale, m_tweenDuration);
        //_image.transform.localScale = Vector3.one * m_defaultScale;
    }

    protected void UpdateLastSelectedPoint(Image _image)
    {
        if (m_expandLastSelected) 
            m_scaleTween = _image.transform.DOScale(Vector3.one * m_defaultScale * m_expandScaleMulti, m_tweenDuration);
    }
}
