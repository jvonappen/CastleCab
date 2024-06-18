using UnityEngine;

public class DustParticle : ParticleHandler
{
    [SerializeField] LayerMask m_requiredLayers;
    [SerializeField] float m_minSpeedToTrigger = 5;

    private void Update()
    {
        if (!IsValidLayer() || m_playerMovement.currentSpeed < m_minSpeedToTrigger || m_playerMovement.isHurricane || !m_playerMovement.isGrounded) StopParticle();
        else if(IsValidLayer() && !m_isEmitting && m_playerMovement.isGrounded && !m_playerMovement.isHurricane) PlayParticle();
    }

    bool IsValidLayer() => m_requiredLayers == (m_requiredLayers | (1 << m_playerMovement.groundLayer));
}
