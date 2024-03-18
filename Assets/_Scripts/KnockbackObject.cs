using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KnockbackObject : MonoBehaviour
{
    Knockback m_knockback;
    private void Awake()
    {
        Rigidbody rb = GetComponent<Collider>().attachedRigidbody;
        if (rb) rb.TryGetComponent(out m_knockback);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (!rb) return;

        Knockback kb = rb.GetComponent<Knockback>();
        if (kb) kb.KnockBack((rb.transform.position - collision.GetContact(0).point).normalized);
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (!rb) return;

        Knockback kb = rb.GetComponent<Knockback>();
        if (kb)
        {
            kb.KnockBack((rb.transform.position - transform.position).normalized);
            if (m_knockback) m_knockback.KnockBack((transform.position - rb.transform.position).normalized);
        }
    }
}
