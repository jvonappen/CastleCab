using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Health
{
    [Header("Explosive")]
    [SerializeField] GameObject m_particlePrefab;
    public LayerMask m_explodeLayer;
    public float explosionRadius = 30f;
    public float explosionForce = 200f;
    public float verticalLaunchForce = 100f; // Adjust the vertical launch force as needed.
    public float rotationForce = 500f; // Adjust the rotation force as needed.

    [SerializeField] int m_explosionDamage = 3;

    ParticleSystem m_particle;

    protected override void Init()
    {
        base.Init();

        m_particle = Instantiate(m_particlePrefab, transform).GetComponent<ParticleSystem>();
        m_particle.transform.localPosition = Vector3.zero;
    }

    protected override void Die(PlayerAttack _player)
    {
        Explode(_player);

        base.Die(_player);
    }

    public void Explode(PlayerAttack _player)
    {
        m_particle.transform.SetParent(null);
        m_particle.Play();

        m_particle.GetComponent<CFX_AutoDestructShuriken>().enabled = true;

        // Find all colliders within the explosion radius.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {

            // Apply an explosion force to rigidbodies.
            Rigidbody rb = collider.attachedRigidbody;// .GetComponentInChildren<Rigidbody>();
            if (rb != null)
            {

                // Calculate direction from the explosion center to the rigidbody.
                Vector3 explosionDir = (rb.transform.position - transform.position).normalized;

                if (rb.TryGetComponent(out Knockback knockback))
                {
                    if (knockback is PlayerKnockback)
                    {
                        ((PlayerKnockback)knockback).playerMovement.EndHurricane();
                    }
                    knockback.KnockBack(explosionDir, Vector3.up);
                }
                else
                {
                    if (m_explodeLayer == (m_explodeLayer | (1 << rb.gameObject.layer)))
                    {
                        if (rb.TryGetComponent(out Health health))
                        {
                            TimerManager.RunAfterTime(() =>
                            {
                                m_isInvulnerable = true;
                                health.DealDamage(m_explosionDamage, _player);
                            }, 0.6f);
                        }

                        rb.AddExplosionForce(explosionForce * 1000, transform.position, explosionRadius);

                        //Apply vertical launch force.
                        rb.AddForce(Vector3.up * verticalLaunchForce, ForceMode.Impulse);

                        Vector3 randomRotation = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                        rb.AddTorque(randomRotation * rotationForce);
                    }
                }

            }
        }
    }
}
