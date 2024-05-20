using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] ProgressBar m_healthBar;
    PlayerMovement m_playerMovement;

    [Space(5)]
    [SerializeField] float m_addHealthPerStat = 10;

    float m_originalMaxHealth;

    [SerializeField] PlayerUpgrades m_playerUpgrades;

    protected override void Init()
    {
        base.Init();

        m_playerMovement = GetComponentInParent<PlayerMovement>();

        m_originalMaxHealth = m_maxHealth;
        m_maxHealth = m_originalMaxHealth + (m_addHealthPerStat * m_playerUpgrades.healthPoints);

        m_health = m_maxHealth;
        UpdateHealthBar();

        m_playerUpgrades.onAddHealth += OnHealthUpgrade;
        m_playerUpgrades.onRemoveHealth += OnHealthUpgrade;

        onHealthChanged += OnHealthChanged;
    }

    void OnHealthUpgrade()
    {
        m_maxHealth = m_originalMaxHealth + (m_addHealthPerStat * m_playerUpgrades.healthPoints);
        m_health = m_maxHealth;
    }

    private void OnValidate() => UpdateHealthBar();
    void OnHealthChanged(float _prevHealth, float _newHealth) => UpdateHealthBar();

    void UpdateHealthBar()
    {
        m_healthBar.progress = m_health / m_maxHealth;

        // Clamp health display progress to limits
        if (m_healthBar.progress < 0) m_healthBar.progress = 0;
        else if (m_healthBar.progress > 1) m_healthBar.progress = 1;

        m_healthBar.UpdateProgress();
    }

    protected override void Destroy() => gameObject.SetActive(false);

    public override void DealDamage(float _damageAmount, PlayerAttack _player)
    {
        if (!m_playerMovement.isHurricane || (_player && _player.playerMovement.isHurricane))
        {
            base.DealDamage(_damageAmount, _player);
        }
    }

    public void HealthPickupIncrease(float pickupValue)
    {
        m_health = m_health + pickupValue;
        if (m_health > m_maxHealth) m_health = m_maxHealth;
        UpdateHealthBar();
    }
}
