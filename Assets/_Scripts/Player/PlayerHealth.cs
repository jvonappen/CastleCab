using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [Space(5)]
    [SerializeField] float m_addHealthPerStat = 10;

    float m_originalMaxHealth;

    protected override void Init()
    {
        base.Init();

        m_originalMaxHealth = m_maxHealth;
        m_maxHealth = m_originalMaxHealth + (m_addHealthPerStat * SharedPlayerStats.healthPoints);

        m_health = m_maxHealth;

        SharedPlayerStats.onAddHealth += OnAddHealth;
    }

    void OnAddHealth()
    {
        m_maxHealth = m_originalMaxHealth + (m_addHealthPerStat * SharedPlayerStats.healthPoints);
        m_health = m_maxHealth;
    }
}
