using System;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    #region AttributePoints

    [SerializeField] int m_attributePoints;
    public int attributePoints { get { return m_attributePoints; } }
    public void SetAttributePoints(int _attributePoints)
    {
        int oldVal = m_attributePoints;
        m_attributePoints = _attributePoints;

        onAttributePointsChanged?.Invoke(oldVal, _attributePoints);
    }

    public void AddAttributePoints(int _attributePointsToAdd)
    {
        int oldVal = m_attributePoints;
        m_attributePoints += _attributePointsToAdd;

        onAttributePointsChanged?.Invoke(oldVal, m_attributePoints);
    }

    public Action<int, int> onAttributePointsChanged;

    #endregion

    private void OnValidate() => onAttributePointsChanged?.Invoke(m_attributePoints, m_attributePoints);
    private void Awake() => onAttributePointsChanged?.Invoke(m_attributePoints, m_attributePoints);


    public Action onAddHealth, onAddStamina, onAddSpeed, onAddAttack;

    int m_healthPoints = 0, m_staminaPoints = 0, m_speedPoints = 0, m_attackPoints = 0;

    public int healthPoints { get { return m_healthPoints; } }
    public int staminaPoints { get { return m_staminaPoints; } }
    public int speedPoints { get { return m_speedPoints; } }
    public int attackPoints { get { return m_attackPoints; } }

    public void AddHealth()
    {
        m_healthPoints++;
        onAddHealth?.Invoke();
    }
    public void AddStamina()
    {
        m_staminaPoints++;
        onAddStamina?.Invoke();
    }
    public void AddSpeed()
    {
        m_speedPoints++;
        onAddSpeed?.Invoke();
    }
    public void AddAttack()
    {
        m_attackPoints++;
        onAddAttack?.Invoke();
    }

    public void ResetPoints()
    {
        m_healthPoints = 0;
        m_staminaPoints = 0;
        m_speedPoints = 0;
        m_attackPoints = 0;
    }

    int m_healthCost, m_staminaCost, m_speedCost, m_attackCost;
    public int healthCost { get { return m_healthCost; } set { m_healthCost = value; } }
    public int staminaCost { get { return m_staminaCost; } set { m_staminaCost = value; } }
    public int speedCost { get { return m_speedCost; } set { m_speedCost = value; } }
    public int attackCost { get { return m_attackCost; } set { m_attackCost = value; } }
}
