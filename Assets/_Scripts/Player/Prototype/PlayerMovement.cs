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

    #region Grounded
    [System.Serializable]
    public struct Grounded
    {
        [SerializeField] internal LayerMask m_groundLayer;
        [SerializeField] internal Transform m_raycastPoint;
        [SerializeField] internal float m_groundRayLength;
    }
    bool m_isGrounded;
    [SerializeField] Grounded _Grounded;
    #endregion

    #region Speed
    [System.Serializable]
    public struct Speed
    {
        [SerializeField] internal float m_maxSpeed;
        [SerializeField] internal float m_accelerationRate, m_decelerationRate;

        [SerializeField] internal float m_maxReverseSpeed, m_reverseAccelerationRate, m_reverseDecelerationRate;
    }
    float m_currentSpeed;
    bool m_attemptingAccelerate;

    Vector3 prevDir;

    [SerializeField] Speed _Speed;
    #endregion

    #region Turning
    [System.Serializable]
    public struct Turning
    {
        [SerializeField] internal float m_defaultTurnSpeed;
        [SerializeField] internal float m_turnOnSpotSpeed, m_turnInAirSpeed;
    }
    float m_turnInput = 0;

    [SerializeField] Turning _Turning;
    #endregion

    #region Drifting
    [System.Serializable]
    public struct Drifting
    {
        [SerializeField] internal float m_driftMoveMultiplier;
        [SerializeField] internal float m_driftMinTurnSpeed, m_driftMaxTurnSpeed, m_driftTurnAcceleration;
        [SerializeField] internal float m_driftStrengthMultiplier;
    }
    float m_currentDriftTurnSpeed = 50;
    bool m_isDrifting, m_attemptingDrift;
    float m_driftTurnInput;

    [SerializeField] Drifting _Drifting;
    #endregion

    #region CartControl
    [System.Serializable]
    public struct CartControl
    {
        [SerializeField] internal float m_accelerateNoTurnAngularDrag;
        [SerializeField] internal float m_turningDrag, m_boostTurnDrag;
    }
    float m_defaultWagonAngularDrag;
    float m_defaultWagonDrag;

    [SerializeField] CartControl _CartControl;
    #endregion

    #region Boost
    [System.Serializable]
    public struct Boost
    {
        [SerializeField] internal float m_maxBoostSpeed;
        [SerializeField] internal float m_boostAccelerationRate, m_boostDecelerationRate;
        [SerializeField] internal float m_boostCostPerSec, m_boostRegenPerSec, m_boostRegenCooldown;
    }
    float m_boostRegenTimer;
    bool m_isBoosting;

    [SerializeField] Boost _Boost;
    #endregion

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

    #region Drift
    void OnDriftPerformed(InputAction.CallbackContext context)
    {
        if (m_turnInput < 0) m_cam.camOffset = RotatePointAroundPivot(m_cam.m_originalCameraOffset, Vector3.zero, Vector3.up * 45);
        else if (m_turnInput > 0) m_cam.camOffset = RotatePointAroundPivot(m_cam.m_originalCameraOffset, Vector3.zero, -Vector3.up * 45);

        m_cam.TweenToOriginalCamPosition(1);

        m_attemptingDrift = true;

        if (!m_isDrifting)
        {
            if (m_turnInput != 0)
            {
                OnTurnDrift();
            }
        }
    }
    void OnDriftCanceled(InputAction.CallbackContext context)
    {
        m_isDrifting = false;
        m_attemptingDrift = false;

        if (!m_attemptingAccelerate) m_isAccelerating = false;
    }

    void OnTurnDrift()
    {
        m_isAccelerating = true;

        m_isDrifting = true;
        m_currentDriftTurnSpeed = _Drifting.m_driftMinTurnSpeed;

        Vector3 rbRot = rb.transform.eulerAngles;
        if (m_turnInput < 0) m_driftTurnInput = -1;
        else m_driftTurnInput = 1;

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
        if (Physics.Raycast(_Grounded.m_raycastPoint.position, Vector3.down, out hit, _Grounded.m_groundRayLength, _Grounded.m_groundLayer))
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
                    if (m_boostRegenTimer < _Boost.m_boostRegenCooldown) m_boostRegenTimer += Time.fixedDeltaTime;
                    else
                    {
                        // Boost is recharging
                        m_boostBar.progress += Time.fixedDeltaTime * _Boost.m_boostRegenPerSec;
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
                    if (m_currentSpeed < _Speed.m_maxSpeed)
                    {
                        m_currentSpeed += Time.fixedDeltaTime * _Speed.m_accelerationRate;
                        if (m_currentSpeed > _Speed.m_maxSpeed) m_currentSpeed = _Speed.m_maxSpeed; // Caps speed at max
                    }
                    else if (m_currentSpeed > _Speed.m_maxSpeed)
                    {
                        m_currentSpeed -= Time.fixedDeltaTime * _Boost.m_boostDecelerationRate;
                        if (m_currentSpeed < _Speed.m_maxSpeed) m_currentSpeed = _Speed.m_maxSpeed;
                    }
                }
                else
                {
                    // Accelerate if player is boosting and isn't at max boost speed
                    if (m_currentSpeed <= _Boost.m_maxBoostSpeed)
                    {
                        if (m_currentSpeed < _Boost.m_maxBoostSpeed)
                        {
                            m_currentSpeed += Time.fixedDeltaTime * _Boost.m_boostAccelerationRate;
                            if (m_currentSpeed > _Boost.m_maxBoostSpeed) m_currentSpeed = _Boost.m_maxBoostSpeed; // Caps speed at max
                        }
                        
                        if (m_boostBar)
                        {
                            m_boostBar.progress -= Time.fixedDeltaTime * _Boost.m_boostCostPerSec;
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
            if (m_currentSpeed > 0) m_currentSpeed -= Time.fixedDeltaTime * _Speed.m_decelerationRate;
            else if (m_currentSpeed > -_Speed.m_maxReverseSpeed) // Accelerates in reverse with different rate once player starts moving backwards
            {
                m_currentSpeed -= Time.fixedDeltaTime * _Speed.m_reverseAccelerationRate;
                if (m_currentSpeed < -_Speed.m_maxReverseSpeed) m_currentSpeed = -_Speed.m_maxReverseSpeed; // Caps speed (in negative because it is moving backwards)
            }
        }
        else
        {
            // If neither moving forward or backward, decelerate to 0, rate based on if player is reversing or moving forward
            if (m_currentSpeed > 0) // Lower speed (player was moving forward)
            {
                m_currentSpeed -= Time.fixedDeltaTime * _Speed.m_decelerationRate;
                if (m_currentSpeed < 0) m_currentSpeed = 0;
            }
            else if (m_currentSpeed < 0) // Increase speed (player was reversing)
            {
                m_currentSpeed += Time.fixedDeltaTime * _Speed.m_reverseDecelerationRate;
                if (m_currentSpeed > 0) m_currentSpeed = 0;
            }
        }
        #endregion

        m_animator.SetFloat("Speed", m_currentSpeed / _Speed.m_maxSpeed);

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

                    dir *= _Drifting.m_driftMoveMultiplier;
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
        float turnSpeed = _Turning.m_defaultTurnSpeed;
        if (m_isDrifting)
        {
            turnInput = m_driftTurnInput;

            if (m_currentDriftTurnSpeed < _Drifting.m_driftMaxTurnSpeed)
            {
                m_currentDriftTurnSpeed += Time.fixedDeltaTime * _Drifting.m_driftTurnAcceleration;
                if (m_currentDriftTurnSpeed > _Drifting.m_driftMaxTurnSpeed) m_currentDriftTurnSpeed = _Drifting.m_driftMaxTurnSpeed;
            }

            // TODO - fix so that going opposite direction of drift makes player go straight
            float turnStrength = m_turnInput + 1;
            if (turnInput < 0) turnStrength = Mathf.Abs(m_turnInput - 1);

            turnSpeed = m_currentDriftTurnSpeed * turnStrength * _Drifting.m_driftStrengthMultiplier;
        }
        else if (!m_isGrounded && !m_isBoosting) turnSpeed = _Turning.m_turnInAirSpeed;
        else if (!m_isAccelerating && !m_isReversing) turnSpeed = _Turning.m_turnOnSpotSpeed;

        if (turnInput != 0) rb.transform.rotation = Quaternion.Euler(rb.transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnSpeed * Time.deltaTime, 0f));
    }

    void SetTurnDrag()
    {
        if (m_isBoosting)
        {
            m_wagonDrag.dragX = _CartControl.m_boostTurnDrag;
            m_wagonDrag.dragZ = _CartControl.m_boostTurnDrag;
        }
        else
        {
            m_wagonDrag.dragX = _CartControl.m_turningDrag;
            m_wagonDrag.dragZ = _CartControl.m_turningDrag;
        }
    }
    #endregion

    #region CartPhysics
    void OnAccelerateNoTurn() => wagon.angularDrag = _CartControl.m_accelerateNoTurnAngularDrag;
    void OnAccelerateNoTurnCancel() => wagon.angularDrag = m_defaultWagonAngularDrag;
    #endregion

    #region Helper
    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }
    #endregion
}
