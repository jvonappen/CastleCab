using System;
using System.Collections.Generic;
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

    [SerializeField] private bool m_canRespawn = true;
    [SerializeField] private float m_respawnTime = 5;
    [SerializeField] private GameObject m_collisionParticlePrefab, m_damagedParticlePrefab, m_destroyedParticlePrefab, m_respawnParticlePrefab;

    //[SerializeField] protected List<GameObject> m_damagedParticlePrefabs, m_destroyedParticlePrefabs;
    protected List<ParticleSystem> m_damagedParticles = new(), m_destroyedParticles = new();
    
    [SerializeField] protected AudioGroupDetails m_collidedSFX, m_damagedSFX, m_destroyedSFX;

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
        SetAllPrefabsInactive();
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

            PlayParticle(m_collisionParticlePrefab);
        }
    }

    public virtual void DealDamage(float _damageAmount, PlayerAttack _player)
    {
        if (!m_isInvulnerable)
        {
            PlayParticle(m_damagedParticlePrefab);

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
        if (m_destroyedSFX != null) AudioManager.Instance.PlaySoundAtLocation(m_destroyedSFX.audioGroupName, transform.position);

        PlayParticle(m_destroyedParticlePrefab);

        GameObject particleParent = GameObject.Find("----Particles");
        if (particleParent && m_destroyedParticlePrefab) m_destroyedParticlePrefab.transform.SetParent(particleParent.transform);

        m_manager.AddGold(m_goldReward);
        if (_player)
        {
            if (_player.TryGetComponent(out Dishonour dishonour)) dishonour.AddDishonour(m_dishonourPunishment);
        }
        
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
            PlayParticle(m_respawnParticlePrefab);
            m_health = m_maxHealth;
            gameObject.SetActive(true);
            Init();
            m_destroyedParticlePrefab.transform.SetParent(transform);
        }, m_respawnTime);
    }

    void PlayParticle(GameObject pp)
    {

        if (pp)
        {
            if (!pp.scene.isLoaded)
            {
                // Instantiates the particle if the reference is a prefab and not an instance
                pp = Instantiate(pp, transform);
                pp.transform.localPosition = Vector3.zero;
                pp.SetActive(false);
            }

            pp.SetActive(true);
            TimerManager.RunAfterTime(() => { pp.SetActive(false); }, 2);

        }
    }

    void SetAllPrefabsInactive()
    {
        m_collisionParticlePrefab.SetActive(false); m_damagedParticlePrefab.SetActive(false); m_destroyedParticlePrefab.SetActive(false);
    }

    //void PlayParticle(List<ParticleSystem> _particleList, List<GameObject> _prefabList)
    //{
    //    // Instantiate particles if they don't exist
    //    if (_particleList.Count == 0)
    //    {
    //        for (int i = 0; i < _prefabList.Count; i++)
    //        {
    //            ParticleSystem damagedParticle;
    //            damagedParticle = Instantiate(_prefabList[i], transform).GetComponent<ParticleSystem>();
    //            damagedParticle.transform.localPosition = Vector3.zero;

    //            GameObject particleParent = GameObject.Find("----Particles");
    //            if (particleParent) damagedParticle.transform.SetParent(particleParent.transform);

    //            _particleList.Add(damagedParticle);
    //        }
    //    }

    //    // Play random particle
    //    if (_particleList.Count > 0)
    //    {
    //        int randIndex = UnityEngine.Random.Range(0, _prefabList.Count);

    //        //_particleList[randIndex].transform.SetParent(null);
    //        _particleList[randIndex].Play();
    //    }
    //}
}
