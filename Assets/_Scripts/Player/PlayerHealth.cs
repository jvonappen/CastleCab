using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : Health
{
    [SerializeField] ProgressBar m_healthBar;
    PlayerMovement m_playerMovement;

    [Space(5)]
    [SerializeField] float m_addHealthPerStat = 10;

    float m_originalMaxHealth;

    float GetMaxHealth()
    {
        int healthPoints = GameManager.Instance.GetPlayerData(GetComponentInParent<PlayerInput>().devices[0]).playerUpgradeData.health;
        return m_originalMaxHealth + (m_addHealthPerStat * healthPoints);
    }

    protected override void Init()
    {
        base.Init();

        m_playerMovement = GetComponentInParent<PlayerMovement>();

        m_originalMaxHealth = m_maxHealth;

        //int healthPoints = GameManager.Instance.GetPlayerData(GetComponentInParent<PlayerInput>().devices[0]).playerUpgradeData.health;
        //m_maxHealth = m_originalMaxHealth + (m_addHealthPerStat * healthPoints);
        m_maxHealth = m_originalMaxHealth;

        m_health = m_maxHealth;
        UpdateHealthBar();

        TimerManager.RunAfterTime(OnHealthUpgrade, 0.1f);

        onHealthChanged += OnHealthChanged;
    }

    void OnHealthUpgrade() // If upgrade is made after init, call this manually
    {
        m_maxHealth = GetMaxHealth();
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

    protected override void Destroy()
    {
        //gameObject.SetActive(false);
    }

    public override void DealDamage(float _damageAmount, PlayerAttack _player)
    {
        if (!m_playerMovement.isHurricane || (_player && _player.playerMovement.isHurricane))
        {
            base.DealDamage(_damageAmount, _player);
        }
    }

    public void HealthPickupIncrease(float pickupValue)
    {
        float prevHealth = m_health;
        m_health = m_health + pickupValue;
        if (m_health > m_maxHealth) m_health = m_maxHealth;
        onHealthChanged?.Invoke(prevHealth, m_health);
    }
}
