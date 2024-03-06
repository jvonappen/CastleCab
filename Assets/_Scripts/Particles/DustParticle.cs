public class DustParticle : ParticleHandler
{
    private void Start()
    {
        m_playerMovement.onStartedMovement += OnStartMovement;
        m_playerMovement.onStoppedMovement += StopParticle;

        m_playerMovement.onGrounded += OnGrounded;
        m_playerMovement.onExitGrounded += StopParticle;
    }

    void OnStartMovement()
    {
        if (m_playerMovement.isGrounded) PlayParticle();
    }

    void OnGrounded()
    {
        if (m_playerMovement.currentSpeed > 0) PlayParticle();
    }
}
