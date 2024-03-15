using System;
using UnityEngine;

[Serializable]
public class Health : MonoBehaviour
{
    protected float m_maxHealth = 100;
    [SerializeField] protected float m_health = 100;

    public Action onDeath;
    public Action<float> onDamaged;

    public Action<float, float> onHealthChanged;

    private void Awake() => Init();
    protected virtual void Init()
    {
        m_maxHealth = m_health;
    }

    public void DealDamage(float _damageAmount)
    {
        float previousHealth = m_health;
        m_health -= _damageAmount;
        onHealthChanged?.Invoke(previousHealth, m_health);

        onDamaged?.Invoke(_damageAmount);

        CheckAlive();
    }

    void CheckAlive()
    {
        if ((int)m_health <= 0) Die();
    }

    protected virtual void Die()
    {
        onDeath?.Invoke();
    }
}
