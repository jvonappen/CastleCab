using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceWallCollisions : MonoBehaviour
{
    [SerializeField] ParticleSystem _fenceImpact;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Fence")
        {
            //ParticleSystem impact = Instantiate(_fenceImpact, collision.transform);
            //_fenceImpact.Play();
            Debug.Log("Hit Fence");
            Destroy(collision.gameObject, 3);
        }
    }
}
