using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradePointProgress : PointProgress
{
    GameManager m_manager;

    [SerializeField] float m_costMulti = 1.3f;

    [SerializeField] protected int m_cost = 100;
    [SerializeField] TextMeshProUGUI m_costText;

    public override void AddProgress()
    {
        if (m_cost <= m_manager.gold)
        {
            if (m_progress < m_points.Count)
            {
                m_manager.SetGold(m_manager.gold - m_cost);
                m_cost = (int)(m_cost * m_costMulti);

                UpdateCostText();

                base.AddProgress();

                OnProgressAdd();
            }
        }
    }

    protected virtual void OnProgressAdd() { }

    protected override void OnValidated()
    {
        base.OnValidated();

        UpdateCostText();
    }

    private void Start()
    {
        m_manager = GameManager.Instance;

        UpdateCostText();

        m_manager.onGoldChanged += OnGoldChanged;
    }

    void OnGoldChanged(int _oldVal, int _newVal) => UpdateCostText();

    protected void UpdateCostText()
    {
        if (m_costText) m_costText.text = "Cost: " + GetColour() + m_cost + EndColour();
        else Debug.LogWarning("No reference is set for cost display");
    }

    #region Colour
    string GetColour()
    {
        if (m_manager)
        {
            string colourHex;
            if (m_cost <= m_manager.gold) colourHex = ColorUtility.ToHtmlStringRGBA(m_manager.m_affordColour);
            else colourHex = ColorUtility.ToHtmlStringRGBA(m_manager.m_notAffordColour);

            return "<color=#" + colourHex + ">";
        }

        return "";
    }

    string EndColour() => "</color>";
    #endregion
}
