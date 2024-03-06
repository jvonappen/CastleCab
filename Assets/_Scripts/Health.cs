using System;
using UnityEngine;

[Serializable]
public class Health
{
    [SerializeField] float m_health = 100;

    public Action onDeath;
    public Action<float> onDamaged;

    public void DealDamage(float _damageAmount)
    {
        m_health -= _damageAmount;

        onDamaged?.Invoke(_damageAmount);

        CheckAlive();
    }

    void CheckAlive()
    {
        if ((int)m_health <= 0) Die();
    }

    void Die()
    {
        onDeath?.Invoke();
    }
}
