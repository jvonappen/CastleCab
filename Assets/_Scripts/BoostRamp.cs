using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostRamp : MonoBehaviour
{
    [SerializeField] float m_maxSpeedMulti = 1.5f, m_accelerationMulti = 1.5f;

    List<PlayerMovement> m_players = new();

    public void OnEnter(Collider _collider)
    {
        Rigidbody rb = _collider.attachedRigidbody;
        if (rb && rb.transform.tag == "Player")
        {
            if (rb.transform.parent.TryGetComponent(out PlayerMovement playerMovement) && !m_players.Contains(playerMovement))
            {
                playerMovement.SetMaxSpeedMulti(m_maxSpeedMulti);
                playerMovement.SetAccelerationMulti(m_accelerationMulti);

                m_players.Add(playerMovement);
            }
        }
    }

    public void OnExit(Collider _collider)
    {
        Rigidbody rb = _collider.attachedRigidbody;
        if (rb && rb.transform.tag == "Player")
        {
            if (rb.transform.parent.TryGetComponent(out PlayerMovement playerMovement) && m_players.Contains(playerMovement))
            {
                playerMovement.SetMaxSpeedMulti(1);
                playerMovement.SetAccelerationMulti(1);

                m_players.Remove(playerMovement);
            }
        }
    }
}
