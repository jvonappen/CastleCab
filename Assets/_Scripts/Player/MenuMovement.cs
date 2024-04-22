using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputHandler))]
public class MenuMovement : MonoBehaviour
{
    PlayerInputHandler m_playerInput;

    Vector2 m_turnInput;

    [SerializeField] Rigidbody m_horse;
    [SerializeField] float m_rotationSpeed = 1;

    void Start()
    {
        m_playerInput = GetComponent<PlayerInputHandler>();

        m_playerInput.m_playerControls.Controls.DirectionInput.performed += DirectionMovePerformed;
        m_playerInput.m_playerControls.Controls.DirectionInput.canceled += DirectionMoveCanceled;
    }

    void DirectionMovePerformed(InputAction.CallbackContext context) => m_turnInput = context.ReadValue<Vector2>();
    void DirectionMoveCanceled(InputAction.CallbackContext context) => m_turnInput = Vector2.zero;

    private void Update()
    {
        Vector3 rotDir = Vector3.zero;
        if (m_turnInput.x > 0.5) rotDir = Vector3.up;
        else if (m_turnInput.x < -0.5) rotDir = -Vector3.up;

        m_horse.transform.Rotate(m_rotationSpeed * Time.deltaTime * rotDir);
    }
}
