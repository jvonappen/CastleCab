using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    GameManager m_manager;

    protected float m_maxHealth = 100;
    [SerializeField] protected float m_health = 100;
    [SerializeField] protected int m_dishonourPunishment = 15, m_goldReward = 20;

    public float maxHealth { get { return m_maxHealth; } }
    public float health { get { return m_health; } }

    protected bool m_isInvulnerable;

    public Action onDeath;
    public Action<float, PlayerAttack> onDamaged;
    public Action<float, float> onHealthChanged;

    [SerializeField] private bool m_canRespawn = true;
    [SerializeField] private float m_respawnTime = 5;

    [Header("Particle Prefab")]
    [SerializeField] private GameObject m_collisionParticlePrefab;
    [SerializeField] private GameObject m_damagedParticlePrefab;
    [SerializeField] private GameObject m_destroyedParticlePrefab;
    [SerializeField] private GameObject m_respawnParticlePrefab;

    [Header("Audio")]
    [SerializeField] protected AudioGroupDetails m_collidedSFX;
    [SerializeField] protected AudioGroupDetails m_damagedSFX;
    [SerializeField] protected AudioGroupDetails m_destroyedSFX;

    [Header("Debug")]
    [SerializeField] private string m_name;

    private void Awake() => Init();
    protected virtual void Init()
    {
        m_manager = FindObjectOfType<GameManager>();
        m_maxHealth = m_health;

        if(GetComponentInParent<QuestTarget>() != null) { m_name = GetComponentInParent<QuestTarget>().targetName; }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (AudioManager.Instance)
            {
                if (m_collidedSFX != null) AudioManager.Instance.PlaySoundAtLocation(m_collidedSFX.audioGroupName, transform.position);
            }
            else Debug.LogWarning("There is no audio manager in scene!");

            PlayParticle(ref m_collisionParticlePrefab);
        }
    }

    public virtual void DealDamage(float _damageAmount, PlayerAttack _player)
    {
        if (!m_isInvulnerable)
        {
            PlayParticle(ref m_damagedParticlePrefab);

            float previousHealth = m_health;
            m_health -= _damageAmount;
            onHealthChanged?.Invoke(previousHealth, m_health);

            // Display damage popup text
            Transform lookAt = null;
            if (_player) lookAt = _player.transform.GetChild(0);
            //PopupDisplay.Spawn(m_popupLocation.position, m_popupRandomRange, _damageAmount.ToString(), m_fontSize, Color.white, Vector3.up * 3, null, lookAt);

            onDamaged?.Invoke(_damageAmount, _player);

            if (CheckAlive(_player))
            {
                if (m_damagedSFX != null) AudioManager.Instance.PlaySoundAtLocation(m_damagedSFX.audioGroupName, transform.position);
            }
        }
    }

    bool CheckAlive(PlayerAttack _player)
    {
        if ((int)m_health <= 0)
        {
            Die(_player);
            return false;
        }
        return true;
    }

    protected virtual void Die(PlayerAttack _player)
    {
        //Double Check name
        if (GetComponentInParent<QuestTarget>() != null) { m_name = GetComponentInParent<QuestTarget>().targetName; }

        if (m_destroyedSFX != null) AudioManager.Instance.PlaySoundAtLocation(m_destroyedSFX.audioGroupName, transform.position);
        PlayParticle(ref m_destroyedParticlePrefab);
        GameObject particleParent = GameObject.Find("----Particles");
        if (particleParent && m_destroyedParticlePrefab) m_destroyedParticlePrefab.transform.SetParent(particleParent.transform);

        if(QuestManager.Instance.quest.questObjectives.questTarget != null) { QuestManager.Instance.quest.questObjectives.ObjectiveKilled(m_name); }
        


        m_manager.AddGold(m_goldReward);
        if (_player)
        {
            if (_player.TryGetComponent(out Dishonour dishonour)) dishonour.AddDishonour(m_dishonourPunishment);
        }

        // If not player, increase destroyed object statistic count
        if (gameObject.layer != LayerMask.NameToLayer("Player")) GameStatistics.GetStat(Statistic.ObjectsDestroyed).Value++;

        onDeath?.Invoke();
        //Destroy();
        gameObject.SetActive(false);
        RespawnObject();
    }

    protected virtual void Destroy() => Destroy(gameObject);



    private void RespawnObject()
    {
        if (!m_canRespawn) return;

        TimerManager.RunAfterTime(() =>
        {
            PlayParticle(ref m_respawnParticlePrefab);
            m_health = m_maxHealth;
            gameObject.SetActive(true);
            Init();
            m_destroyedParticlePrefab.transform.SetParent(transform);
        }, m_respawnTime);
    }

    void PlayParticle(ref GameObject pp)
    {

        if (pp)
        {
            if (!pp.scene.isLoaded)
            {
                // Instantiates the particle if the reference is a prefab and not an instance
                pp = Instantiate(pp, transform);
                pp.transform.localPosition = Vector3.zero;
               // pp.SetActive(false);
            }

            GameObject obj = pp;

            pp.SetActive(true);
            TimerManager.RunAfterTime(() => { obj.SetActive(false); }, 0.7F);

        }
    }
}
