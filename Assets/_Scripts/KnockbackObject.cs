using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KnockbackObject : MonoBehaviour
{
    Collider m_collider;
    Knockback m_knockback;
    private void Awake()
    {
        m_collider = GetComponent<Collider>();
        Rigidbody rb = m_collider.attachedRigidbody;
        if (rb) rb.TryGetComponent(out m_knockback);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (!rb) return;

        Knockback kb = rb.GetComponent<Knockback>();

        Vector3 contactPoint = collision.GetContact(0).point;

        if (kb) kb.KnockBack((rb.transform.position - contactPoint).normalized, contactPoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (!rb) return;

        Knockback kb = rb.GetComponent<Knockback>();
        if (kb)
        {
            Vector3 contactPoint = m_collider.ClosestPoint(transform.position);

            kb.KnockBack((rb.transform.position - transform.position).normalized, contactPoint);
            if (m_knockback) m_knockback.KnockBack((transform.position - rb.transform.position).normalized, contactPoint);
        }
    }
}
