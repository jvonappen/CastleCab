using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using URNTS;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    #region CanMove
    bool m_canMove = true;
    bool m_isSmackStunned;
    public bool isSmackStunned { get { return m_isSmackStunned; } set { m_isSmackStunned = value; } }

    #endregion

    #region References
    PlayerInputHandler m_playerInput;
    CameraFollow m_cam;

    Animator m_animator;

    CustomGravity m_customGravity;

    [SerializeField] PlayerUpgrades m_playerUpgrades;

    public Rigidbody rb;
    [SerializeField] Rigidbody wagon;
    public Transform horse { get { return rb.transform; } }

    [SerializeField] ProgressBar m_staminaBar;
    #endregion

    #region Grounded
    [System.Serializable]
    public struct Grounded
    {
        [SerializeField] internal LayerMask m_groundLayer;
        [SerializeField] internal Transform m_raycastPoint, m_secondaryPoint, m_thirdPoint;
        [SerializeField] internal float m_groundDist;
    }
    bool m_isGrounded;
    public bool isGrounded { get { return m_isGrounded; } }
    [SerializeField] Grounded _Grounded;
    #endregion

    #region Speed
    [System.Serializable]
    public struct Speed
    {
        [SerializeField] internal float m_maxSpeed;
        [SerializeField] internal float m_accelerationRate, m_decelerationRate;

        [SerializeField] internal float m_maxReverseSpeed, m_reverseAccelerationRate, m_reverseDecelerationRate;

        [SerializeField] internal float m_inAirMultiplier;
        [SerializeField] internal float m_maxVelY;

        [Space(5)]
        [SerializeField] internal float m_multiPerStatPoint;
    }
    float m_currentSpeed;
    public void SetCurrentSpeed(float _speed) => m_currentSpeed = _speed;
    public float currentSpeed { get { return m_currentSpeed; } }
    bool m_attemptingAccelerate;

    bool m_isAccelerating;
    bool m_isReversing;

    float m_accelerationAmount;

    float prevRotY;

    [SerializeField] Speed _Speed;
    #endregion

    #region Turning
    [System.Serializable]
    public struct Turning
    {
        [SerializeField] internal float m_defaultSpeed;
        [SerializeField] internal float m_onSpotSpeed, m_inAirSpeed;
    }
    float m_turnInput = 0;

    [SerializeField] Turning _Turning;
    #endregion

    #region Drifting
    [System.Serializable]
    public struct Drifting
    {
        [SerializeField] internal float m_moveMultiplier;
        [SerializeField] internal float m_minTurnSpeed, m_maxTurnSpeed, m_turnAcceleration;
        [SerializeField] internal float m_strengthMultiplier;
        [SerializeField] internal float m_startCooldown;
    }
    float m_currentDriftTurnSpeed = 50;
    bool m_isDrifting, m_attemptingDrift;
    float m_driftTurnInput;

    float m_driftCooldownTimer;

    public bool isDrifting { get { return m_isDrifting; } }
    [SerializeField] Drifting _Drifting;
    #endregion

    #region CartControl
    [System.Serializable]
    public struct CartControl
    {
        [SerializeField] internal float m_turningDrag, m_boostTurnDrag;
    }
    float m_defaultWagonDrag;
    CustomDrag m_wagonDrag;

    [Header("Misc")]
    [SerializeField] CartControl _CartControl;
    #endregion

    #region AirControl
    [System.Serializable]
    public struct AirControl
    {
        [SerializeField] internal float m_flipSpeed, m_rollSpeed, m_boostMultiplier;
    }

    bool m_isAirControl;
    [SerializeField] AirControl _AirControl;
    #endregion

    #region Stamina
    [System.Serializable]
    public struct Stamina
    {
        [SerializeField] internal float m_regenPerSec, m_regenCooldown;

        [Space(5)]
        [SerializeField] internal float m_decreasePercentPerStatPoint;
    }
    float m_staminaRegenTimer;

    [SerializeField] Stamina _Stamina;
    #endregion

    #region Boost
    [System.Serializable]
    public struct Boost
    {
        [SerializeField] internal float m_maxSpeed;
        [SerializeField] internal float m_accelerationRate, m_decelerationRate;
        [SerializeField] internal float m_staminaCostPerSec;
        [SerializeField] internal float m_camFOV, m_tweenSpeedFOV;
    }
    bool m_attemptingBoost;
    bool m_isBoosting;
    public bool isBoosting { get { return m_isBoosting; } }

    [Header("Ability")]
    [SerializeField] Boost _Boost;
    #endregion

    #region Hurricane
    [System.Serializable]
    public struct Hurricane
    {
        [SerializeField] internal float m_spinSpeed, m_moveSpeed, m_staminaCostPerSec;
    }
    bool m_isHurricane;
    bool m_endingHurricane;
    Vector2 m_directionMoveInput;

    public bool isHurricane { get { return m_isHurricane; } }
    [SerializeField] Hurricane _Hurricane;
    #endregion

    #region Actions

    public Action onStartedMovement;
    public Action onStoppedMovement;

    public Action onGrounded;
    public Action onExitGrounded;

    public Action onDrift;
    public Action onDriftCanceled;

    public Action onBoost;
    public Action onBoostCanceled;

    public Action onHurricane;
    public Action onHurricaneCanceled;

    #endregion

    #endregion

    #region Start/Update
    private void Start()
    {
        m_playerInput = GetComponent<PlayerInputHandler>();

        m_wagonDrag = wagon.GetComponent<CustomDrag>();
        m_defaultWagonDrag = m_wagonDrag.dragX;

        m_animator = GetComponentInChildren<Animator>();

        m_customGravity = GetComponentInChildren<CustomGravity>();
        m_customGravity.enabled = false;

        if (!m_staminaBar) Debug.LogWarning("Boost bar reference not found");

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

        m_playerInput.m_playerControls.Controls.DirectionInput.performed += DirectionMovePerformed;
        m_playerInput.m_playerControls.Controls.DirectionInput.canceled += DirectionMoveCanceled;

        m_playerInput.m_playerControls.Controls.AirControl.performed += OnAirControl;
        m_playerInput.m_playerControls.Controls.AirControl.canceled += OnAirControlCanceled;
        #endregion
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Turn();
        MoveVelocity();
    }

    private void Update()
    {
        if (m_driftCooldownTimer < _Drifting.m_startCooldown) m_driftCooldownTimer += Time.deltaTime;

        // Recharge stamina
        if (m_staminaBar & m_staminaBar.progress < 1)
        {
            if (m_staminaRegenTimer < _Stamina.m_regenCooldown) m_staminaRegenTimer += Time.deltaTime;
            else
            {
                m_staminaBar.progress += Time.deltaTime * _Stamina.m_regenPerSec;
                m_staminaBar.UpdateProgress();
            }
        }
    }
    #endregion

    #region Events

    #region Acceleration
    void OnAccelerate(InputAction.CallbackContext context)
    {
        onStartedMovement?.Invoke();

        m_attemptingAccelerate = true;
        m_isAccelerating = true;

        m_accelerationAmount = context.ReadValue<float>();

        if (m_attemptingBoost && !m_isBoosting) StartBoost();
    }

    void OnDecelerate(InputAction.CallbackContext context)
    {
        m_attemptingAccelerate = false;
        if (!m_isDrifting) m_isAccelerating = false;

        m_accelerationAmount = 0;

        if (m_isBoosting && !m_isDrifting) EndBoost();
    }
    #endregion

    #region Reverse
    void OnReversePerformed(InputAction.CallbackContext context)
    {
        float deadZone = 0.7f;
        if (context.ReadValue<float>() > deadZone)
        {
            m_isReversing = true;
            m_isBoosting = false;
        }
        else m_isReversing = false;
    }
    void OnReverseCanceled(InputAction.CallbackContext context) => m_isReversing = false;

    #endregion

    #region Turning
    void OnSteeringPerformed(InputAction.CallbackContext context)
    {
        m_turnInput = context.ReadValue<float>();

        if (m_isAccelerating)
        {
            if (m_attemptingDrift && !m_isDrifting && isGrounded) OnTurnDrift();
        }
        if (m_isGrounded) SetTurnDrag();
    }
    void OnSteeringCanceled(InputAction.CallbackContext context)
    {
        m_turnInput = 0;

        m_wagonDrag.dragX = m_defaultWagonDrag;
        m_wagonDrag.dragY = m_defaultWagonDrag;
        m_wagonDrag.dragZ = m_defaultWagonDrag;
    }

    #endregion

    #region Boost

    void OnBoostPerformed(InputAction.CallbackContext context)
    {
        m_attemptingBoost = true;
        if (m_isAccelerating || m_isDrifting || m_isAirControl) StartBoost();
    }

    void OnBoostCanceled(InputAction.CallbackContext context)
    {
        m_attemptingBoost = false;
        if (m_isBoosting) EndBoost();
    }

    void StartBoost()
    {
        onBoost?.Invoke();

        if (m_cam) m_cam.TweenFOV(_Boost.m_camFOV, _Boost.m_tweenSpeedFOV);

        m_isBoosting = true;

        if (m_turnInput != 0) SetTurnDrag();
    }

    void EndBoost()
    {
        onBoostCanceled?.Invoke();

        if (m_cam) m_cam.TweenFOV(m_cam.originalFOV, _Boost.m_tweenSpeedFOV);

        m_isBoosting = false;
        if (m_turnInput != 0) SetTurnDrag();
    }

    #endregion

    #region Grounded

    void OnBeginGrounded()
    {
        onGrounded?.Invoke();

        if (m_isSmackStunned)
        {
            m_isSmackStunned = false;
            m_currentSpeed /= 2;
        }

        if (m_isAirControl) CancelAirControl();

        if (m_turnInput != 0) SetTurnDrag();

        if (m_attemptingDrift)
        {
            m_turnInput = m_driftTurnInput;
            OnTurnDrift();
        }

        //if (m_isDrifting) m_cam.m_useOffsetOverride = false; // Unlocks camera to follow player after air control
    }

    void OnExitGrounded()
    {
        onExitGrounded?.Invoke();

        if (m_turnInput != 0)
        {
            m_wagonDrag.dragX = m_defaultWagonDrag;
            m_wagonDrag.dragY = m_defaultWagonDrag;
            m_wagonDrag.dragZ = m_defaultWagonDrag;
        }

        m_isDrifting = false;
        //if (m_isDrifting) m_cam.SetOffsetWorldSpace(); // Locks camera rotation to world space while air control
    }

    #endregion

    #region Drift
    void OnDriftPerformed(InputAction.CallbackContext context)
    {
        //FindObjectOfType<EnemySpawner>().UpdateEnemies(); // TEMP

        if (m_isGrounded)
        {
            if (!m_isHurricane)
            {
                if (m_cam)
                {
                    if (m_turnInput < 0) m_cam.camOffset = RotatePointAroundPivot(m_cam.m_originalCameraOffset, Vector3.zero, Vector3.up * 45);
                    else if (m_turnInput > 0) m_cam.camOffset = RotatePointAroundPivot(m_cam.m_originalCameraOffset, Vector3.zero, -Vector3.up * 45);

                    m_cam.TweenToOriginalCamPosition(1);
                }
                
                m_attemptingDrift = true;

                if (!m_isDrifting)
                {
                    if (m_turnInput != 0)
                    {
                        OnTurnDrift();
                    }
                }
            }
        }
        else
        {
            //m_cam.SetOffsetWorldSpace(); // Locks camera for air control
            //m_isDrifting = true;
        }
    }
    void OnDriftCanceled(InputAction.CallbackContext context)
    {
        onDriftCanceled?.Invoke();

        m_isDrifting = false;
        m_attemptingDrift = false;

        if (!m_attemptingAccelerate) m_isAccelerating = false;

        if (m_isBoosting && !m_isAccelerating) EndBoost();
        //if (!m_isGrounded) m_cam.m_useOffsetOverride = false; // Unlocks camera if air control is canceled in air
    }

    void OnTurnDrift()
    {
        onDrift?.Invoke();

        m_isAccelerating = true;

        m_isDrifting = true;
        m_currentDriftTurnSpeed = _Drifting.m_minTurnSpeed;

        m_driftCooldownTimer = 0;

        SetDriftTurnInput();

        Vector3 rbRot = rb.transform.eulerAngles;
        Vector3 rotateAmount = new Vector3(0, 45, 0);
        if (m_turnInput > 0) rb.rotation = Quaternion.Euler(rbRot.x + rotateAmount.x, rbRot.y + rotateAmount.y, rbRot.z + rotateAmount.z);
        else if (m_turnInput < 0) rb.rotation = Quaternion.Euler(rbRot.x - rotateAmount.x, rbRot.y - rotateAmount.y, rbRot.z - rotateAmount.z);
    }

    void SetDriftTurnInput()
    {
        if (m_turnInput < 0) m_driftTurnInput = -1;
        else m_driftTurnInput = 1;
    }

    #endregion

    #region Hurricane

    void OnHurricanePerformed(InputAction.CallbackContext context)
    {
        if (m_staminaBar)
        {
            if (m_staminaBar.progress > 0) OnHurricanePerformed();
        }
        else OnHurricanePerformed();
        
    }
    void OnHurricanePerformed()
    {
        onHurricane?.Invoke();

        m_isHurricane = true;
        if (m_cam)
        {
            m_cam.camSpeed = 2;

            m_cam.SetOffsetWorldSpace();
        }
    }

    void OnHurricaneCanceled(InputAction.CallbackContext context) => CancelHurricane();
    public void CancelHurricane()
    {
        if (m_isHurricane) m_endingHurricane = true;
    }

    public void EndHurricane()
    {
        float leeway = 15f;
        if (rb.transform.eulerAngles.y > prevRotY - leeway && rb.transform.eulerAngles.y < prevRotY + leeway)
        {
            onHurricaneCanceled?.Invoke();

            m_isHurricane = false;
            m_endingHurricane = false;

            // Bug - makes player jump sometimes, maybe make player temporarily kinematic or kill velocity?
            rb.transform.eulerAngles = new Vector3(rb.transform.eulerAngles.x, prevRotY, rb.transform.eulerAngles.z);

            if (m_cam)
            {
                m_cam.camSpeed = m_cam.m_originalCamSpeed;

                m_cam.m_useOffsetOverride = false;
            }
        }
    }

    #endregion

    #region Camera
    public void OnCameraSetTarget(CameraFollow _camFollow) => m_cam = _camFollow;
    #endregion

    #region DirectionMove
    void DirectionMovePerformed(InputAction.CallbackContext context) => m_directionMoveInput = context.ReadValue<Vector2>();
    void DirectionMoveCanceled(InputAction.CallbackContext context) => m_directionMoveInput = Vector2.zero;
    #endregion

    #region AirControl

    void OnAirControl(InputAction.CallbackContext context)
    {
        if (!m_isGrounded)
        {
            m_isAirControl = true; 
            m_cam.SetOffsetWorldSpace(); // Locks camera
        }
    }

    void OnAirControlCanceled(InputAction.CallbackContext context)
    {
        if (m_isAirControl) CancelAirControl();
    }

    void CancelAirControl()
    {
        m_isAirControl = false;
        m_cam.m_useOffsetOverride = false; // Unlocks camera if air control is canceled
    }

    #endregion

    #endregion

    #region GroundCheck
    private void GroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(_Grounded.m_raycastPoint.position, Vector3.down, out hit, _Grounded.m_groundDist, _Grounded.m_groundLayer)) SetGrounded(hit);
        else
        {
            if (Physics.Raycast(_Grounded.m_secondaryPoint.position, Vector3.down, out hit, _Grounded.m_groundDist, _Grounded.m_groundLayer)) SetGrounded(hit);
            else
            {
                if (Physics.Raycast(_Grounded.m_thirdPoint.position, Vector3.down, out hit, _Grounded.m_groundDist, _Grounded.m_groundLayer)) SetGrounded(hit);
                else
                {
                    if (m_isGrounded) OnExitGrounded();
                    m_isGrounded = false;
                }
            }
        }
    }

    void SetGrounded(RaycastHit _hit)
    {
        if (!m_isGrounded) OnBeginGrounded();
        m_isGrounded = true;
        rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, Quaternion.FromToRotation(rb.transform.up, _hit.normal) * rb.transform.rotation, Time.fixedDeltaTime * 10.0f);
    }
    #endregion

    #region Move
    void MoveVelocity()
    {
        if (!m_canMove) return;
        if (m_isSmackStunned) return;

        #region CalculateSpeed

        float statMulti = 1;

        if (m_staminaBar && m_isBoosting)
        {
            // If stamina runs out, cancel boost
            if (m_staminaBar.progress <= 0)
            {
                onBoostCanceled?.Invoke();

                if (m_isBoosting) EndBoost();
                m_attemptingBoost = false;
                m_staminaBar.progress = 0;
                m_staminaBar.UpdateProgress();
            }

            // Reset stamina cooldown timer while boosting
            if (m_isAccelerating) m_staminaRegenTimer = 0;
        }
        
        if (m_isAccelerating)
        {
            if (!m_isReversing)
            {
                if (!m_isBoosting)
                {
                    // Accelerate if player is accelerating and isn't at max speed
                    if (m_currentSpeed < _Speed.m_maxSpeed)
                    {
                        m_currentSpeed += Time.fixedDeltaTime * _Speed.m_accelerationRate * m_accelerationAmount;
                        if (m_currentSpeed > _Speed.m_maxSpeed) m_currentSpeed = _Speed.m_maxSpeed; // Caps speed at max
                    }
                    else if (m_currentSpeed > _Speed.m_maxSpeed)
                    {
                        m_currentSpeed -= Time.fixedDeltaTime * _Boost.m_decelerationRate;
                        if (m_currentSpeed < _Speed.m_maxSpeed) m_currentSpeed = _Speed.m_maxSpeed;
                    }
                }
                else
                {
                    // Accelerate if player is boosting and isn't at max boost speed
                    if (m_currentSpeed <= _Boost.m_maxSpeed)
                    {
                        if (m_currentSpeed < _Boost.m_maxSpeed)
                        {
                            m_currentSpeed += Time.fixedDeltaTime * _Boost.m_accelerationRate * m_accelerationAmount;
                            if (m_currentSpeed > _Boost.m_maxSpeed) m_currentSpeed = _Boost.m_maxSpeed; // Caps speed at max
                        }
                        
                        if (m_staminaBar)
                        {
                            float staminaCostPerSec = _Boost.m_staminaCostPerSec - (m_playerUpgrades.staminaPoints * (_Boost.m_staminaCostPerSec * (_Stamina.m_decreasePercentPerStatPoint / 100)));

                            m_staminaBar.progress -= Time.fixedDeltaTime * staminaCostPerSec;
                            m_staminaBar.UpdateProgress();
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
                if (m_currentSpeed < 0)
                {
                    onStoppedMovement?.Invoke();
                    m_currentSpeed = 0;
                }
            }
            else if (m_currentSpeed < 0) // Increase speed (player was reversing)
            {
                m_currentSpeed += Time.fixedDeltaTime * _Speed.m_reverseDecelerationRate;
                if (m_currentSpeed > 0) m_currentSpeed = 0;
            }
        }

        // Alters speed based on stat upgrades
        if (m_playerUpgrades.speedPoints > 0)
        {
            statMulti = (m_playerUpgrades.speedPoints * _Speed.m_multiPerStatPoint) + 1;
        }
        #endregion

        float newSpeed = m_currentSpeed * statMulti;
        if (!m_isGrounded) newSpeed *= _Speed.m_inAirMultiplier;

        m_animator.SetFloat("Speed", newSpeed / _Speed.m_maxSpeed);

        if (!m_isHurricane && !m_endingHurricane)
        {
            prevRotY = rb.transform.eulerAngles.y;

            Vector3 dir = rb.transform.forward;
            if (m_isDrifting)
            {
                if (m_isGrounded)
                {
                    if (m_driftTurnInput != 0)
                    {
                        if (m_driftTurnInput > 0) dir -= rb.transform.right;
                        else if (m_driftTurnInput < 0) dir += rb.transform.right;

                        dir *= _Drifting.m_moveMultiplier;
                    }
                }
                else // Air control
                {
                    //dir = m_cam.transform.forward;
                }
            }
            else if (m_isAirControl)
            {
                if (m_cam) dir = m_cam.transform.forward;
            }

            // Apply velocity based on calculated speed, Without affecting y velocity
            if (newSpeed != 0)
            {
                float velY = rb.velocity.y;
                if (!m_isGrounded && velY > _Speed.m_maxVelY) velY = _Speed.m_maxVelY;
                
                if (!rb.isKinematic)
                {
                    rb.velocity = dir * newSpeed;
                    rb.velocity = new Vector3(rb.velocity.x, velY, rb.velocity.z);
                }
            }
        }
        else // If hurricane
        {
            if (m_staminaBar)
            {
                float staminaCostPerSec = _Hurricane.m_staminaCostPerSec - (m_playerUpgrades.staminaPoints * (_Hurricane.m_staminaCostPerSec * (_Stamina.m_decreasePercentPerStatPoint / 100)));

                float staminaCostThisFrame = staminaCostPerSec * Time.fixedDeltaTime;

                if (m_staminaBar.progress >= staminaCostThisFrame || m_endingHurricane)
                {
                    m_staminaBar.progress -= staminaCostThisFrame;
                    m_staminaBar.UpdateProgress();

                    m_staminaRegenTimer = 0;

                    rb.transform.rotation = Quaternion.Euler(rb.transform.rotation.eulerAngles + new Vector3(0f, _Hurricane.m_spinSpeed * Time.fixedDeltaTime, 0f));

                    // Handle movement
                    if (m_cam)
                    {
                        Vector3 dir = new Vector3(m_directionMoveInput.x, 0, m_directionMoveInput.y); // Gets local movement direction vector
                        dir = m_cam.transform.TransformDirection(dir); // Converts movement direction to world space

                        // Converts movement direction to velocity and applies it to rigidbody
                        float velY = rb.velocity.y;
                        rb.velocity = dir * _Hurricane.m_moveSpeed;
                        rb.velocity = new Vector3(rb.velocity.x, velY, rb.velocity.z);

                        if (rb.velocity.y > 0) rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

                        m_cam.transform.position += rb.velocity * Time.fixedDeltaTime;
                    }
                    
                    if (m_endingHurricane) EndHurricane();
                }
                else if (m_isHurricane) m_endingHurricane = true;
            }
        }

    }
    #endregion

    #region Turn
    void Turn()
    {
        if (m_isHurricane) return;

        float turnInput = m_turnInput;
        float turnSpeed = _Turning.m_defaultSpeed;
        if (m_isDrifting)
        {
            if (m_isGrounded)
            {
                turnInput = m_driftTurnInput;

                if (m_driftCooldownTimer >= _Drifting.m_startCooldown)
                {
                    if (m_currentDriftTurnSpeed < _Drifting.m_maxTurnSpeed)
                    {
                        m_currentDriftTurnSpeed += Time.fixedDeltaTime * _Drifting.m_turnAcceleration;
                        if (m_currentDriftTurnSpeed > _Drifting.m_maxTurnSpeed) m_currentDriftTurnSpeed = _Drifting.m_maxTurnSpeed;
                    }
                }

                float turnStrength = m_turnInput + 1;
                if (turnInput < 0) turnStrength = Mathf.Abs(m_turnInput - 1);

                turnSpeed = m_currentDriftTurnSpeed * turnStrength * _Drifting.m_strengthMultiplier;
            }
        }
        else if (m_isAirControl)
        {
            turnInput = 0;
            
            // Calculates rotation amount this physics update based on input and rotation speed
            float rotZ = m_directionMoveInput.x * _AirControl.m_rollSpeed * Time.fixedDeltaTime;
            float rotX = m_directionMoveInput.y * _AirControl.m_flipSpeed * Time.fixedDeltaTime;
            
            if (m_isBoosting)
            {
                rotZ *= _AirControl.m_boostMultiplier;
                rotX *= _AirControl.m_boostMultiplier;
            }

            // Applies desired rotation
            Vector3 rotateVector = new(rotX, 0, rotZ);
            rb.transform.Rotate(rotateVector);// .rotation = Quaternion.Euler(rb.transform.rotation.eulerAngles + rotateVector);
        }
        else if (!m_isGrounded && !m_isBoosting) turnSpeed = _Turning.m_inAirSpeed;
        else if (!m_isAccelerating && !m_isReversing) turnSpeed = _Turning.m_onSpotSpeed;

        if (turnInput != 0) rb.transform.rotation = Quaternion.Euler(rb.transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnSpeed * Time.deltaTime, 0f));
    }

    void SetTurnDrag()
    {
        if (m_isBoosting)
        {
            m_wagonDrag.dragX = _CartControl.m_boostTurnDrag;
            m_wagonDrag.dragY = _CartControl.m_boostTurnDrag;
            m_wagonDrag.dragZ = _CartControl.m_boostTurnDrag;
        }
        else
        {
            m_wagonDrag.dragX = _CartControl.m_turningDrag;
            m_wagonDrag.dragY = _CartControl.m_turningDrag;
            m_wagonDrag.dragZ = _CartControl.m_turningDrag;
        }
    }
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
