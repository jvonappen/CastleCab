using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class ExpandOnSelect : MonoBehaviour
{
    [SerializeField] bool m_setInFront = true;
    [SerializeField] float m_scaleMulti = 1.2f;
    [SerializeField] float m_defaultScale;

    TweenerCore<Vector3, Vector3, VectorOptions> m_tween;

    private void Awake() => m_defaultScale = transform.localScale.x;
    public void Select()
    {
        m_tween = transform.DOScale(m_defaultScale * m_scaleMulti, 0.2f);

        if (m_setInFront)
        {
            if (transform.parent && transform.parent.TryGetComponent(out GridLayoutGroup layoutGroup) && layoutGroup.enabled) TimerManager.RunAfterTime(() => { layoutGroup.enabled = false; transform.SetAsLastSibling(); }, 0.1f);
            else transform.SetAsLastSibling();
        }
    }
    public void Deselect() => m_tween = transform.DOScale(m_defaultScale, 0.2f);

    //private void OnEnable() => transform.localScale = Vector3.one * m_defaultScale;
    private void OnDisable()
    {
        m_tween.Kill();
        transform.localScale = Vector3.one * m_defaultScale;
    }

    public void ResetScale()
    {
        m_tween.Kill();
        transform.localScale = Vector3.one * m_defaultScale;
    }
}
