using System;
using System.Collections.Generic;
using UnityEditor;
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

    [SerializeField] protected List<GameObject> m_damagedParticlePrefabs, m_destroyedParticlePrefabs;
    protected List<ParticleSystem> m_damagedParticles = new(), m_destroyedParticles = new();
    
    [SerializeField] protected AudioGroupDetails sfxAudio;

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

        for (int i = 0; i < m_damagedParticlePrefabs.Count; i++)
        {
            ParticleSystem damagedParticle;
            damagedParticle = Instantiate(m_damagedParticlePrefabs[i], transform).GetComponent<ParticleSystem>();
            damagedParticle.transform.localPosition = Vector3.zero;

            m_damagedParticles.Add(damagedParticle);
        }

        for (int i = 0; i < m_destroyedParticlePrefabs.Count; i++)
        {
            ParticleSystem destroyedParticle;
            destroyedParticle = Instantiate(m_destroyedParticlePrefabs[i], transform).GetComponent<ParticleSystem>();
            destroyedParticle.transform.localPosition = Vector3.zero;

            m_destroyedParticles.Add(destroyedParticle);
        }
    }

    public virtual void DealDamage(float _damageAmount, PlayerAttack _player)
    {
        if (!m_isInvulnerable)
        {
            if (m_damagedParticles.Count > 0)
            {
                int randIndex = UnityEngine.Random.Range(0, m_damagedParticlePrefabs.Count);

                m_damagedParticles[randIndex].transform.SetParent(null);
                m_damagedParticles[randIndex].Play();

                CFX_AutoDestructShuriken t = m_damagedParticles[randIndex].GetComponent<CFX_AutoDestructShuriken>();
                if (t) t.enabled = true;
            }

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
        if (m_destroyedParticles.Count > 0)
        {
            int randIndex = UnityEngine.Random.Range(0, m_destroyedParticlePrefabs.Count);

            m_destroyedParticles[randIndex].transform.SetParent(null);
            m_destroyedParticles[randIndex].Play();

            CFX_AutoDestructShuriken t = m_destroyedParticles[randIndex].GetComponent<CFX_AutoDestructShuriken>();
            if (t) t.enabled = true;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (AudioManager.Instance)
        {
            if (sfxAudio != null) AudioManager.Instance.PlayGroupAudio(sfxAudio.audioGroupName);
        }
        else Debug.LogWarning("There is no audio manager in scene!");
    }
}
