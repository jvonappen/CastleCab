public class DriftParticle : ParticleHandler
{
    private void Start()
    {
        m_playerMovement.onDrift += PlayParticle;
        m_playerMovement.onDriftCanceled += EndDrift;

        m_playerMovement.onGrounded += OnGrounded;
        m_playerMovement.onExitGrounded += ExitGrounded;

        m_playerMovement.onHurricane += PlayParticle;
        m_playerMovement.onHurricaneCanceled += EndHurricane;
    }

    void OnGrounded()
    {
        if (m_playerMovement.isDrifting) PlayParticle();
    }

    void ExitGrounded() => StopParticle();

    void EndHurricane()
    {
        if (!m_playerMovement.isDrifting) StopParticle();
    }

    void EndDrift()
    {
        if (!m_playerMovement.isHurricane) StopParticle();
    }
}
