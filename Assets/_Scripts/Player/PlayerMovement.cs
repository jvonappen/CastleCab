using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    #region CanMove
    bool m_canMove = true;
    bool m_isSmackStunned;
    public bool isSmackStunned { get { return m_isSmackStunned; } set { m_isSmackStunned = value; } }

    bool m_canRotateToGround = true;
    public void SetCanRotateToGround(bool _canRotateToGround) => m_canRotateToGround = _canRotateToGround;
    #endregion

    #region References
    PlayerInputHandler m_playerInput;
    CameraFollow m_cam;

    Animator m_animator;

    public Rigidbody rb;
    public Rigidbody wagon;
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
        public float m_maxSpeed;
        public float m_accelerationRate, m_decelerationRate;

        [SerializeField] internal float m_maxReverseSpeed, m_reverseAccelerationRate, m_reverseDecelerationRate;

        [SerializeField] internal float m_airDrag;

        [SerializeField] internal float m_maxVelY;

        [Space(5)]
        [SerializeField] internal float m_maxSpeedMultiPerStatPoint;
        [SerializeField] internal float m_accelerationMultiPerStatPoint;
    }
    public float currentSpeed { get { return rb.velocity.magnitude; } }
    bool m_attemptingAccelerate;

    bool m_isAccelerating;
    bool m_isReversing;

    float m_accelerationInput;

    float prevRotY;

    float m_defaultDrag;

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
        [SerializeField] internal float m_startMoveCooldown;
        [SerializeField] internal float m_nextDriftCooldown;
    }
    float m_currentDriftTurnSpeed = 50;
    bool m_isDrifting, m_attemptingDrift;
    float m_driftTurnInput;

    float m_driftBeginMoveCooldownTimer, m_newDriftCooldownTimer;

    public bool isDrifting { get { return m_isDrifting; } }
    [SerializeField] Drifting _Drifting;
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

    #region Slipstream
    [System.Serializable]
    public struct SlipstreamData
    {
        [SerializeField] internal float trailDetectionRange;
        [SerializeField] internal float maxSpeedMulti, accelerationMulti;
        [SerializeField] internal float maxAngle;
    }
    List<TrailData> m_trailSegmentsInRange = new();
    bool m_inSlipstream = false;

    [SerializeField] SlipstreamData _Slipstream;
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
    float m_baseMaxBoostSpeed;

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
        [SerializeField] internal Collider m_collider;
        [SerializeField] internal List<Collider> m_collidersToDisable;
        [SerializeField] internal float m_spinSpeed, m_moveSpeed, m_staminaCostPerSec;
        [SerializeField] internal float m_newHurricaneCooldown;
    }
    bool m_isHurricane;
    bool m_endingHurricane;
    Vector2 m_directionMoveInput;

    bool m_continueHurricane;

    float m_newHurricaneCooldownTimer;

    Timer m_continueHurricaneTimer;

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

        m_animator = GetComponentInChildren<Animator>();

        if (!m_staminaBar) Debug.LogWarning("Boost bar reference not found");

        m_cam = GetComponentInChildren<CameraFollow>();

        m_defaultDrag = wagon.drag;

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

        m_playerInput.m_playerControls.Controls.AirControl.performed += OnAirControlPerformed;
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
        // - Cooldowns
        // Delay before drift starts drifting
        if (m_driftBeginMoveCooldownTimer < _Drifting.m_startMoveCooldown) m_driftBeginMoveCooldownTimer += Time.deltaTime;

        // Cooldown to use ability again
        if (m_newDriftCooldownTimer < _Drifting.m_nextDriftCooldown) m_newDriftCooldownTimer += Time.deltaTime;
        if (m_newHurricaneCooldownTimer < _Hurricane.m_newHurricaneCooldown) m_newHurricaneCooldownTimer += Time.deltaTime;


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

        m_accelerationInput = context.ReadValue<float>();

        if (m_attemptingBoost && !m_isBoosting) StartBoost();
    }

    void OnDecelerate(InputAction.CallbackContext context)
    {
        m_attemptingAccelerate = false;
        if (!m_isDrifting) m_isAccelerating = false;

        m_accelerationInput = 0;

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
    }
    void OnSteeringCanceled(InputAction.CallbackContext context) => m_turnInput = 0;

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

        m_cam.TweenFOV(_Boost.m_camFOV, _Boost.m_tweenSpeedFOV);
        m_isBoosting = true;
    }

    void EndBoost()
    {
        onBoostCanceled?.Invoke();

        m_cam.TweenFOV(m_cam.originalFOV, _Boost.m_tweenSpeedFOV);
        m_isBoosting = false;
    }

    #endregion

    #region Grounded

    void OnBeginGrounded()
    {
        onGrounded?.Invoke();

        wagon.drag = m_defaultDrag;

        if (m_isSmackStunned)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            TimerManager.RunAfterTime(() => { m_isSmackStunned = false; }, 0.1f);
            
        }

        if (m_isAirControl) CancelAirControl();

        if (m_attemptingDrift)
        {
            m_turnInput = m_driftTurnInput;
            OnTurnDrift();
        }
    }

    void OnExitGrounded()
    {
        onExitGrounded?.Invoke();

        wagon.drag = 0;
        m_isDrifting = false;

        if (m_isHurricane) EndHurricane();
    }

    #endregion

    #region Drift
    void OnDriftPerformed(InputAction.CallbackContext context)
    {
        if (m_isGrounded)
        {
            if (!m_isHurricane)
            {
                m_attemptingDrift = true;

                if (!m_isDrifting)
                {
                    if (m_turnInput != 0) OnTurnDrift();
                }
            }
        }
    }
    void OnDriftCanceled(InputAction.CallbackContext context)
    {
        onDriftCanceled?.Invoke();

        m_isDrifting = false;
        m_attemptingDrift = false;

        if (!m_attemptingAccelerate) m_isAccelerating = false;

        if (m_isBoosting && !m_isAccelerating) EndBoost();
    }

    void OnTurnDrift()
    {
        if (m_newDriftCooldownTimer >= _Drifting.m_nextDriftCooldown)
        {
            m_newDriftCooldownTimer = 0;

            onDrift?.Invoke();

            // Camera
            if (!m_isDrifting)
            {
                if (m_turnInput < 0) m_cam.m_target.Rotate(Vector3.up * 45);
                else if (m_turnInput > 0) m_cam.m_target.Rotate(Vector3.up * -45);
                m_cam.TweenTargetRotation(Vector3.zero, _Drifting.m_nextDriftCooldown);
            }

            m_isAccelerating = true;

            m_isDrifting = true;
            m_currentDriftTurnSpeed = _Drifting.m_minTurnSpeed;

            m_driftBeginMoveCooldownTimer = 0;

            SetDriftTurnInput();

            Vector3 playerRot = rb.transform.eulerAngles;
            Vector3 rotateAmount = new(0, 45, 0);

            if (m_turnInput > 0) rb.transform.rotation = Quaternion.Euler(playerRot.x + rotateAmount.x, playerRot.y + rotateAmount.y, playerRot.z + rotateAmount.z);
            else if (m_turnInput < 0) rb.transform.rotation = Quaternion.Euler(playerRot.x - rotateAmount.x, playerRot.y - rotateAmount.y, playerRot.z - rotateAmount.z);
        }
    }

    void SetDriftTurnInput()
    {
        if (m_turnInput < 0) m_driftTurnInput = -1;
        else m_driftTurnInput = 1;
    }

    #endregion

    #region AirControl

    void OnAirControlPerformed(InputAction.CallbackContext context)
    {
        if (!m_isGrounded)
        {
            m_isAirControl = true;
            //m_cam.SetOffsetWorldSpace(); // Locks camera
            m_cam.SetAirControl();
        }
    }

    void OnAirControlCanceled(InputAction.CallbackContext context)
    {
        if (m_isAirControl) CancelAirControl();
    }

    void CancelAirControl()
    {
        m_isAirControl = false;
        //m_cam.m_useOffsetOverride = false; // Unlocks camera if air control is canceled
        m_cam.StopAirControl();
    }

    #endregion

    #region Hurricane

    void OnHurricanePerformed(InputAction.CallbackContext context)
    {
        if (m_isGrounded)
        {
            if (m_canMove && !m_isSmackStunned)
            {
                if (m_staminaBar)
                {
                    if (m_staminaBar.progress > 0) OnHurricanePerformed();
                }
                else OnHurricanePerformed();
            }
        }
    }
    void OnHurricanePerformed()
    {
        if (m_isHurricane)
        {
            m_continueHurricane = true;
            if (m_continueHurricaneTimer != null)
            {
                TimerManager.DestroyTimer(m_continueHurricaneTimer);
                m_continueHurricaneTimer = null;
            }
        }
        
        if (m_newHurricaneCooldownTimer >= _Hurricane.m_newHurricaneCooldown)
        {
            onHurricane?.Invoke();

            if (_Hurricane.m_collider) _Hurricane.m_collider.gameObject.SetActive(true);
            foreach (Collider collider in _Hurricane.m_collidersToDisable) collider.gameObject.layer = LayerMask.NameToLayer("DontCollideStructure");

            m_lastPosHurricane = rb.transform.position;
            m_isHurricane = true;

            m_cam.StartWhirlwind();
        }
    }

    void OnHurricaneCanceled(InputAction.CallbackContext context) => CancelHurricane();
    public void CancelHurricane() { if (m_isHurricane) m_endingHurricane = true; }

    public void TryEndHurricane()
    {
        float leeway = 15f;
        if (rb.transform.eulerAngles.y > prevRotY - leeway && rb.transform.eulerAngles.y < prevRotY + leeway)
        {
            if (m_continueHurricane)
            {
                // If player attempted hurricane again during hurricane and the buffer timer isn't set
                if (m_continueHurricaneTimer == null)
                {
                    // Once the hurricane button hasn't been been pressed for {0.2f} seconds, allow it to end. (Timer is destroyed each time button is pressed)
                    m_continueHurricaneTimer = TimerManager.RunAfterTime(() => { m_continueHurricaneTimer = null; m_continueHurricane = false; }, 0.2f);
                }
            }
            else EndHurricane();
        }
    }

    public void EndHurricane()
    {
        onHurricaneCanceled?.Invoke();

        if (_Hurricane.m_collider) _Hurricane.m_collider.gameObject.SetActive(false);
        foreach (Collider collider in _Hurricane.m_collidersToDisable) collider.gameObject.layer = LayerMask.NameToLayer("Player");

        m_isHurricane = false;
        m_endingHurricane = false;

        rb.transform.eulerAngles = new Vector3(rb.transform.eulerAngles.x, prevRotY, rb.transform.eulerAngles.z);

        m_cam.StopWhirlwind(_Hurricane.m_newHurricaneCooldown - 0.1f);
    }

    Vector3 m_lastPosHurricane = Vector3.zero;
    void UpdateHurricane()
    {
        m_newHurricaneCooldownTimer = 0;

        rb.transform.Rotate(new Vector3(0f, _Hurricane.m_spinSpeed * Time.fixedDeltaTime, 0f), Space.Self);// .rotation = Quaternion.Euler(rb.transform.rotation.eulerAngles + new Vector3(0f, _Hurricane.m_spinSpeed * Time.fixedDeltaTime, 0f));


        // Handle movement
        if (m_cam.whirlwindCam)
        {
            Vector3 dir = new Vector3(m_directionMoveInput.x, 0, m_directionMoveInput.y); // Gets local movement direction vector
            dir = m_cam.whirlwindCam.TransformDirection(dir); // Converts movement direction to world space

            // Converts movement direction to velocity and applies it to rigidbody
            float velY = rb.velocity.y;
            rb.velocity = dir * _Hurricane.m_moveSpeed;
            rb.velocity = new Vector3(rb.velocity.x, velY, rb.velocity.z);

            if (rb.velocity.y > 0) rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            if (m_lastPosHurricane != Vector3.zero)
            {
                Vector3 moveVelocity = rb.transform.position - m_lastPosHurricane;

                m_cam.whirlwindCam.position += moveVelocity;
            }
            //m_cam.whirlwindCam.position += rb.velocity * Time.fixedDeltaTime;

            m_lastPosHurricane = rb.transform.position;
        }

        if (m_endingHurricane) TryEndHurricane();
    }

    #endregion

    #region DirectionMove
    void DirectionMovePerformed(InputAction.CallbackContext context) => m_directionMoveInput = context.ReadValue<Vector2>();
    void DirectionMoveCanceled(InputAction.CallbackContext context) => m_directionMoveInput = Vector2.zero;
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

        if (m_canRotateToGround)
        {
            rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, Quaternion.FromToRotation(rb.transform.up, _hit.normal) * rb.transform.rotation, Time.fixedDeltaTime * 10.0f);
        }
            
    }
    #endregion

    #region Move


    #region Speed

    public void AddSpeed(float _speedToAdd)
    {
        // Calculates new move direction (usually transform.forward)
        Vector3 moveDir = GetMoveDirection();
        if (!m_isGrounded) moveDir.y = 0;
        
        // Sets previous speed to either the full magnitude, or the XY magnitude if the player isn't accelerating (to prevent falling moving the player on grounded)
        float previousSpeed = rb.velocity.magnitude;
        if (!m_isAccelerating) previousSpeed = GetMagnitudeXY();

        // Redirect velocity to new moveDir
        if (m_isGrounded && !m_isReversing) rb.velocity = new(moveDir.x * previousSpeed, rb.velocity.y, moveDir.z * previousSpeed);

        // Caps local y velocity - May make player float, hence the grounded check
        if (isGrounded)
        {
            Vector3 localVelocity = rb.transform.InverseTransformDirection(rb.velocity);
            if (localVelocity.y > _Speed.m_maxVelY) rb.velocity = new Vector3(rb.velocity.x, rb.transform.TransformDirection(Vector3.up * _Speed.m_maxVelY).y, rb.velocity.z);
        }

        // Add desired speed
        rb.velocity += moveDir * _speedToAdd;
    }
    public void SetCurrentSpeed(float _speed)
    {
        Vector3 moveDir = GetMoveDirection();

        float velY = rb.velocity.y;

        rb.velocity = new(moveDir.x * _speed, velY, moveDir.z * _speed);
    }

    #endregion

    
    void MoveVelocity()
    {
        if (!m_canMove) return;
        if (m_isSmackStunned) return;

        if (!m_isGrounded) return;

        // Move outside this function on awake if performance issues, this just means updating playerdata during play will need to call a function to manually update it
        PlayerUpgradeData data = new();
        try
        {
            data = GameManager.Instance.GetPlayerData(m_playerInput.playerInput.devices[0]).playerUpgradeData;
        }
        catch { return; }

        #region CalculateSpeed

        // Alters speed based on stat upgrades
        float statMaxSpeedMulti = 1;
        float statAccelerationMulti = 1;
        if (data.speed > 0)
        {
            statMaxSpeedMulti = (data.speed * _Speed.m_maxSpeedMultiPerStatPoint) + 1;
            statAccelerationMulti = (data.speed * _Speed.m_accelerationMultiPerStatPoint) + 1;
        }



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
                CalculateSlipstream();

                float maxSpeed;
                float accelerationRate;
                if (!m_isBoosting) // Regular acceleration speed
                {
                    maxSpeed = m_inSlipstream ? _Speed.m_maxSpeed * _Slipstream.maxSpeedMulti : _Speed.m_maxSpeed;
                    accelerationRate = m_inSlipstream ? _Speed.m_accelerationRate * _Slipstream.accelerationMulti : _Speed.m_accelerationRate;
                }
                else // Boost speed
                {
                    maxSpeed = m_inSlipstream ? _Boost.m_maxSpeed * _Slipstream.maxSpeedMulti : _Boost.m_maxSpeed;
                    accelerationRate = m_inSlipstream ? _Boost.m_accelerationRate * _Slipstream.accelerationMulti : _Boost.m_accelerationRate;

                    // Updates stamina from boost
                    float staminaCostPerSec = _Boost.m_staminaCostPerSec - (data.stamina * (_Boost.m_staminaCostPerSec * (_Stamina.m_decreasePercentPerStatPoint / 100)));
                    m_staminaBar.progress -= Time.fixedDeltaTime * staminaCostPerSec;
                    m_staminaBar.UpdateProgress();
                }
                maxSpeed *= statMaxSpeedMulti;
                accelerationRate *= statAccelerationMulti;

                if (currentSpeed < maxSpeed)
                {
                    float accelInput = m_isDrifting ? 1 : m_accelerationInput;
                    if (accelInput > 0.7f) accelInput = 1;
                    AddSpeed(Time.fixedDeltaTime * accelerationRate * accelInput);
                    if (currentSpeed > maxSpeed) SetCurrentSpeed(maxSpeed); // Caps speed at max
                }
                else if (!m_isBoosting)
                {
                    AddSpeed(Time.fixedDeltaTime * -_Boost.m_decelerationRate);
                    if (currentSpeed < maxSpeed) SetCurrentSpeed(maxSpeed);
                }

                //if (!m_isBoosting)
                //{
                //    
                //
                //    // Accelerate if player is accelerating and isn't at max speed
                //    if (currentSpeed < maxSpeed)
                //    {
                //        float accelInput = m_isDrifting ? 1 : m_accelerationInput;
                //        if (accelInput > 0.7f) accelInput = 1;
                //        AddSpeed(Time.fixedDeltaTime * accelerationRate * accelInput);
                //        if (currentSpeed > maxSpeed) SetCurrentSpeed(maxSpeed); // Caps speed at max
                //    }
                //    else if (currentSpeed > maxSpeed) // Decelerates rather than sets due to boost
                //    {
                //        AddSpeed(Time.fixedDeltaTime * -_Boost.m_decelerationRate);
                //        if (currentSpeed < maxSpeed) SetCurrentSpeed(maxSpeed);
                //    }
                //}
                //else
                //{
                //    
                //
                //    // Accelerate if player is boosting and isn't at max boost speed
                //    if (currentSpeed <= maxSpeed)
                //    {
                //        if (currentSpeed < maxSpeed)
                //        {
                //            AddSpeed(Time.fixedDeltaTime * accelerationRate);
                //            if (currentSpeed > maxSpeed) SetCurrentSpeed(maxSpeed); // Caps speed at max
                //        }
                //        
                //        if (m_staminaBar)
                //        {
                //            float staminaCostPerSec = _Boost.m_staminaCostPerSec - (m_playerUpgrades.staminaPoints * (_Boost.m_staminaCostPerSec * (_Stamina.m_decreasePercentPerStatPoint / 100)));
                //
                //            m_staminaBar.progress -= Time.fixedDeltaTime * staminaCostPerSec;
                //            m_staminaBar.UpdateProgress();
                //        }
                //    }
                //}
            }
        }
        else if (m_isReversing)
        {
            // Decelerates if player is reversing and isn't at max reverse speed.
            // Uses deceleration rate instead of reverse acceleration if it is still going forward to prevent sliding 
            if (Vector3.Dot(rb.transform.forward, rb.velocity.normalized) > 0.2f) AddSpeed(Time.fixedDeltaTime * -_Speed.m_decelerationRate);
            else if (currentSpeed < _Speed.m_maxReverseSpeed) // Accelerates in reverse with different rate once player starts moving backwards
            {
                AddSpeed(Time.fixedDeltaTime * -_Speed.m_reverseAccelerationRate);
                if (currentSpeed > _Speed.m_maxReverseSpeed) SetCurrentSpeed(-_Speed.m_maxReverseSpeed); // Caps speed (in negative because it is moving backwards)
            }
        }
        else
        {
            // If player rotation to ground is locked, unlock it when standing still (Used on ramp where player is prevented from nose-diving off the end)
            m_canRotateToGround = true; 

            float leeway = 0.2f;

            // If neither moving forward or backward, decelerate to 0, rate based on if player is reversing or moving forward
            if (Vector3.Dot(rb.transform.forward, rb.velocity.normalized) > 0) // Lower speed (player was moving forward)
            {
                if (currentSpeed > 0 + leeway) AddSpeed(Time.fixedDeltaTime * -_Speed.m_decelerationRate);
                if (currentSpeed < 0 + leeway)
                {
                    onStoppedMovement?.Invoke();
                    SetCurrentSpeed(0);
                }
            }
            else if (Vector3.Dot(rb.transform.forward, rb.velocity.normalized) < 0) // Increase speed (player was reversing)
            {
                if (currentSpeed > 0 + leeway) AddSpeed(Time.fixedDeltaTime * _Speed.m_reverseDecelerationRate);
                if (currentSpeed < 0 + leeway) SetCurrentSpeed(0);
            }
        }

        
        #endregion

        m_animator.SetFloat("Speed", rb.velocity.magnitude);

        if (!m_isGrounded && _Speed.m_airDrag != 0)
        {
            Vector3 vel = rb.velocity;
            if (vel.x != 0) vel.x *= _Speed.m_airDrag;
            if (vel.y > 0) vel.y *= _Speed.m_airDrag;
            if (vel.z != 0) vel.z *= _Speed.m_airDrag;

            rb.velocity = vel;
        }

        if (!m_isHurricane && !m_endingHurricane) prevRotY = rb.transform.eulerAngles.y;
        else // If hurricane
        {
            if (m_staminaBar)
            {
                float staminaCostPerSec = _Hurricane.m_staminaCostPerSec - (data.stamina * (_Hurricane.m_staminaCostPerSec * (_Stamina.m_decreasePercentPerStatPoint / 100)));

                float staminaCostThisFrame = staminaCostPerSec * Time.fixedDeltaTime;

                if (m_staminaBar.progress >= staminaCostThisFrame || m_endingHurricane)
                {
                    m_staminaBar.progress -= staminaCostThisFrame;
                    m_staminaBar.UpdateProgress();

                    m_staminaRegenTimer = 0;

                    UpdateHurricane();
                }
                else if (m_isHurricane) m_endingHurricane = true;
            }
            else UpdateHurricane();
        }
    }

    Vector3 GetMoveDirection()
    {
        Vector3 dir = rb.transform.forward;
        if (m_isDrifting)
        {
            if (m_isGrounded)
            {
                if (m_driftTurnInput != 0)
                {
                    if (m_driftTurnInput > 0) dir = Quaternion.AngleAxis(-45, Vector3.up) * rb.transform.forward;
                    else if (m_driftTurnInput < 0) dir = Quaternion.AngleAxis(45, Vector3.up) * rb.transform.forward;

                    dir *= _Drifting.m_moveMultiplier;
                }
            }
        }

        return dir;
    }

    float GetMagnitudeXY()
    {
        float result = Mathf.Sqrt((rb.velocity.x * rb.velocity.x) + (rb.velocity.z * rb.velocity.z));
        return result;
    }
    #endregion

    #region Slipstream
    void CalculateSlipstream()
    {
        // Accelerating or boosting:
        List<PlayerData> playerData = GameManager.Instance.players;

        //List<TrailData> trailSegmentsInRange = new();
        m_trailSegmentsInRange.Clear();

        for (int i = 0; i < playerData.Count; i++)
        {
            GameObject player = playerData[i].player;
            if (player == gameObject) continue;

            if (player.TryGetComponent(out Slipstream slipstream))
            {
                List<TrailData> trailList = slipstream.trailList;
                foreach (TrailData data in trailList) // trail list is a copy of the list, not a reference
                {
                    // This functionality may cause lag, if each player has (10) trail segments, this runs 30 times per player that is moving, assuming there are 4 players active
                    if (Vector3.Distance(data.position, rb.transform.position) <= _Slipstream.trailDetectionRange)
                    {
                        // Uses dot product to calculate if the player that owns the trail is behind you. This is used ensure a slipstream isn't accessed from a player that isn't in front of you
                        Vector3 trailDisplacement = slipstream.horse.position - rb.transform.position;
                        float dot = Vector3.Dot(trailDisplacement, rb.transform.forward);
                        if (dot >= 0)
                        {
                            m_trailSegmentsInRange.Add(data);
                        }
                        else
                        {
                            //Debug.Log(player.name + " is behind: " + gameObject.name);
                        }
                    }
                }
            }
        }

        // Loops through slipstream trail segments in range until it finds one that is facing a similar direction. Otherwise does not enter slipstream.
        m_inSlipstream = false;
        for (int i = 0; i < m_trailSegmentsInRange.Count; i++)
        {
            float slipstreamAngle = Vector3.Angle(rb.transform.forward, m_trailSegmentsInRange[i].direction);
            //Debug.Log("Slipstream angle = " + slipstreamAngle);
            if (slipstreamAngle <= _Slipstream.maxAngle)
            {
                //Debug.Log("In range of angle, breaking loop. Setting inSlipstream to true");
                m_inSlipstream = true;
                break;
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

                if (m_driftBeginMoveCooldownTimer >= _Drifting.m_startMoveCooldown)
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
