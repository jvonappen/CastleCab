using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    [SerializeField] PlayerMovement m_movement;

    Rigidbody rb;
    [SerializeField] float m_downForce;

    private void Start() => rb = m_movement.rb;

    private void FixedUpdate()
    {
        if (m_movement.isGrounded)
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - (m_downForce * Time.fixedDeltaTime), rb.velocity.z);
    }
        
}
