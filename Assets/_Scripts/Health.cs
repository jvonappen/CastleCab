using System;
using UnityEngine;

[Serializable]
public class Health : MonoBehaviour
{
    protected float m_maxHealth = 100;
    [SerializeField] protected float m_health = 100;

    public Action onDeath;
    public Action<float> onDamaged;

    private void Awake() => Init();
    protected virtual void Init()
    {
        m_maxHealth = m_health;
    }

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
