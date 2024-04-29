using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAttack : MonoBehaviour
{
    PlayerMovement m_playerMovement;
    public PlayerMovement playerMovement { get { return m_playerMovement; } }

    [SerializeField] float m_baseDamage = 2, m_driftDamageMulti = 2, m_boostDamageMulti = 2.5f, m_hurricaneDamageMulti = 4f;

    [SerializeField] float m_damageMultiPerStat = 0.075f;

    [Space(10)]
    [SerializeField] [Tooltip("Minimum force required to damage entity")] float m_damageForce = 18;
    [SerializeField] float m_abilityDamageForce = 9;

    [SerializeField] PlayerUpgrades m_playerUpgrades;

    public float GetVelocityAverage() => m_magnitudeOverSeconds.Average();

    List<float> m_magnitudeOverSeconds = new();
    const int BUFFER_SIZE = 5;

    float m_magnitudeUpdateFrequency = 0.2f, m_counter;

    private void Update()
    {
        if (m_counter >= m_magnitudeUpdateFrequency)
        {
            m_counter = 0;

            m_magnitudeOverSeconds.Add(m_playerMovement.rb.velocity.magnitude);
            if (m_magnitudeOverSeconds.Count > BUFFER_SIZE) m_magnitudeOverSeconds.RemoveAt(0);
        }
        else m_counter += Time.deltaTime;
    }

    private void Awake() => m_playerMovement = GetComponent<PlayerMovement>();

    public float GetDamage()
    {
        float currentDamage = m_baseDamage;

        if (m_playerMovement.isDrifting) currentDamage += (m_baseDamage * m_driftDamageMulti) - m_baseDamage;

        if (m_playerMovement.isBoosting) currentDamage += (m_baseDamage * m_boostDamageMulti) - m_baseDamage;
        else if (m_playerMovement.isHurricane) currentDamage += (m_baseDamage * m_hurricaneDamageMulti) - m_baseDamage;

        float multi = m_playerUpgrades.attackPoints * m_damageMultiPerStat + 1;

        return currentDamage * multi;
    }

    public float GetDamageForce()
    {
        if (m_playerMovement.isHurricane)
            return 0;
        else if (m_playerMovement.isDrifting || m_playerMovement.isBoosting)
            return m_abilityDamageForce;
        else
            return m_damageForce;
    }

    public void AttemptAttack(Collider _collider)
    {
        Rigidbody colliderRB = _collider.attachedRigidbody;

        Health health = null;
        if (!colliderRB)
        {
            health = _collider.GetComponent<Health>();
            if (!health) return;
        }

        if (!_collider.isTrigger)
        {
            if (!health) health = colliderRB.GetComponent<Health>();

            if (health)
            {
                float playerForce = m_playerMovement.rb.velocity.magnitude;
                float averagePlayerForce = GetVelocityAverage();
                if (playerForce < averagePlayerForce) playerForce = averagePlayerForce;

                if (playerForce > GetDamageForce())
                {
                    //health.gameObject.SetActive(false);
                    health.DealDamage(GetDamage(), this);
                }
            }
        }
    }
}
