using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformAdjust2P : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField] float m_2pLeft, m_2pRight;
    float m_defaultLeft, m_defaultRight;

    bool m_hasInitialised;
    void Init()
    {
        if (m_hasInitialised) return;

        rectTransform = GetComponent<RectTransform>();

        m_defaultLeft = rectTransform.offsetMin.x;
        m_defaultRight = -rectTransform.offsetMax.x;

        m_hasInitialised = true;
    }

    private void OnEnable()
    {
        Init();

        if (GameManager.Instance.players.Count == 2)
        {
            RectTransformExtensions.SetLeft(rectTransform, m_2pLeft);
            RectTransformExtensions.SetRight(rectTransform, m_2pRight);
        }
        else
        {
            RectTransformExtensions.SetLeft(rectTransform, m_defaultLeft);
            RectTransformExtensions.SetRight(rectTransform, m_defaultRight);
        }
    }
}

public static class RectTransformExtensions
{
    public static void SetLeft(this RectTransform rt, float left) => rt.offsetMin = new Vector2(left, rt.offsetMin.y);

    public static void SetRight(this RectTransform rt, float right) => rt.offsetMax = new Vector2(-right, rt.offsetMax.y);

    public static void SetTop(this RectTransform rt, float top) => rt.offsetMax = new Vector2(rt.offsetMax.x, -top);

    public static void SetBottom(this RectTransform rt, float bottom) => rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
}
