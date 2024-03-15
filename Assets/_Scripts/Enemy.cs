using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Health m_health;

    [SerializeField] Transform m_popupLocation;
    [SerializeField] float m_popupRandomRange = 5, m_fontSize = 20;

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

                if (playerForce > player.GetDamageForce())
                {
                    m_health.DealDamage(player.GetDamage(), player);
                }
            }
            
        }
    }

    void OnDamaged(float _damageAmount, PlayerAttack _player)
    {
        // Display damage popup text
        PopupDisplay.Spawn(m_popupLocation.position, m_popupRandomRange, _damageAmount.ToString(), m_fontSize, Color.white, Vector3.up * 3, null, _player.transform.GetChild(0));
    }

    void OnDeath()
    {
        Destroy(gameObject);
    }
}
