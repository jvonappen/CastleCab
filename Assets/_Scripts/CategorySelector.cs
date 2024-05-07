using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategorySelector : MonoBehaviour
{
    List<GameObject> m_selections = new();

    public GameObject m_selectedObject { get; protected set; }

    [SerializeField] float m_selectImageSizeMulti = 1.25f;

    private void Awake()
    {
        foreach (Transform child in transform) m_selections.Add(child.gameObject);
        m_selectedObject = m_selections[0];
    }

    TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> m_tween;
    public void SelectObject(GameObject _obj)
    {
        if (m_tween == null || !m_tween.IsPlaying())
        {
            float selectionDistanceFromPrevious = m_selectedObject.transform.localPosition.y - _obj.transform.localPosition.y;
            m_tween = transform.DOLocalMoveY(transform.localPosition.y + selectionDistanceFromPrevious, 0.1f);

            _obj.GetComponentInChildren<Image>().transform.DOScale(Vector3.one * m_selectImageSizeMulti, 0.1f);

            DeselectObject(m_selectedObject);
            _obj.transform.GetChild(0).gameObject.SetActive(true);

            m_selectedObject = _obj;
        }
        
    }

    void DeselectAllObjects() { foreach (GameObject selection in m_selections) DeselectObject(selection); }
    void DeselectObject(GameObject _obj)
    {
        _obj.transform.GetChild(0).gameObject.SetActive(false);
        _obj.GetComponentInChildren<Image>().transform.DOScale(Vector3.one, 0.1f);
    }

    public void SetInteraction(bool _canInteract)
    {
        foreach (GameObject selection in m_selections) selection.GetComponent<Button>().interactable = _canInteract;
    }
}
