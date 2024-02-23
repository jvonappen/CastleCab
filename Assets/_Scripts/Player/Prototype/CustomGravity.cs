using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float m_downForce;

    private void FixedUpdate() =>
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - (m_downForce * Time.fixedDeltaTime), rb.velocity.z);
}
