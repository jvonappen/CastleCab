using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KnockbackObject : MonoBehaviour
{
    [SerializeField] LayerMask m_collisionLayer;

    [SerializeField] float m_knockbackStrengthMulti = 1;
    public float knockbackStrengthMulti { get { return m_knockbackStrengthMulti; } }

    Collider m_collider;
    Rigidbody rb;
    Knockback m_knockback;

    [SerializeField] private AudioGroupDetails audioGroup;
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
            if (audioGroup != null) AudioManager.Instance.PlaySoundAtLocation(audioGroup.audioGroupName, transform.position);

            Knockback kb = rb.GetComponent<Knockback>();

            Vector3 contactPoint = collision.GetContact(0).point;

            if (kb) kb.KnockBack((rb.transform.position - contactPoint).normalized, contactPoint, m_knockbackStrengthMulti);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        Rigidbody otherRB = other.attachedRigidbody;
        if (!otherRB) return;

        if (CheckLayer(otherRB.gameObject.layer))
        {
            Knockback kb = otherRB.GetComponent<Knockback>();
            if (kb)
            {
                Vector3 contactPoint = transform.position;
                if (!(m_collider.GetType() == typeof(MeshCollider) && !((MeshCollider)m_collider).convex)) contactPoint = m_collider.ClosestPoint(transform.position);

                kb.KnockBack((otherRB.transform.position - transform.position).normalized, contactPoint, m_knockbackStrengthMulti);

                if ((m_knockback && !m_knockback.CompareTag("Player")) || kb.gameObject.CompareTag("Player")) // Prevents player x non-player knockback for player
                {
                    if (m_knockback) m_knockback.KnockBack((transform.position - otherRB.transform.position).normalized, contactPoint, m_knockbackStrengthMulti);
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
