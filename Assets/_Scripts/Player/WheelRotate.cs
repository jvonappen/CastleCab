using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotate : MonoBehaviour
{
    [SerializeField] PlayerMovement m_playerMovement;
    [SerializeField] float m_speedMultiplier;
    private void Update() => transform.Rotate(m_playerMovement.currentSpeed * m_speedMultiplier * 360 * Time.deltaTime, 0, 0);
}
