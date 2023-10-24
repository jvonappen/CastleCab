using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExplosionForce : MonoBehaviour
{
    public float explosionRadius = 45f;
    public float explosionForce = 9000f;
    public float verticalLaunchForce = 1500f; // Adjust the vertical launch force as needed.
    public float rotationForce = 500f; // Adjust the rotation force as needed.
    // Start is called before the first frame update

    public void Explode()
    {
        // Find all colliders within the explosion radius.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            NavMeshAgent agent = collider.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
                Destroy(collider.gameObject, 5);
            }
            // Apply an explosion force to rigidbodies.
            Rigidbody rb = collider.GetComponentInChildren<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                // Apply vertical launch force.
                rb.AddForce(Vector3.up * verticalLaunchForce, ForceMode.Impulse);

                Vector3 randomRotation = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                rb.AddTorque(randomRotation * rotationForce);
            }


        }
    }
}
