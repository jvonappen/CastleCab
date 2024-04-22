using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Currency
{
    Gold,
    AttributePoints
}

public class UpgradePointProgress : PointProgress
{
    GameManager m_manager;

    [SerializeField] Currency m_currency;

    [SerializeField] bool m_isExponential;
    [ConditionalHide("m_isExponential")] [SerializeField] float m_costMulti = 1.3f;
    [ConditionalHide("m_isExponential", Inverse = true)] [SerializeField] int m_costIncrease = 1;

    [SerializeField] protected int m_cost = 100;
    [SerializeField] TextMeshProUGUI m_costText;

    [ConditionalEnumHide("m_currency", 1)] [SerializeField] protected PlayerUpgrades m_playerUpgrades;

    public override void AddProgress()
    {
        if (m_cost <= GetCurrencyAmount())
        {
            if (m_progress < m_points.Count)
            {
                if (m_currency == Currency.Gold) m_manager.SetGold(m_manager.gold - m_cost);
                else if (m_currency == Currency.AttributePoints) m_playerUpgrades.SetAttributePoints(m_playerUpgrades.attributePoints - m_cost);
                else Debug.LogWarning("Currency not found");

                if (m_isExponential) m_cost = (int)(m_cost * m_costMulti);
                else m_cost += m_costIncrease;

                UpdateCostText();

                base.AddProgress();

                OnProgressAdd();
            }
        }

    }

    public override void RemoveProgress()
    {
        if (m_progress > 0)
        {
            int currencyToRecieve = 0;
            if (m_isExponential) currencyToRecieve = (int)(m_cost / m_costMulti);
            else currencyToRecieve = m_cost - m_costIncrease;

            if (m_currency == Currency.Gold) m_manager.AddGold(currencyToRecieve);
            else if (m_currency == Currency.AttributePoints) m_playerUpgrades.AddAttributePoints(currencyToRecieve);

            m_cost = currencyToRecieve;

            UpdateCostText();

            base.RemoveProgress();

            OnProgressRemove();
        }
    }

    int GetCurrencyAmount()
    {
        if (m_currency == Currency.Gold) return m_manager.gold;
        else if (m_currency == Currency.AttributePoints) return m_playerUpgrades.attributePoints;

        Debug.LogWarning("Currency amount not found");
        return 0;
    }

    protected virtual void OnProgressAdd() { }
    protected virtual void OnProgressRemove() { }

    protected override void OnValidated()
    {
        base.OnValidated();

        UpdateCostText();
    }

    private void Start()
    {
        m_manager = GameManager.Instance;

        UpdateCostText();

        m_manager.onGoldChanged += OnCurrencyChanged;
        if (m_playerUpgrades) m_playerUpgrades.onAttributePointsChanged += OnCurrencyChanged;
    }

    void OnCurrencyChanged(int _oldVal, int _newVal) => UpdateCostText();

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
            if (m_cost <= GetCurrencyAmount()) colourHex = ColorUtility.ToHtmlStringRGBA(m_manager.m_affordColour);
            else colourHex = ColorUtility.ToHtmlStringRGBA(m_manager.m_notAffordColour);

            return "<color=#" + colourHex + ">";
        }

        return "";
    }

    string EndColour() => "</color>";
    #endregion
}
