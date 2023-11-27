using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Water : MonoBehaviour
{
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] ParticleSystem[] _bubbles;
    public bool _underWater { get; private set; }

    private void Awake()
    {
        //_soundManager = FindObjectOfType<SoundManager>();
        _underWater = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 4)
        {
            _underWater = true;
            PlayParticles(_bubbles);
            _soundManager.Play("Water");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 4)
        {
            _underWater = false;
            StopParticles(_bubbles);
            _soundManager.Stop("Water");
            //_soundManager.Fade("Water");
        }
    }

    private void PlayParticles(ParticleSystem[] particles)
    {
        if (particles != null)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                if (!particles[i].isEmitting)
                {
                    Debug.Log("Play particles");
                    particles[i].Play();
                }
            }
        }
    }

    private void StopParticles(ParticleSystem[] particles)
    {
        if (particles != null)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].isEmitting)
                {
                    particles[i].Stop();
                }
            }
        }
    }
}
