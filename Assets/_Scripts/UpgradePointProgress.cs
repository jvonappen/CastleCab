using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradePointProgress : PointProgress
{
    [SerializeField] int m_TEMPGold = 500;
    [SerializeField] float m_costMulti = 1.3f;

    [SerializeField] int m_cost = 100;
    [SerializeField] TextMeshProUGUI m_costText;

    public override void AddProgress()
    {
        if (m_cost <= m_TEMPGold)
        {
            m_TEMPGold -= m_cost;
            m_cost = (int)(m_cost * m_costMulti);

            UpdateCostText();

            base.AddProgress();
        }
    }

    protected override void OnValidated()
    {
        base.OnValidated();

        UpdateCostText();
    }

    private void Start() => UpdateCostText();
    void UpdateCostText()
    {
        if (m_costText) m_costText.text = "Cost: " + m_cost;
        else Debug.LogWarning("No reference is set for cost display");
    }
}
