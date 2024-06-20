using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DeactivateParticle : MonoBehaviour
{
    ParticleSystem m_particleSystem;
    private void OnEnable() { m_particleSystem = GetComponent<ParticleSystem>(); }
    private void Update() { if (!m_particleSystem.isPlaying) gameObject.SetActive(false); }
}
