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
    [SerializeField] ParticleSystem _graveImpact;
    [SerializeField] ParticleSystem _ghostImpact;
    [SerializeField] ParticleSystem _vatImpact;
    [SerializeField] ParticleSystem _boxImpact;

    [SerializeField] ParticleSystem _explosiveImpact;
    [Space]
    [SerializeField] List<ParticleSystem> _stallImpactList;

    private static Transform _particlePos;

    ExplosionForce explosionForce;

    [Header("Debug")]
    [SerializeField] private float _objectRespawnDelay = 30;

    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.gameObject.tag == "Fence")
        {
            AchievementManager.fenceTracker = AchievementManager.fenceTracker + 1;
            AchievementManager.Instance.PloughHorse();

            AudioManager.Instance.PlayGroupAudio("FenceCollisions");
            _particlePos = collision.transform;
            PlayParticle(_fenceImpact);
            collision.gameObject.SetActive(false);
            StartCoroutine(ObjectRespawnDelay(collision.gameObject));
        }
        if (collision.gameObject.tag == "NotFence")
        {
            AudioManager.Instance.PlayGroupAudio("FenceCollisions");
            _particlePos = collision.transform;
            PlayParticle(_fenceImpact);
            collision.gameObject.SetActive(false);
            StartCoroutine(ObjectRespawnDelay(collision.gameObject));
        }
        if (collision.gameObject.tag == "Sheep")
        {
            //AudioManager.Instance.StopSFX();
            AudioManager.Instance.PlaySFX("Sheep");
            _particlePos = collision.transform;
            PlayParticle(_sheepImpact);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Chicken")
        {
            AchievementManager.cluckTracker = AchievementManager.cluckTracker + 1;
            AchievementManager.Instance.CluckMe();

            //AudioManager.Instance.StopSFX();
            AudioManager.Instance.PlaySFX("Chicken");
            AchievementManager.eggCheck = true;
            AchievementManager.Instance.BaconEggs();
            _particlePos = collision.transform;
            PlayParticle(_chickenImpact);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Pig")
        {
            AchievementManager.pigTracker = AchievementManager.pigTracker + 1;
            AchievementManager.Instance.MakinBacon();
            //AudioManager.Instance.StopSFX();
            AudioManager.Instance.PlayGroupAudio("Pig");
            AchievementManager.baconCheck = true;
            AchievementManager.Instance.BaconEggs();
            _particlePos = collision.transform;
            PlayParticle(_pigImpact);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Horse")
        {
            AchievementManager.glueTracker = AchievementManager.glueTracker + 1;
            AchievementManager.Instance.GlueFactory();
            _particlePos = collision.transform;
            collision.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(1000, this.transform.position, 20, 500); //Same as pigsplode valuse

            //AudioManager.Instance.StopSFX();
            AudioManager.Instance.PlayGroupAudio("Horse");
            PlayParticle(_horseImpact);
            Destroy(collision.gameObject, 5);
           
        }
        if (collision.gameObject.tag == "NPC")
        {
            AchievementManager.menaceTracker = AchievementManager.menaceTracker + 1;
            AchievementManager.Instance.Menace();
            //AudioManager.Instance.StopSFX();
            _particlePos = collision.transform;
            collision.gameObject.GetComponent<NavMeshAgent>().enabled = false;

            collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(1000, this.transform.position, 20, 500); //Same as pigsplode valuse
            AudioManager.Instance.PlayGroupAudio("NPCScreams");
            PlayParticle(_npcImpact);
            
            Destroy(collision.gameObject, 5);
        }
        if (collision.gameObject.tag == "BOOM" && PlayerData.isOccupied == false)
        {
            if (AchievementManager.unlockBaaBoom == false) { AchievementManager.Instance.BaaBoom(); }
            _particlePos = collision.transform;
            PlayParticle(_explosiveImpact);
            collision.gameObject.GetComponent<ExplosionForce>().Explode();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Grave")
        {
            AchievementManager.graveTracker = AchievementManager.graveTracker + 1;
            AchievementManager.Instance.GraveDigger();

            //AudioManager.Instance.StopSFX();
            //AudioManager.Instance.PlaySFX("");
            _particlePos = collision.transform;
            PlayParticle(_graveImpact);
            collision.gameObject.SetActive(false);
            StartCoroutine(ObjectRespawnDelay(collision.gameObject));
        }
        if (collision.gameObject.tag == "MarketStall")
        {
            AchievementManager.stallTracker = AchievementManager.stallTracker + 1;
            AchievementManager.Instance.Collidesdale();

            //AudioManager.Instance.StopSFX();
            //AudioManager.Instance.PlaySFX("");
            _particlePos = collision.transform;
            PlayParticleMarketStall();
            //Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);
            StartCoroutine(ObjectRespawnDelay(collision.gameObject));
        }
        if (collision.gameObject.tag == "Vat")
        {
            //AudioManager.Instance.StopSFX();
            //AudioManager.Instance.PlaySFX("");
            _particlePos = collision.transform;
            PlayParticle(_vatImpact);
            collision.gameObject.SetActive(false);
            StartCoroutine(ObjectRespawnDelay(collision.gameObject));
        }
        if (collision.gameObject.tag == "Box")
        {
            //AudioManager.Instance.StopSFX();
            //AudioManager.Instance.PlaySFX("");
            _particlePos = collision.transform;
            PlayParticle(_boxImpact);
            collision.gameObject.SetActive(false);
            StartCoroutine(ObjectRespawnDelay(collision.gameObject));
        }
    }

    public void PlayParticle(ParticleSystem particle)
    {
        particle.transform.position = _particlePos.position;
        particle.Play();
    }

    public void PlayParticleMarketStall()
    {
        int randomVal = UnityEngine.Random.Range(0, _stallImpactList.Count);

        ParticleSystem particle = _stallImpactList[randomVal];

        PlayParticle(particle);     
    }

    IEnumerator ObjectRespawnDelay(GameObject obj)
    {
        yield return new WaitForSeconds(_objectRespawnDelay);
        obj.SetActive(true);
    }
}
