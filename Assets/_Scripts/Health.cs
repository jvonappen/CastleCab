using System;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Health : MonoBehaviour
{
    GameManager m_manager;

    protected float m_maxHealth = 100;
    [SerializeField] protected float m_health = 100;
    [SerializeField] protected int m_dishonourPunishment = 15, m_goldReward = 20;

    protected bool m_isInvulnerable;

    public Action onDeath;
    public Action<float, PlayerAttack> onDamaged;

    public Action<float, float> onHealthChanged;

    [SerializeField] protected GameObject m_damagedParticlePrefab, m_destroyedParticlePrefab;
    protected ParticleSystem m_damagedParticle, m_destroyedParticle;

    [Header("Popup")]
    [SerializeField] protected bool m_showPopup = true;
    [ConditionalHide("m_showPopup")] [SerializeField] protected Transform m_popupLocation;
    [ConditionalHide("m_showPopup")] [SerializeField] protected float m_popupRandomRange = 2, m_fontSize = 5;

    private void Awake() => Init();
    protected virtual void Init()
    {
        m_manager = FindObjectOfType<GameManager>();

        if (!m_popupLocation) m_popupLocation = transform;
        m_maxHealth = m_health;

        if (m_damagedParticlePrefab)
        {
            m_damagedParticle = Instantiate(m_damagedParticlePrefab, transform).GetComponent<ParticleSystem>();
            m_damagedParticle.transform.localPosition = Vector3.zero;
        }
        
        if (m_destroyedParticlePrefab)
        {
            m_destroyedParticle = Instantiate(m_destroyedParticlePrefab, transform).GetComponent<ParticleSystem>();
            m_destroyedParticle.transform.localPosition = Vector3.zero;
        }
    }

    public virtual void DealDamage(float _damageAmount, PlayerAttack _player)
    {
        if (!m_isInvulnerable)
        {
            float previousHealth = m_health;
            m_health -= _damageAmount;
            onHealthChanged?.Invoke(previousHealth, m_health);

            // Display damage popup text
            Transform lookAt = null;
            if (_player) lookAt = _player.transform.GetChild(0);
            PopupDisplay.Spawn(m_popupLocation.position, m_popupRandomRange, _damageAmount.ToString(), m_fontSize, Color.white, Vector3.up * 3, null, lookAt);

            onDamaged?.Invoke(_damageAmount, _player);

            CheckAlive(_player);
        }
    }

    void CheckAlive(PlayerAttack _player)
    {
        if ((int)m_health <= 0) Die(_player);
    }

    protected virtual void Die(PlayerAttack _player)
    {
        if (m_destroyedParticle)
        {
            m_destroyedParticle.transform.SetParent(null);
            m_destroyedParticle.Play();

            m_destroyedParticle.GetComponent<CFX_AutoDestructShuriken>().enabled = true;
        }
        
        m_manager.AddGold(m_goldReward);
        if (_player)
        {
            if (_player.TryGetComponent(out Dishonour dishonour)) dishonour.AddDishonour(m_dishonourPunishment);
        }
        
        onDeath?.Invoke();
        Destroy();
    }

    protected virtual void Destroy() => Destroy(gameObject);
}
