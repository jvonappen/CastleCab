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
            m_playerUpgrades.AddHealth();
            m_playerUpgrades.healthCost = m_cost;
        }
        else if (m_statUpgrade == StatUpgrade.Stamina)
        {
            m_playerUpgrades.AddStamina();
            m_playerUpgrades.staminaCost = m_cost;
        }
        else if (m_statUpgrade == StatUpgrade.Speed)
        {
            m_playerUpgrades.AddSpeed();
            m_playerUpgrades.speedCost = m_cost;
        }
        else if (m_statUpgrade == StatUpgrade.Attack)
        {
            m_playerUpgrades.AddAttack();
            m_playerUpgrades.attackCost = m_cost;
        }

        //CartUpgradeProgress[] cartUpgradeProgresses = FindObjectsOfType<CartUpgradeProgress>();
        //foreach (CartUpgradeProgress c in cartUpgradeProgresses) c.UpdateStatToGlobal();
    }
    
    /// <summary>
    /// Syncs upgrades to other players
    /// </summary>
    //internal void UpdateStatToGlobal()
    //{
    //    if (m_statUpgrade == StatUpgrade.Health)
    //    {
    //        m_progress = m_playerUpgrades.healthPoints;
    //        if (m_playerUpgrades.healthCost > 0) m_cost = m_playerUpgrades.healthCost;
    //    }
    //    else if (m_statUpgrade == StatUpgrade.Stamina)
    //    {
    //        m_progress = m_playerUpgrades.staminaPoints;
    //        if (m_playerUpgrades.staminaCost > 0) m_cost = m_playerUpgrades.staminaCost;
    //    }
    //    else if (m_statUpgrade == StatUpgrade.Speed)
    //    {
    //        m_progress = m_playerUpgrades.speedPoints;
    //        if (m_playerUpgrades.speedCost > 0) m_cost = m_playerUpgrades.speedCost;
    //    }
    //    else if (m_statUpgrade == StatUpgrade.Attack)
    //    {
    //        m_progress = m_playerUpgrades.attackPoints;
    //        if (m_playerUpgrades.attackCost > 0) m_cost = m_playerUpgrades.attackCost;
    //    }
    //    
    //    UpdateCostText();
    //    UpdateProgress();
    //}

    private void OnEnable()
    {
        //UpdateStatToGlobal();
    }
}
