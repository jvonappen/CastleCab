using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FenceWallCollisions : MonoBehaviour
{
    [SerializeField] ParticleSystem _fenceImpact;
    [SerializeField] ParticleSystem _chickenImpact;
    [SerializeField] ParticleSystem _pigImpact;
    [SerializeField] ParticleSystem _sheepImpact;
    [SerializeField] ParticleSystem _horseImpact;
    [SerializeField] ParticleSystem _npcImpact;

    [SerializeField] ParticleSystem _explosiveImpact;

    private static Transform _particlePos;
    ExplosionForce explosionForce;

    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.gameObject.tag == "Fence")
        {
            AchievementManager.fenceTracker = AchievementManager.fenceTracker + 1;
            AchievementManager.Instance.PloughHorse();

            AudioManager.Instance.StopSFX();
            AudioManager.Instance.PlayGroupAudio("FenceCollisions");
            _particlePos = collision.transform;
            PlayParticle(_fenceImpact);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Sheep")
        {
            AudioManager.Instance.StopSFX();
            AudioManager.Instance.PlaySFX("Sheep");
            _particlePos = collision.transform;
            PlayParticle(_sheepImpact);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Chicken")
        {
            AudioManager.Instance.StopSFX();
            AudioManager.Instance.PlaySFX("Chicken");
            AchievementManager.eggCheck = true;
            AchievementManager.Instance.BaconEggs();
            _particlePos = collision.transform;
            PlayParticle(_chickenImpact);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Pig")
        {
            AudioManager.Instance.StopSFX();
            AudioManager.Instance.PlayGroupAudio("Pig");
            AchievementManager.baconCheck = true;
            AchievementManager.Instance.BaconEggs();
            _particlePos = collision.transform;
            PlayParticle(_pigImpact);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Horse")
        {
            _particlePos = collision.transform;
            collision.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(1000, this.transform.position, 20, 500); //Same as pigsplode valuse

            AudioManager.Instance.StopSFX();
            AudioManager.Instance.PlayGroupAudio("Horse");
            PlayParticle(_horseImpact);
            Destroy(collision.gameObject, 5);
           
        }
        if (collision.gameObject.tag == "NPC")
        {
            AudioManager.Instance.StopSFX();
            _particlePos = collision.transform;
            collision.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(1000, this.transform.position, 20, 500); //Same as pigsplode valuse

            AudioManager.Instance.PlayGroupAudio("NPCScreams");

            PlayParticle(_npcImpact);
            Destroy(collision.gameObject, 5);
        }
        if (collision.gameObject.tag == "BOOM")
        {
            _particlePos = collision.transform;
            PlayParticle(_explosiveImpact);
            collision.gameObject.GetComponent<ExplosionForce>().Explode();
            Destroy(collision.gameObject);
        }

    }

    public void PlayParticle(ParticleSystem particle)
    {
        particle.transform.position = _particlePos.position;
        particle.Play();
    }
}
