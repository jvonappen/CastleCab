using UnityEngine;

public class DustParticle : ParticleHandler
{
    [SerializeField] float m_minSpeedToTrigger = 5;

    private void Update()
    {
        if (m_playerMovement.currentSpeed < m_minSpeedToTrigger || m_playerMovement.isHurricane || !m_playerMovement.isGrounded) StopParticle();
        else if(!m_isEmitting && m_playerMovement.isGrounded && !m_playerMovement.isHurricane) PlayParticle();
    }
}
