using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public Vector3 DebugVelocity;

    [Header("Transform to be followed by camera")]
    public Transform ToFollow;

    [Header("Offset in position in reference to target transform (focus point)")]
    public Vector3 FollowingOffset = new Vector3(0f, 1.5f, 0f);

    [Header("Offset in position in reference to camera orientation")]
    public Vector3 FollowingOffsetDirection = new Vector3(0f, 0f, 0f);

    // Variables to controll distance of camera to following object
    [Header("Clamp values for zoom of camera")]
    public Vector2 DistanceRanges = new Vector2(5f, 10f);

    private float targetDistance;
    private float animatedDistance;

    //Variables to controll rotation of camera around followed object
    public Vector2 RotationRanges = new Vector2(-60f, 60f);

    private Vector2 targetSphericRotation = new Vector2(0f, 0f);
    private Vector2 animatedSphericRotation = new Vector2(0f, 0f);

    [Space(10)]
    [Tooltip("Sensitivity value for rotating camera around following object")]
    public float RotationSensitivity = 10f;

    [Header("If you want camera rotation to be smooth")]
    [Range(0.1f, 1f)]
   
    public float RotationSpeed = 1f;

    [Header("If you want camera to follow target with some smoothness")]
    [Range(0f, 1f)]
  
    public float HardFollowValue = 1f;

    [Header("If you want to hold cursor (cursor switch on TAB)")]
    public bool LockCursor = false;

    //Just to make turning off lock cursor less annoying
    private bool rotateCamera = true;

    //Raycast checking if there is obstacle blocking our vision
    private RaycastHit sightObstacleHit;

    [Header("Layer mask to check obstacles in sight ray")]
    public LayerMask SightLayerMask;

    //Target position for camera from basic calculations, if raycast hit something, there is used other position
    private Vector3 targetPosition;

    [Header("How far forward raycast should check collision for camera")]
    public float CollisionOffset = 1f;

    Vector3 movVelo = Vector3.zero;

    private void Start()
    {
        targetDistance = (DistanceRanges.x + DistanceRanges.y) / 2;
        animatedDistance = DistanceRanges.y;

        targetSphericRotation = new Vector2(0f, 23f);
        animatedSphericRotation = targetSphericRotation;

    }

    private Vector3 prePos = Vector3.zero;

    private void UpdateMethods()
    {
        InputCalculations();
        ZoomCalculations();
        FollowCalculations();
        RaycastCalculations();
        SwitchCalculations();

        DebugVelocity = transform.position - prePos;
        prePos = transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            if (Cursor.lockState != CursorLockMode.Locked) HelperSwitchCursor();

        if (Input.GetKey(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
            if (Cursor.lockState == CursorLockMode.Locked) HelperSwitchCursor();

        UpdateMethods();
    }

    private void InputCalculations()
    {
        targetDistance -= (Input.GetAxis("Mouse ScrollWheel") * 5f);

        if (!rotateCamera) return;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            targetSphericRotation.x += Input.GetAxis("Mouse X") * RotationSensitivity;
            targetSphericRotation.y -= Input.GetAxis("Mouse Y") * RotationSensitivity;
        }
    }

    private void ZoomCalculations()
    {
        if (!sightObstacleHit.transform) targetDistance = Mathf.Clamp(targetDistance, DistanceRanges.x, DistanceRanges.y);
        animatedDistance = Mathf.Lerp(animatedDistance, targetDistance, Time.deltaTime * 8f);
    }

    private void FollowCalculations()
    {
        targetSphericRotation.y = HelperClampAngle(targetSphericRotation.y, RotationRanges.x, RotationRanges.y);

        if (RotationSpeed < 1f) animatedSphericRotation = new Vector2(Mathf.LerpAngle(animatedSphericRotation.x, targetSphericRotation.x, Time.deltaTime * 30 * RotationSpeed), Mathf.LerpAngle(animatedSphericRotation.y, targetSphericRotation.y, Time.deltaTime * 30 * RotationSpeed)); else animatedSphericRotation = targetSphericRotation;

        Quaternion rotation = Quaternion.Euler(animatedSphericRotation.y, animatedSphericRotation.x, 0f);
        transform.rotation = rotation;

        Vector3 targetPosition = ToFollow.transform.position + FollowingOffset;

        if (HardFollowValue < 1f)
        {
            targetPosition = Vector3.SmoothDamp(this.targetPosition, targetPosition, ref movVelo, Mathf.Lerp(.5f, 0f, HardFollowValue), Mathf.Infinity, Time.deltaTime);
        }

        this.targetPosition = targetPosition;
    }

    private void RaycastCalculations()
    {
        Vector3 followPoint = ToFollow.transform.position + FollowingOffset + transform.TransformVector(FollowingOffsetDirection);
        Quaternion cameraDir = Quaternion.Euler(targetSphericRotation.y, targetSphericRotation.x, 0f);
        Ray directionRay = new Ray(followPoint, cameraDir * -Vector3.forward);

        // If there is something in sight ray way
        if (Physics.Raycast(directionRay, out sightObstacleHit, targetDistance + CollisionOffset, SightLayerMask, QueryTriggerInteraction.Ignore))
        {
            transform.position = sightObstacleHit.point - directionRay.direction * CollisionOffset;
        }
        else
        {
            Vector3 rotationOffset = transform.rotation * -Vector3.forward * animatedDistance;
            transform.position = targetPosition + rotationOffset + transform.TransformVector(FollowingOffsetDirection);
        }
    }

    private void SwitchCalculations()
    {
        if (LockCursor)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                HelperSwitchCursor();
                if (Cursor.visible) rotateCamera = false; else rotateCamera = true;
            }
        }
    }

    #region Helpers
    // Clamping angle in 360 circle

    private float HelperClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360.0f;

        if (angle > 360)
            angle -= 360.0f;

        return Mathf.Clamp(angle, min, max);
    }
    // Switching cursor state for right work of camera rotating mechanics
    private void HelperSwitchCursor()
    {
        if (Cursor.visible)
        {
            if (Application.isFocused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    #endregion Helpers
}

