using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScroll : MonoBehaviour
{
    [SerializeField] int m_displayCount = 4;
    List<GameObject> m_selectables = new();
    GameObject m_previousSelectable, m_currentSelectable;

    int m_moveHistory = 0; // -1 = down, 1 = up
    int m_listPos = 1;

    private void OnEnable()
    {
        TimerManager.RunAfterTime(() =>
        {
            foreach (Transform child in transform) m_selectables.Add(child.gameObject);
            transform.parent.localPosition = Vector3.zero;
            m_moveHistory = 0;
        }, 0.05f);
    }

    public void SelectObject(GameObject _obj)
    {
        m_previousSelectable = m_currentSelectable;
        m_currentSelectable = _obj;

        CalculateScroll();
    }

    void CalculateScroll()
    {
        int prevIndex = m_previousSelectable != null ? m_previousSelectable.transform.GetSiblingIndex() : 0;
        int currIndex = m_currentSelectable.transform.GetSiblingIndex();
        if (prevIndex < currIndex) // If moving down
        {
            if (m_listPos < m_displayCount) m_listPos++;
            else MoveContent();

            //if (m_moveHistory <= 0) // If was moving down
            //{
            //    if (m_listPos < m_displayCount) m_listPos++;
            //    m_moveHistory--;
            //    if (Mathf.Abs(m_moveHistory) >= m_displayCount) MoveContent();
            //}
            //else if (m_moveHistory > 0) // If was moving up
            //{
            //    m_moveHistory = -1;
            //    m_listPos++;
            //}
        }
        else if (prevIndex > currIndex) // If moving up
        {
            if (m_listPos > 1) m_listPos--;
            else MoveContent();

            //if (m_moveHistory >= 0) // If was moving up
            //{
            //    if (m_listPos > 1) m_listPos--;
            //    m_moveHistory++;
            //    if (Mathf.Abs(m_moveHistory) >= m_displayCount) MoveContent();
            //}
            //else if (m_moveHistory < 0) // If was moving down
            //{
            //    m_moveHistory = 1;
            //    m_listPos--;
            //}
        }
    }

    void MoveContent()
    {
        Vector3 displacement = m_previousSelectable.transform.position - m_currentSelectable.transform.position;
        transform.parent.position += displacement;
    }
}
