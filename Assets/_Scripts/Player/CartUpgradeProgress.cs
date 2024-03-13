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
            SharedPlayerStats.AddHealth();
            SharedPlayerStats.healthCost = m_cost;
        }
        else if (m_statUpgrade == StatUpgrade.Stamina)
        {
            SharedPlayerStats.AddStamina();
            SharedPlayerStats.staminaCost = m_cost;
        }
        else if (m_statUpgrade == StatUpgrade.Speed)
        {
            SharedPlayerStats.AddSpeed();
            SharedPlayerStats.speedCost = m_cost;
        }
        else if (m_statUpgrade == StatUpgrade.Attack)
        {
            SharedPlayerStats.AddAttack();
            SharedPlayerStats.attackCost = m_cost;
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
            m_progress = SharedPlayerStats.healthPoints;
            if (SharedPlayerStats.healthCost > 0) m_cost = SharedPlayerStats.healthCost;
        }
        else if (m_statUpgrade == StatUpgrade.Stamina)
        {
            m_progress = SharedPlayerStats.staminaPoints;
            if (SharedPlayerStats.staminaCost > 0) m_cost = SharedPlayerStats.staminaCost;
        }
        else if (m_statUpgrade == StatUpgrade.Speed)
        {
            m_progress = SharedPlayerStats.speedPoints;
            if (SharedPlayerStats.speedCost > 0) m_cost = SharedPlayerStats.speedCost;
        }
        else if (m_statUpgrade == StatUpgrade.Attack)
        {
            m_progress = SharedPlayerStats.attackPoints;
            if (SharedPlayerStats.attackCost > 0) m_cost = SharedPlayerStats.attackCost;
        }
        
        UpdateCostText();
        UpdateProgress();
    }

    private void OnEnable()
    {
        UpdateStatToGlobal();
    }
}
