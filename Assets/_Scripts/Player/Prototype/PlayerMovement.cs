using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput m_playerInput;

    [SerializeField] Rigidbody rb;

    bool m_isAccelerating;
    bool m_isReversing;

    // Speed
    [SerializeField] float m_maxSpeed = 10, m_accelerationRate = 0.5f, m_decelerationRate = 1;
    float m_currentSpeed;

    [SerializeField] float m_maxReverseSpeed = 10, m_reverseAccelerationRate = 0.5f, m_reverseDecelerationRate = 1;
    float m_reverseSpeed;

    private void Start()
    {
        m_playerInput = GetComponent<PlayerInput>();

        m_playerInput.m_playerControls.Controls.Acceleration.performed += OnAccelerate;
        m_playerInput.m_playerControls.Controls.Acceleration.canceled += OnDecelerate;

        m_playerInput.m_playerControls.Controls.Reverse.performed += OnReversePerformed;
        m_playerInput.m_playerControls.Controls.Reverse.canceled += OnReverseCancelled;
    }

    private void FixedUpdate()
    {
        MoveVelocity();
    }

    void MoveVelocity()
    {
        if (m_isAccelerating)
        {
            // Accelerate if player is accelerating and isn't at max speed
            if (m_currentSpeed < m_maxSpeed)
            {
                m_currentSpeed += Time.fixedDeltaTime * m_accelerationRate;
                if (m_currentSpeed > m_maxSpeed) m_currentSpeed = m_maxSpeed; // Caps speed
            }
            
            rb.velocity = transform.forward * m_currentSpeed;
        }
        else
        {
            // Decelerates
            if (m_currentSpeed > 0)
            {
                m_currentSpeed -= Time.fixedDeltaTime * m_decelerationRate;
                if (m_currentSpeed < 0) m_currentSpeed = 0; 

                rb.velocity = transform.forward * m_currentSpeed;
            }
        }

        if (m_isReversing)
        {
            // Accelerate if player is reversing and isn't at max speed
            if (m_reverseSpeed < m_maxReverseSpeed)
            {
                m_reverseSpeed += Time.fixedDeltaTime * m_reverseAccelerationRate;
                if (m_reverseSpeed > m_maxReverseSpeed) m_reverseSpeed = m_maxReverseSpeed; // Caps speed
            }

            rb.velocity = -transform.forward * m_reverseSpeed;
        }
        else
        {
            // Decelerates
            if (m_reverseSpeed > 0)
            {
                m_reverseSpeed -= Time.fixedDeltaTime * m_reverseDecelerationRate;
                if (m_reverseSpeed < 0) m_reverseSpeed = 0;

                rb.velocity = -transform.forward * m_reverseSpeed;
            }
        }
    }

    void MoveForce()
    {
        if (m_isAccelerating) rb.AddForce(transform.forward * m_maxSpeed);
        if (m_isReversing) rb.AddForce(-transform.forward * m_maxSpeed);
    }

    void OnAccelerate(InputAction.CallbackContext context) => m_isAccelerating = true;

    void OnDecelerate(InputAction.CallbackContext context) => m_isAccelerating = false;

    void OnReversePerformed(InputAction.CallbackContext context) => m_isReversing = true;
    void OnReverseCancelled(InputAction.CallbackContext context) => m_isReversing = false;
}
