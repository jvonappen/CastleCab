using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    PlayerInput m_playerInput;

    [SerializeField] Rigidbody rb;
    [SerializeField] Rigidbody wagon;
    CustomDrag m_wagonDrag;

    [SerializeField] ProgressBar m_boostBar;

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

    Vector3 prevDir;

    [Header("Turning")]
    [SerializeField] float m_defaultTurnSpeed = 150;
    [SerializeField] float m_turnOnSpotSpeed = 350, m_turnInAirSpeed = 400;
    float m_turnInput = 0;
    
    [Header("Drifting")]
    [SerializeField] float m_driftMoveMultiplier = 1;
    [SerializeField] float m_driftMinTurnSpeed = 50, m_driftMaxTurnSpeed = 250, m_driftTurnAcceleration = 10;
    [SerializeField] float m_driftBoostThreshold1 = 2, m_driftBoostThreshold2 = 3, m_driftBoostThreshold3 = 5;
    float m_currentDriftTurnSpeed = 50;
    bool m_isDrifting, m_startedDrift;

    [Header("Cart Control")]
    [SerializeField] float m_accelerateNoTurnAngularDrag = 20;
    float m_defaultWagonAngularDrag;
    [SerializeField] float m_turningDrag = 0.8f, m_boostTurnDrag = 0.95f;
    float m_defaultWagonDrag;

    [Header("Boost")]
    [SerializeField] float m_maxBoostSpeed = 25;
    [SerializeField] float m_boostAccelerationRate = 20, m_boostDecelerationRate = 30;
    [SerializeField] float m_boostCostPerSec = 10, m_boostRegenPerSec = 20, m_boostRegenCooldown = 2.5f;
    float m_boostRegenTimer;
    bool m_isBoosting;

    [Header("Hurricane")]
    [SerializeField] float m_hurricaneSpeed = 1500;
    bool m_isHurricane;
    bool m_endingHurricane;

    #endregion

    private void Start()
    {
        m_playerInput = GetComponent<PlayerInput>();

        m_defaultWagonAngularDrag = wagon.angularDrag;

        m_wagonDrag = wagon.GetComponent<CustomDrag>();
        m_defaultWagonDrag = m_wagonDrag.dragX;

        if (!m_boostBar) Debug.LogWarning("Boost bar reference not found");

        #region Delegates
        m_playerInput.m_playerControls.Controls.Acceleration.performed += OnAccelerate;
        m_playerInput.m_playerControls.Controls.Acceleration.canceled += OnDecelerate;

        m_playerInput.m_playerControls.Controls.Reverse.performed += OnReversePerformed;
        m_playerInput.m_playerControls.Controls.Reverse.canceled += OnReverseCanceled;

        m_playerInput.m_playerControls.Controls.Steering.performed += OnSteeringPerformed;
        m_playerInput.m_playerControls.Controls.Steering.canceled += OnSteeringCanceled;

        m_playerInput.m_playerControls.Controls.Boost.performed += OnBoostPerformed;
        m_playerInput.m_playerControls.Controls.Boost.canceled += OnBoostCanceled;

        m_playerInput.m_playerControls.Controls.TailWhip.performed += OnDriftPerformed;
        m_playerInput.m_playerControls.Controls.TailWhip.canceled += OnDriftCanceled;

        m_playerInput.m_playerControls.Controls.Hurricane.performed += OnHurricanePerformed;
        m_playerInput.m_playerControls.Controls.Hurricane.canceled += OnHurricaneCanceled;
        #endregion
    }

    #region Events

    #region Acceleration
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
    #endregion

    #region Reverse
    void OnReversePerformed(InputAction.CallbackContext context)
    {
        m_isReversing = true;
        m_isBoosting = false;
    }
    void OnReverseCanceled(InputAction.CallbackContext context) => m_isReversing = false;

    #endregion

    #region Turning
    void OnSteeringPerformed(InputAction.CallbackContext context)
    {
        m_turnInput = context.ReadValue<float>();
        if (m_isAccelerating)
        {
            OnAccelerateNoTurnCancel();
            OnAccelerateTurn();

            if (m_isDrifting) OnTurnDrift();
        }
        if (m_isGrounded)
        {
            SetTurnDrag();
        }
    }
    void OnSteeringCanceled(InputAction.CallbackContext context)
    {
        m_turnInput = 0;
        if (m_isAccelerating)
        {
            OnAccelerateNoTurn();
            OnAccelerateTurnCancel();
        }

        m_wagonDrag.dragX = m_defaultWagonDrag;
        m_wagonDrag.dragZ = m_defaultWagonDrag;
    }

    #endregion

    #region Boost

    void OnBoostPerformed(InputAction.CallbackContext context)
    {
        m_isBoosting = true;
        if (m_turnInput != 0) SetTurnDrag();

        m_boostRegenTimer = 0;
    }

    void OnBoostCanceled(InputAction.CallbackContext context)
    {
        m_isBoosting = false;
        if (m_turnInput != 0) SetTurnDrag();
    }

    #endregion

    #region Grounded

    void OnBeginGrounded()
    {
        if (m_turnInput != 0) SetTurnDrag();
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

    #region Drift

    void OnDriftPerformed(InputAction.CallbackContext context)
    {
        m_isDrifting = true;
        m_currentDriftTurnSpeed = m_driftMinTurnSpeed;

        OnTurnDrift();
    }
    void OnDriftCanceled(InputAction.CallbackContext context) => m_isDrifting = false;

    void OnTurnDrift()
    {
        if (m_isAccelerating)
        {
            m_startedDrift = true;
            Vector3 rbRot = rb.transform.eulerAngles;

            Vector3 rotateAmount = new Vector3(0, 75, 0);
            if (m_turnInput > 0) rb.rotation = Quaternion.Euler(rbRot.x + rotateAmount.x, rbRot.y + rotateAmount.y, rbRot.z + rotateAmount.z);
            else if (m_turnInput < 0) rb.rotation = Quaternion.Euler(rbRot.x - rotateAmount.x, rbRot.y - rotateAmount.y, rbRot.z - rotateAmount.z);
        }
    }

    private void LateUpdate()
    {
        if (m_startedDrift)
        {
            m_startedDrift = false;
            //rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }

    #endregion

    #region Hurricane

    void OnHurricanePerformed(InputAction.CallbackContext context)
    {
        m_isHurricane = true;
    }

    void OnHurricaneCanceled(InputAction.CallbackContext context)
    {
        m_endingHurricane = true;
    }

    #endregion

    #endregion

    private void FixedUpdate()
    {
        GroundCheck();
        Turn();
        MoveVelocity();
    }

    #region GroundCheck
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
    #endregion

    #region Move
    void MoveVelocity()
    {
        #region CalculateSpeed

        // Recharge boost
        if (m_boostBar)
        {
            if (!m_isBoosting)
            {
                if (m_boostBar.progress < 1)
                {
                    if (m_boostRegenTimer < m_boostRegenCooldown) m_boostRegenTimer += Time.fixedDeltaTime;
                    else
                    {
                        // Boost is recharging
                        m_boostBar.progress += Time.fixedDeltaTime * m_boostRegenPerSec;
                        m_boostBar.UpdateProgress();
                    }
                }
            }
            else if (m_boostBar.progress <= 0)
            {
                m_isBoosting = false;
                m_boostBar.progress = 0;
                m_boostBar.UpdateProgress();
            }
        }
        
        if (m_isAccelerating)
        {
            if (m_isReversing)
            {
                // Player is reversing and accelerating simultaniously

                // TODO
            }
            else
            {
                if (!m_isBoosting)
                {
                    // Accelerate if player is accelerating and isn't at max speed
                    if (m_currentSpeed < m_maxSpeed)
                    {
                        m_currentSpeed += Time.fixedDeltaTime * m_accelerationRate;
                        if (m_currentSpeed > m_maxSpeed) m_currentSpeed = m_maxSpeed; // Caps speed at max
                    }
                    else if (m_currentSpeed > m_maxSpeed)
                    {
                        m_currentSpeed -= Time.fixedDeltaTime * m_boostDecelerationRate;
                        if (m_currentSpeed < m_maxSpeed) m_currentSpeed = m_maxSpeed;
                    }
                }
                else
                {
                    // Accelerate if player is boosting and isn't at max boost speed
                    if (m_currentSpeed <= m_maxBoostSpeed)
                    {
                        if (m_currentSpeed < m_maxBoostSpeed)
                        {
                            m_currentSpeed += Time.fixedDeltaTime * m_boostAccelerationRate;
                            if (m_currentSpeed > m_maxBoostSpeed) m_currentSpeed = m_maxBoostSpeed; // Caps speed at max
                        }
                        
                        if (m_boostBar)
                        {
                            m_boostBar.progress -= Time.fixedDeltaTime * m_boostCostPerSec;
                            m_boostBar.UpdateProgress();
                        }
                    }
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

        if (!m_isHurricane && !m_endingHurricane)
        {
            prevDir = rb.transform.forward;

            Vector3 dir = rb.transform.forward;
            if (m_isDrifting)
            {
                if (m_turnInput != 0)
                {
                    if (m_turnInput > 0) dir = -rb.transform.right;
                    else if (m_turnInput < 0) dir = rb.transform.right;

                    dir *= m_driftMoveMultiplier;
                }
            }

            // Apply velocity based on calculated speed, Without affecting y velocity
            if (m_currentSpeed != 0)
            {
                float velY = rb.velocity.y;
                rb.velocity = dir * m_currentSpeed;
                rb.velocity = new Vector3(rb.velocity.x, velY, rb.velocity.z);
            }
        }
        else
        {
           rb.transform.rotation = Quaternion.Euler(rb.transform.rotation.eulerAngles + new Vector3(0f, m_hurricaneSpeed * Time.fixedDeltaTime, 0f));

            float velY = rb.velocity.y;
            rb.velocity = prevDir * m_currentSpeed;
            rb.velocity = new Vector3(rb.velocity.x, velY, rb.velocity.z);

            if (m_endingHurricane)
            {
                if (rb.transform.forward.x > prevDir.x - 2 && rb.transform.forward.x < prevDir.x + 2)
                {
                    if (rb.transform.forward.z > prevDir.z - 2 && rb.transform.forward.z < prevDir.z + 2)
                    {
                        m_isHurricane = false;
                        m_endingHurricane = false;

                        // Bug - makes player jump sometimes, maybe make player temporarily kinematic or kill velocity?
                        rb.transform.forward = prevDir;
                    }
                }
            }
        }

    }
    #endregion

    #region Turn
    void Turn()
    {
        if (m_isHurricane) return;

        float turnSpeed = m_defaultTurnSpeed;
        if (m_isDrifting)
        {
            if (m_currentDriftTurnSpeed < m_driftMaxTurnSpeed)
            {
                m_currentDriftTurnSpeed += Time.fixedDeltaTime * m_driftTurnAcceleration;
                if (m_currentDriftTurnSpeed > m_driftMaxTurnSpeed) m_currentDriftTurnSpeed = m_driftMaxTurnSpeed;
            }

            turnSpeed = m_currentDriftTurnSpeed;
        }
        else if (!m_isGrounded && !m_isBoosting) turnSpeed = m_turnInAirSpeed;
        else if (!m_isAccelerating && !m_isReversing) turnSpeed = m_turnOnSpotSpeed;

        if (m_turnInput != 0) rb.transform.rotation = Quaternion.Euler(rb.transform.rotation.eulerAngles + new Vector3(0f, m_turnInput * turnSpeed * Time.deltaTime, 0f));
    }

    void SetTurnDrag()
    {
        if (m_isBoosting)
        {
            m_wagonDrag.dragX = m_boostTurnDrag;
            m_wagonDrag.dragZ = m_boostTurnDrag;
        }
        else
        {
            m_wagonDrag.dragX = m_turningDrag;
            m_wagonDrag.dragZ = m_turningDrag;
        }
    }
    #endregion

    #region CartPhysics
    void OnAccelerateNoTurn() => wagon.angularDrag = m_accelerateNoTurnAngularDrag;
    void OnAccelerateNoTurnCancel() => wagon.angularDrag = m_defaultWagonAngularDrag;
    void OnAccelerateTurn() { }
    void OnAccelerateTurnCancel() { }
    #endregion
}
