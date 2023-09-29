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
    [SerializeField] ParticleSystem _npcImpact;

    [SerializeField] ParticleSystem _explosiveImpact;

    private static Transform _particlePos;

    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.gameObject.tag == "Fence")
        {
            _particlePos = collision.transform;
            PlayParticle(_fenceImpact);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Sheep")
        {
            _particlePos = collision.transform;
            PlayParticle(_sheepImpact);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Chicken")
        {
            _particlePos = collision.transform;
            PlayParticle(_chickenImpact);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Pig")
        {
            _particlePos = collision.transform;
            PlayParticle(_pigImpact);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "NPC")
        {
            _particlePos = collision.transform;
            collision.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(1000, this.transform.position, 20, 500); //Same as pigsplode valuse

            PlayParticle(_npcImpact);
            Destroy(collision.gameObject, 5);
        }
        if (collision.gameObject.tag == "BOOM")
        {
            _particlePos = collision.transform;
           
            PlayParticle(_explosiveImpact);
            Destroy(collision.gameObject, 3);
        }

    }

    public void PlayParticle(ParticleSystem particle)
    {
        particle.transform.position = _particlePos.position;
        particle.Play();
    }
}
