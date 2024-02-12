using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput m_playerInput;

    [SerializeField] Rigidbody rb;
    [SerializeField] Rigidbody wagon;
    CustomDrag m_wagonDrag;

    bool m_isAccelerating;
    bool m_isReversing;

    [Header("Grounded")]
    [SerializeField] LayerMask m_groundLayer;
    [SerializeField] Transform m_raycastPoint;
    [SerializeField] float m_groundRayLength = 1;
    bool m_isGrounded = false;

    // Speed
    [Header("Speed")]
    [SerializeField] float m_maxSpeed = 10;
    [SerializeField] float m_accelerationRate = 0.5f, m_decelerationRate = 1;
    float m_currentSpeed;

    [SerializeField] float m_maxReverseSpeed = 10, m_reverseAccelerationRate = 0.5f, m_reverseDecelerationRate = 1;

    [Header("Turning")]
    [SerializeField] float m_maxTurnSpeed = 10;
    float m_turnInput = 0;

    [Header("Cart Control")]
    [SerializeField] float m_accelerateNoTurnAngularDrag = 20;
    float m_defaultWagonAngularDrag;
    [SerializeField] float m_turningDrag = 0.1f;
    float m_defaultWagonDrag;


    private void Start()
    {
        m_playerInput = GetComponent<PlayerInput>();

        m_defaultWagonAngularDrag = wagon.angularDrag;

        m_wagonDrag = wagon.GetComponent<CustomDrag>();
        m_defaultWagonDrag = m_wagonDrag.dragX;

        #region Delegates
        m_playerInput.m_playerControls.Controls.Acceleration.performed += OnAccelerate;
        m_playerInput.m_playerControls.Controls.Acceleration.canceled += OnDecelerate;

        m_playerInput.m_playerControls.Controls.Reverse.performed += OnReversePerformed;
        m_playerInput.m_playerControls.Controls.Reverse.canceled += OnReverseCancelled;

        m_playerInput.m_playerControls.Controls.Steering.performed += OnSteeringPerformed;
        m_playerInput.m_playerControls.Controls.Steering.canceled += OnSteeringCancelled;
        #endregion
    } 

    #region Events
    void OnAccelerate(InputAction.CallbackContext context)
    {
        m_isAccelerating = true;
        if (m_turnInput == 0) OnAccelerateNoTurn();
        else OnAccelerateTurn();
    }

    void OnDecelerate(InputAction.CallbackContext context)
    {
        m_isAccelerating = false;
        if (m_turnInput == 0) OnAccelerateNoTurnCancel();
        else OnAccelerateTurnCancel();
    }

    void OnReversePerformed(InputAction.CallbackContext context) => m_isReversing = true;
    void OnReverseCancelled(InputAction.CallbackContext context) => m_isReversing = false;

    void OnSteeringPerformed(InputAction.CallbackContext context)
    {
        m_turnInput = context.ReadValue<float>();
        if (m_isAccelerating)
        {
            OnAccelerateNoTurnCancel();
            OnAccelerateTurn();
        }
        if (m_isGrounded && m_turningDrag != 0)
        {
            m_wagonDrag.dragX = m_turningDrag;
            m_wagonDrag.dragZ = m_turningDrag;
        }
    }
    void OnSteeringCancelled(InputAction.CallbackContext context)
    {
        m_turnInput = 0;
        if (m_isAccelerating)
        {
            OnAccelerateNoTurn();
            OnAccelerateTurnCancel();
        }
        if (m_turningDrag != 0)
        {
            m_wagonDrag.dragX = m_defaultWagonDrag;
            m_wagonDrag.dragZ = m_defaultWagonDrag;
        }
    }

    void OnBeginGrounded()
    {
        if (m_turnInput != 0)
        {
            m_wagonDrag.dragX = m_turningDrag;
            m_wagonDrag.dragZ = m_turningDrag;
        }
    }

    void OnExitGrounded()
    {
        if (m_turnInput != 0)
        {
            m_wagonDrag.dragX = m_defaultWagonDrag;
            m_wagonDrag.dragZ = m_defaultWagonDrag;
        }
    }
    #endregion

    private void FixedUpdate()
    {
        GroundCheck();
        Turn();
        MoveVelocity();
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(m_raycastPoint.position, Vector3.down, out hit, m_groundRayLength, m_groundLayer))
        {
            if (!m_isGrounded) OnBeginGrounded();
            m_isGrounded = true;
            rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, Quaternion.FromToRotation(rb.transform.up, hit.normal) * rb.transform.rotation, Time.fixedDeltaTime * 10.0f);
        }
        else
        {
            if (m_isGrounded) OnExitGrounded();
            m_isGrounded = false;
        }
    }

    void MoveVelocity()
    {
        #region CalculateSpeed
        if (m_isAccelerating)
        {
            if (m_isReversing)
            {
                // Player is reversing and accelerating simultaniously

                // TODO
            }
            else
            {
                // Accelerate if player is accelerating and isn't at max speed
                if (m_currentSpeed < m_maxSpeed)
                {
                    m_currentSpeed += Time.fixedDeltaTime * m_accelerationRate;
                    if (m_currentSpeed > m_maxSpeed) m_currentSpeed = m_maxSpeed; // Caps speed at max
                }
            }
        }
        else if (m_isReversing)
        {
            // Decelerates if player is reversing and isn't at max reverse speed.
            // Uses deceleration rate instead of reverse acceleration if it is still going forward to prevent sliding 
            if (m_currentSpeed > 0) m_currentSpeed -= Time.fixedDeltaTime * m_decelerationRate; 
            else if (m_currentSpeed > -m_maxReverseSpeed) // Accelerates in reverse with different rate once player starts moving backwards
            {
                m_currentSpeed -= Time.fixedDeltaTime * m_reverseAccelerationRate;
                if (m_currentSpeed < -m_maxReverseSpeed) m_currentSpeed = -m_maxReverseSpeed; // Caps speed (in negative because it is moving backwards)
            }
        }
        else
        {
            // If neither moving forward or backward, decelerate to 0, rate based on if player is reversing or moving forward
            if (m_currentSpeed > 0) // Lower speed (player was moving forward)
            {
                m_currentSpeed -= Time.fixedDeltaTime * m_decelerationRate;
                if (m_currentSpeed < 0) m_currentSpeed = 0;
            }
            else if (m_currentSpeed < 0) // Increase speed (player was reversing)
            {
                m_currentSpeed += Time.fixedDeltaTime * m_reverseDecelerationRate;
                if (m_currentSpeed > 0) m_currentSpeed = 0;
            }
        }
        #endregion

        // Apply velocity based on calculated speed, Without affecting y velocity
        if (m_currentSpeed != 0)
        {
            float velY = rb.velocity.y;
            rb.velocity = rb.transform.forward * m_currentSpeed;
            rb.velocity = new Vector3(rb.velocity.x, velY, rb.velocity.z);
        }
    }

    //void MoveForce()
    //{
    //    if (m_isAccelerating) rb.AddForce(rb.transform.forward * m_maxSpeed);
    //    if (m_isReversing) rb.AddForce(-rb.transform.forward * m_maxSpeed);
    //}

    void Turn()
    {
        if (m_turnInput != 0) rb.transform.rotation = Quaternion.Euler(rb.transform.rotation.eulerAngles + new Vector3(0f, m_turnInput * m_maxTurnSpeed * Time.deltaTime, 0f));
    }

    #region CartPhysics
    void OnAccelerateNoTurn()
    {
        wagon.angularDrag = m_accelerateNoTurnAngularDrag;
    }

    void OnAccelerateNoTurnCancel()
    {
        wagon.angularDrag = m_defaultWagonAngularDrag;
    }

    void OnAccelerateTurn()
    {
        
    }

    void OnAccelerateTurnCancel()
    {

    }
    #endregion
}
