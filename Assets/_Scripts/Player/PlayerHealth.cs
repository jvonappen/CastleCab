using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] ProgressBar m_healthBar;

    [Space(5)]
    [SerializeField] float m_addHealthPerStat = 10;

    float m_originalMaxHealth;

    protected override void Init()
    {
        base.Init();

        m_originalMaxHealth = m_maxHealth;
        m_maxHealth = m_originalMaxHealth + (m_addHealthPerStat * SharedPlayerStats.healthPoints);

        m_health = m_maxHealth;
        UpdateHealthBar();

        SharedPlayerStats.onAddHealth += OnAddHealth;
        onHealthChanged += OnHealthChanged;
    }

    void OnAddHealth()
    {
        m_maxHealth = m_originalMaxHealth + (m_addHealthPerStat * SharedPlayerStats.healthPoints);
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

    protected override void Die()
    {
        base.Die();

        gameObject.SetActive(false);
    }
}
