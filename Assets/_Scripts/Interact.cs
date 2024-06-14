using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    [SerializeField] PlayerInputHandler m_input;
    [SerializeField] Camera m_cam;
    public Camera cam { get { return m_cam; } }

    List<Interactable> m_interactablesInRange = new();

    Interactable m_previousClosestInteractable;

    #region Unity Callbacks

    private void OnEnable() => m_input.m_playerControls.Controls.Interact.performed += InteractPerformed;
    private void OnDisable() => m_input.m_playerControls.Controls.Interact.performed -= InteractPerformed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Interactable interactable))
        {
            if (!m_interactablesInRange.Contains(interactable))
            {
                m_interactablesInRange.Add(interactable);
                UpdateClosestInteractable();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Interactable interactable))
        {
            if (m_interactablesInRange.Contains(interactable))
            {
                m_interactablesInRange.Remove(interactable);
                UpdateClosestInteractable();
            }
        }
    }

    #endregion

    void UpdateClosestInteractable()
    {
        Interactable interactable = GetClosestInteractable();
        if (interactable != m_previousClosestInteractable)
        {
            if (m_previousClosestInteractable) m_previousClosestInteractable.OnExitInteractable(this);
            if (interactable) interactable.OnEnterInteractable(this);

            m_previousClosestInteractable = interactable;
        }
    }

    void InteractPerformed(InputAction.CallbackContext _context)
    {
        Interactable interactable = GetClosestInteractable();
        if (interactable) interactable.Interact(this);
    }

    Interactable GetClosestInteractable()
    {
        if (m_interactablesInRange.Count == 0) return null;
        if (m_interactablesInRange.Count == 1) return m_interactablesInRange[0];

        // Gets the closest interactable
        Interactable selectedInteractable = null;
        float closestDist = float.MaxValue;
        for (int i = 0; i < m_interactablesInRange.Count; i++)
        {
            float currentDist = Vector3.Distance(transform.position, m_interactablesInRange[i].transform.position);
            if (currentDist < closestDist)
            {
                closestDist = currentDist;
                selectedInteractable = m_interactablesInRange[i];
            }
        }
        return selectedInteractable;
    }
}
