using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationSpin : MonoBehaviour
{
    [SerializeField] PlayerInputHandler m_playerInput;
    [SerializeField] Camera m_cam;

    [SerializeField] Transform m_rotationPoint;

    [Space(10)]
    [SerializeField] float m_rotSpeedX = 10;
    [SerializeField] float m_rotSpeedY = 10;

    [SerializeField] float m_maxAngleLimitY = 60;

    private void Start()
    {
        m_cam.transform.SetParent(null);
    }

    private void Update()
    {
        Vector3 currentPos = transform.position;
        Vector3 currentRot = transform.eulerAngles;

        Vector2 rotInput = m_playerInput.m_playerControls.Controls.DirectionInput.ReadValue<Vector2>();
        
        // Vertical rotation
        transform.RotateAround(m_rotationPoint.position, transform.right, -rotInput.y * m_rotSpeedY * Time.deltaTime);
        float angle = Quaternion.Angle(new Quaternion(0, transform.rotation.y, 0, transform.rotation.w), transform.rotation);
        if (angle > m_maxAngleLimitY)
        {
            transform.position = currentPos;
            transform.eulerAngles = currentRot;
        }

        // Horizontal rotation
        transform.RotateAround(m_rotationPoint.position, Vector3.up, rotInput.x * m_rotSpeedX * Time.deltaTime);
    }

    private void OnDestroy()
    {
        Destroy(m_cam.gameObject);
    }
}
