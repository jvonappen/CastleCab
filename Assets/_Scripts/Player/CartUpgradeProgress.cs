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

    protected override void OnEnable()
    {
        base.OnEnable();
        SyncUpgradesToPlayer();
    }
    void SyncUpgradesToPlayer()
    {
        if (m_statUpgrade == StatUpgrade.Health)
        {
            if (m_playerUpgrades.isInitialised) m_cost = m_playerUpgrades.healthCost;
            else m_playerUpgrades.healthCost = m_cost;
            SetProgress(m_playerUpgrades.healthPoints);
        }
        else if (m_statUpgrade == StatUpgrade.Stamina)
        {
            if (m_playerUpgrades.isInitialised) m_cost = m_playerUpgrades.staminaCost;
            else m_playerUpgrades.staminaCost = m_cost;
            SetProgress(m_playerUpgrades.staminaPoints);
        }
        else if (m_statUpgrade == StatUpgrade.Speed)
        {
            if (m_playerUpgrades.isInitialised) m_cost = m_playerUpgrades.speedCost;
            else m_playerUpgrades.speedCost = m_cost;
            SetProgress(m_playerUpgrades.speedPoints);
        }
        else if (m_statUpgrade == StatUpgrade.Attack)
        {
            if (m_playerUpgrades.isInitialised) m_cost = m_playerUpgrades.attackCost;
            else m_playerUpgrades.attackCost = m_cost;
            SetProgress(m_playerUpgrades.attackPoints);
        }
    }

    protected override void OnProgressAdd()
    {
        base.OnProgressAdd();

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
    }

    protected override void OnProgressRemove()
    {
        base.OnProgressRemove();

        if (m_statUpgrade == StatUpgrade.Health)
        {
            m_playerUpgrades.RemoveHealth();
            m_playerUpgrades.healthCost = m_cost;
        }
        else if (m_statUpgrade == StatUpgrade.Stamina)
        {
            m_playerUpgrades.RemoveStamina();
            m_playerUpgrades.staminaCost = m_cost;
        }
        else if (m_statUpgrade == StatUpgrade.Speed)
        {
            m_playerUpgrades.RemoveSpeed();
            m_playerUpgrades.speedCost = m_cost;
        }
        else if (m_statUpgrade == StatUpgrade.Attack)
        {
            m_playerUpgrades.RemoveAttack();
            m_playerUpgrades.attackCost = m_cost;
        }
    }
}
