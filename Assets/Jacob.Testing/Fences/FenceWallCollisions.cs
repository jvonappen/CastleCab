using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceWallCollisions : MonoBehaviour
{
    [SerializeField] ParticleSystem _fenceImpact;

    private static Transform _particlePos;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Fence")
        {
           
            _particlePos = collision.transform;

            PlayParticle();

            Debug.Log("Hit Fence");
            Destroy(collision.gameObject);
        }
    }

    public void PlayParticle()
    {
        _fenceImpact.transform.position = _particlePos.position;
        _fenceImpact.Play();
    }
}
