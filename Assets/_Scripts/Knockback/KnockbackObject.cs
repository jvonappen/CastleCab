using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KnockbackObject : MonoBehaviour
{
    [SerializeField] LayerMask m_collisionLayer;

    Collider m_collider;
    Rigidbody rb;
    Knockback m_knockback;
    private void Awake()
    {
        m_collider = GetComponent<Collider>();
        rb = m_collider.attachedRigidbody;
        if (rb) rb.TryGetComponent(out m_knockback);
    }

    bool CheckLayer(int _layer)
    {
        if (m_collisionLayer == (m_collisionLayer | (1 << _layer))) return true;
        else return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (!rb) return;

        if (CheckLayer(rb.gameObject.layer))
        {
            Knockback kb = rb.GetComponent<Knockback>();

            Vector3 contactPoint = collision.GetContact(0).point;

            if (kb) kb.KnockBack((rb.transform.position - contactPoint).normalized, contactPoint);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRB = other.attachedRigidbody;
        if (!otherRB) return;

        if (CheckLayer(otherRB.gameObject.layer))
        {
            Knockback kb = otherRB.GetComponent<Knockback>();
            if (kb)
            {
                Vector3 contactPoint = m_collider.ClosestPoint(transform.position);

                kb.KnockBack((otherRB.transform.position - transform.position).normalized, contactPoint);

                if ((m_knockback && !m_knockback.CompareTag("Player")) || kb.gameObject.CompareTag("Player")) // Prevents player x non-player knockback for player
                {
                    if (m_knockback) m_knockback.KnockBack((transform.position - otherRB.transform.position).normalized, contactPoint);
                }
                else
                {
                    if (m_knockback)
                    {
                        TimerManager.RunUntilTime(() => 
                        {
                            if (rb.velocity.y > 0) rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                        }, 0.4f);
                    }
                }
            }
        }  
    }
}
