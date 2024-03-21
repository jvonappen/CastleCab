using System;
using UnityEngine;

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
    }

    public void DealDamage(float _damageAmount, PlayerAttack _player)
    {
        if (!m_isInvulnerable)
        {
            float previousHealth = m_health;
            m_health -= _damageAmount;
            onHealthChanged?.Invoke(previousHealth, m_health);

            // Display damage popup text
            PopupDisplay.Spawn(m_popupLocation.position, m_popupRandomRange, _damageAmount.ToString(), m_fontSize, Color.white, Vector3.up * 3, null, _player.transform.GetChild(0));

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
        m_manager.AddGold(m_goldReward);
        if (_player.TryGetComponent(out Dishonour dishonour)) dishonour.AddDishonour(m_dishonourPunishment);

        onDeath?.Invoke();
        Destroy();
    }

    protected virtual void Destroy() => Destroy(gameObject);
}
