using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Health m_health;

    private void Awake()
    {
        m_health.onDamaged += OnDamaged;
        m_health.onDeath += OnDeath;
    }

    void OnDamaged(float _damageAmount, PlayerAttack _player)
    {
        
    }

    void OnDeath()
    {
        
    }
}
