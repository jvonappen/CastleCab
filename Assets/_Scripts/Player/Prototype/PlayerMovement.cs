using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    PlayerInputHandler m_playerInput;

    CameraFollow m_cam;

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
    bool m_attemptingAccelerate;
    
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
    [SerializeField] float m_driftStrengthMultiplier = 0.5f;
    float m_currentDriftTurnSpeed = 50;
    bool m_isDrifting, m_attemptingDrift;
    float m_driftTurnInput;

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

    Animator m_animator;

    #endregion

    #region Start/Update
    private void Start()
    {
        m_playerInput = GetComponent<PlayerInputHandler>();

        m_defaultWagonAngularDrag = wagon.angularDrag;

        m_wagonDrag = wagon.GetComponent<CustomDrag>();
        m_defaultWagonDrag = m_wagonDrag.dragX;

        m_animator = GetComponentInChildren<Animator>();

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

        m_playerInput.m_playerControls.Controls.Drift.performed += OnDriftPerformed;
        m_playerInput.m_playerControls.Controls.Drift.canceled += OnDriftCanceled;

        m_playerInput.m_playerControls.Controls.Hurricane.performed += OnHurricanePerformed;
        m_playerInput.m_playerControls.Controls.Hurricane.canceled += OnHurricaneCanceled;
        #endregion
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Turn();
        MoveVelocity();
    }
    #endregion

    #region Events

    #region Acceleration
    void OnAccelerate(InputAction.CallbackContext context)
    {
        m_attemptingAccelerate = true;
        m_isAccelerating = true;
        if (m_turnInput == 0) OnAccelerateNoTurn();
    }

    void OnDecelerate(InputAction.CallbackContext context)
    {
        m_attemptingAccelerate = false;
        if (!m_isDrifting) m_isAccelerating = false;
        if (m_turnInput == 0) OnAccelerateNoTurnCancel();
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

            if (m_attemptingDrift && !m_isDrifting) OnTurnDrift();
        }
        if (m_isGrounded) SetTurnDrag();
    }
    void OnSteeringCanceled(InputAction.CallbackContext context)
    {
        m_turnInput = 0;
        if (m_isAccelerating) OnAccelerateNoTurn();

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

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    #region Drift
    //Vector3 m_driftCamOffset;

    void OnDriftPerformed(InputAction.CallbackContext context)
    {
        //m_driftCamOffset = m_cam.camOffset;
        if (m_turnInput < 0) m_cam.camOffset = RotatePointAroundPivot(m_cam.m_originalCameraOffset, Vector3.zero, Vector3.up * 45);
        else if (m_turnInput > 0) m_cam.camOffset = RotatePointAroundPivot(m_cam.m_originalCameraOffset, Vector3.zero, -Vector3.up * 45);

        m_cam.TweenToOriginalCamPosition(1);

        //m_cam.camSpeed = 10;

        m_attemptingDrift = true;

        if (!m_isDrifting)
        {
            if (m_turnInput != 0/* && m_isAccelerating*/)
            {
                OnTurnDrift();
            }
        }
    }
    void OnDriftCanceled(InputAction.CallbackContext context)
    {
        m_isDrifting = false;
        m_attemptingDrift = false;

        //m_cam.camSpeed = m_cam.m_originalCamSpeed;

        //m_cam.camOffset = m_cam.m_originalCameraOffset;

        if (!m_attemptingAccelerate) m_isAccelerating = false;
    }

    void OnTurnDrift()
    {
        m_isAccelerating = true;

        m_isDrifting = true;
        m_currentDriftTurnSpeed = m_driftMinTurnSpeed;

        Vector3 rbRot = rb.transform.eulerAngles;
        if (m_turnInput < 0) m_driftTurnInput = -1;
        else m_driftTurnInput = 1;
        //m_driftTurnInput = m_turnInput;

        Vector3 rotateAmount = new Vector3(0, 45, 0);
        if (m_turnInput > 0) rb.rotation = Quaternion.Euler(rbRot.x + rotateAmount.x, rbRot.y + rotateAmount.y, rbRot.z + rotateAmount.z);
        else if (m_turnInput < 0) rb.rotation = Quaternion.Euler(rbRot.x - rotateAmount.x, rbRot.y - rotateAmount.y, rbRot.z - rotateAmount.z);
    }

    #endregion

    #region Hurricane

    void OnHurricanePerformed(InputAction.CallbackContext context)
    {
        m_isHurricane = true;
        m_cam.camSpeed = 2;

        //m_cam.transform.LookAt(m_cam.lookAt.position + m_cam.lookAtOffset);

        m_cam.m_useOffsetOverride = true;
        m_cam.m_worldOffsetOverride = (m_cam.lookAt.position + m_cam.lookAt.TransformDirection(m_cam.camOffset)) - m_cam.lookAt.position;
    }

    void OnHurricaneCanceled(InputAction.CallbackContext context)
    {
        m_endingHurricane = true;

        m_cam.camSpeed = m_cam.m_originalCamSpeed;

        m_cam.m_useOffsetOverride = false;
    }

    #endregion

    #region Camera
    public void OnCameraSetTarget(CameraFollow _camFollow) => m_cam = _camFollow;
    #endregion

    #endregion

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

        m_animator.SetFloat("Speed", m_currentSpeed / m_maxSpeed);

        if (!m_isHurricane && !m_endingHurricane)
        {
            prevDir = rb.transform.forward;

            Vector3 dir = rb.transform.forward;
            if (m_isDrifting)
            {
                if (m_driftTurnInput != 0)
                {
                    if (m_driftTurnInput > 0) dir -= rb.transform.right;
                    else if (m_driftTurnInput < 0) dir += rb.transform.right;

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

            m_cam.transform.position += rb.velocity * Time.fixedDeltaTime;

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

        float turnInput = m_turnInput;
        float turnSpeed = m_defaultTurnSpeed;
        if (m_isDrifting)
        {
            turnInput = m_driftTurnInput;

            if (m_currentDriftTurnSpeed < m_driftMaxTurnSpeed)
            {
                m_currentDriftTurnSpeed += Time.fixedDeltaTime * m_driftTurnAcceleration;
                if (m_currentDriftTurnSpeed > m_driftMaxTurnSpeed) m_currentDriftTurnSpeed = m_driftMaxTurnSpeed;
            }

            // TODO - fix so that going opposite direction of drift makes player go straight
            float turnStrength = m_turnInput + 1;
            if (turnInput < 0) turnStrength = Mathf.Abs(m_turnInput - 1);

            turnSpeed = m_currentDriftTurnSpeed * turnStrength * m_driftStrengthMultiplier;
        }
        else if (!m_isGrounded && !m_isBoosting) turnSpeed = m_turnInAirSpeed;
        else if (!m_isAccelerating && !m_isReversing) turnSpeed = m_turnOnSpotSpeed;

        if (turnInput != 0) rb.transform.rotation = Quaternion.Euler(rb.transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnSpeed * Time.deltaTime, 0f));
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
    #endregion
}
