using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Knockback : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float m_force = 15, m_velY = 10;

    private void Awake() => Init();
    protected virtual void Init() => rb = GetComponent<Rigidbody>();

    public virtual void KnockBack(Vector3 _dir)
    {
        rb.velocity = _dir * m_force;
        rb.velocity = new Vector3(rb.velocity.x, m_velY, rb.velocity.z);
    }
}
