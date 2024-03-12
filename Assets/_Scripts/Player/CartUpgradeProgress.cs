using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartUpgradeProgress : UpgradePointProgress
{
    enum StatUpgrade
    {
        Health,
        Stamina,
        Speed,
        Attack
    }
    [SerializeField] StatUpgrade m_statUpgrade;

    /// <summary>
    /// Updates global stat and runs a function on all instances of this script type to keep them in sync
    /// </summary>
    protected override void OnProgressAdd()
    {
        base.OnProgressAdd();

        // Updates static global stat
        if (m_statUpgrade == StatUpgrade.Health)
        {
            CartStats.AddHealth();
            CartStats.healthCost = m_cost;
        }
        else if (m_statUpgrade == StatUpgrade.Stamina)
        {
            CartStats.AddStamina();
            CartStats.staminaCost = m_cost;
        }
        else if (m_statUpgrade == StatUpgrade.Speed)
        {
            CartStats.AddSpeed();
            CartStats.speedCost = m_cost;
        }
        else if (m_statUpgrade == StatUpgrade.Attack)
        {
            CartStats.AddAttack();
            CartStats.attackCost = m_cost;
        }

        CartUpgradeProgress[] cartUpgradeProgresses = FindObjectsOfType<CartUpgradeProgress>();
        foreach (CartUpgradeProgress c in cartUpgradeProgresses) c.UpdateStatToGlobal();
    }
    
    /// <summary>
    /// Syncs upgrades to other players
    /// </summary>
    internal void UpdateStatToGlobal()
    {
        if (m_statUpgrade == StatUpgrade.Health)
        {
            m_progress = CartStats.healthPoints;
            if (CartStats.healthCost > 0) m_cost = CartStats.healthCost;
        }
        else if (m_statUpgrade == StatUpgrade.Stamina)
        {
            m_progress = CartStats.staminaPoints;
            if (CartStats.staminaCost > 0) m_cost = CartStats.staminaCost;
        }
        else if (m_statUpgrade == StatUpgrade.Speed)
        {
            m_progress = CartStats.speedPoints;
            if (CartStats.speedCost > 0) m_cost = CartStats.speedCost;
        }
        else if (m_statUpgrade == StatUpgrade.Attack)
        {
            m_progress = CartStats.attackPoints;
            if (CartStats.attackCost > 0) m_cost = CartStats.attackCost;
        }
        
        UpdateCostText();
        UpdateProgress();
    }

    private void OnEnable()
    {
        UpdateStatToGlobal();
    }
}
