public class DriftParticle : ParticleHandler
{
    private void Start()
    {
        m_playerMovement.onDrift += PlayParticle;
        m_playerMovement.onDriftCanceled += StopParticle;

        m_playerMovement.onGrounded += OnGrounded;
        m_playerMovement.onExitGrounded += ExitGrounded;
    }

    void OnGrounded()
    {
        if (m_playerMovement.isDrifting) PlayParticle();
    }

    void ExitGrounded() => StopParticle();
}
