using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleHandler : MonoBehaviour
{
    [SerializeField] protected PlayerMovement m_playerMovement;
    protected ParticleSystem m_particleSystem;

    private void Awake()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
        StopParticle();
    }

    protected void PlayParticle() => m_particleSystem.Play();
    protected void StopParticle() => m_particleSystem.Stop();
}
