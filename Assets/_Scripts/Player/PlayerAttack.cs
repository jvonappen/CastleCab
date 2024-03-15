using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAttack : MonoBehaviour
{
    PlayerMovement m_playerMovement;

    [SerializeField] float m_baseDamage = 2, m_driftDamageMulti = 2, m_boostDamageMulti = 2.5f, m_hurricaneDamageMulti = 4f;

    [SerializeField] float m_damageMultiPerStat = 0.075f;

    [Space(10)]
    [SerializeField] [Tooltip("Minimum force required to damage entity")] float m_damageForce = 18;
    [SerializeField] float m_abilityDamageForce = 9;

    private void Awake() => m_playerMovement = GetComponent<PlayerMovement>();

    public float GetDamage()
    {
        float currentDamage = m_baseDamage;

        if (m_playerMovement.isDrifting) currentDamage += (m_baseDamage * m_driftDamageMulti) - m_baseDamage;

        if (m_playerMovement.isBoosting) currentDamage += (m_baseDamage * m_boostDamageMulti) - m_baseDamage;
        else if (m_playerMovement.isHurricane) currentDamage += (m_baseDamage * m_hurricaneDamageMulti) - m_baseDamage;

        float multi = SharedPlayerStats.attackPoints * m_damageMultiPerStat + 1;

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
}
