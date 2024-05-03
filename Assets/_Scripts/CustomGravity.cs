using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    [SerializeField] PlayerMovement m_movement;

    [SerializeField] Rigidbody rb;
    [SerializeField] float m_downForce;

    private void FixedUpdate()
    {
        if (m_movement.isGrounded)
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - (m_downForce * Time.fixedDeltaTime), rb.velocity.z);
    }
        
}
