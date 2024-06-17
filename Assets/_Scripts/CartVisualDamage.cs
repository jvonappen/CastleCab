using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartVisualDamage : MonoBehaviour
{
    [SerializeField] PlayerHealth m_health;
    Renderer m_renderer;

    private void OnEnable()
    {
        m_renderer = GetComponent<Renderer>();

        if (m_health) m_health.onHealthChanged += UpdateDamage;
    }
    private void OnDisable() { if (m_health) m_health.onHealthChanged -= UpdateDamage; }

    void UpdateDamage(float _oldVal, float _newVal)
    {
        if (m_renderer.material.HasFloat("_Damage_Progression"))
        {
            m_renderer.material.SetFloat("_Damage_Progression", 1 - (m_health.health / m_health.maxHealth));
        }
    }
}
