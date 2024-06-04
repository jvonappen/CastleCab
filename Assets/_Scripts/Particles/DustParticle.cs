using UnityEngine;

public class DustParticle : ParticleHandler
{
    [SerializeField] float m_minSpeedToTrigger = 5;

    private void Start()
    {
        //m_playerMovement.onStartedMovement += OnStartMovement;
        //m_playerMovement.onStoppedMovement += StopParticle;

        //m_playerMovement.onGrounded += OnGrounded;
        //m_playerMovement.onExitGrounded += StopParticle;
    }

    private void Update()
    {
        if (m_playerMovement.currentSpeed < m_minSpeedToTrigger || m_playerMovement.isHurricane) StopParticle();
        else if(!m_isEmitting && m_playerMovement.isGrounded && !m_playerMovement.isHurricane) PlayParticle();
    }

    //void OnStartMovement()
    //{
    //    if (m_playerMovement.isGrounded) PlayParticle();
    //}
    //
    //void OnGrounded()
    //{
    //    if (m_playerMovement.currentSpeed > 0) PlayParticle();
    //}
}
