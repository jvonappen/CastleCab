using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRampRot : MonoBehaviour
{
    List<Collider> m_colliders = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb && rb.transform.parent)
        {
            if (rb.transform.parent.TryGetComponent(out PlayerMovement movement))
            {
                m_colliders.Add(other);
                movement.SetCanRotateToGround(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb && rb.transform.parent)
        {
            if (rb.transform.parent.TryGetComponent(out PlayerMovement movement))
            {
                m_colliders.Remove(other);
                if (m_colliders.Count == 0) movement.SetCanRotateToGround(true);
            }
        }
    }
}
