using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DestroyedStatisticTracker : StatisticTracker
{
    Health m_health;
    private void OnEnable()
    {
        m_health = GetComponent<Health>();
        m_health.onDeath += UpdateStatistic;
    }
    
    private void OnDisable()
    {
        m_health.onDeath -= UpdateStatistic;
    }
}
