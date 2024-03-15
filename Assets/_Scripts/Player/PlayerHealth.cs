using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] ProgressBar m_healthBar;

    [SerializeField] Transform m_popupLocation;
    [SerializeField] float m_popupRandomRange = 2, m_fontSize = 5;

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
        onDamaged += OnDamaged;
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

    void OnDamaged(float _damageAmount, PlayerAttack _player)
    {
        // Display damage popup text
        PopupDisplay.Spawn(m_popupLocation.position, m_popupRandomRange, _damageAmount.ToString(), m_fontSize, Color.white, Vector3.up * 3, null, _player.transform.GetChild(0));
    }
}
