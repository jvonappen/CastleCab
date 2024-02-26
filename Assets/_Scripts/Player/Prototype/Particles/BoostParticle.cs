public class BoostParticle : ParticleHandler
{
    private void Start()
    {
        m_playerMovement.onBoost += PlayParticle;
        m_playerMovement.onBoostCanceled += StopParticle;
    }
}
