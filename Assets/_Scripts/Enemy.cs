using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Health m_health;

    private void Awake()
    {
        m_health.onDamaged += OnDamaged;
        m_health.onDeath += OnDeath;
    }

    private void OnTriggerEnter(Collider other) => OnCollision(other);
    private void OnCollisionEnter(Collision collision) => OnCollision(collision.collider);

    void OnCollision(Collider _collider)
    {
        if (_collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerAttack player = _collider.attachedRigidbody.transform.parent.GetComponent<PlayerAttack>();
            if (player)
            {
                float playerForce = _collider.attachedRigidbody.velocity.magnitude;
                Debug.Log("player force = " + playerForce);

                if (playerForce > player.GetDamageForce())
                {
                    m_health.DealDamage(player.GetDamage());
                }
            }
            
        }
    }

    void OnDamaged(float _damageAmount)
    {
        // Display damage popup text
    }

    void OnDeath()
    {
        Destroy(gameObject);
    }
}
