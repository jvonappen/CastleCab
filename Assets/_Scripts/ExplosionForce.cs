using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ExplosionForce : MonoBehaviour
{
    public float explosionRadius = 45f;
    public float explosionForce = 9000f;
    public float verticalLaunchForce = 1500f; // Adjust the vertical launch force as needed.
    public float rotationForce = 500f; // Adjust the rotation force as needed.
    PlayerMovement movement;

    // TODO <- float Break force
    public float breakForce = 30f;

    private void OnTriggerEnter(Collider other) => CheckCollision(other);
    private void OnCollisionEnter(Collision collision) => CheckCollision(collision.collider);

    void CheckCollision(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            float playerForce = other.attachedRigidbody.velocity.magnitude;

            // TODO <- if playerforce > breakforce
            if (playerForce > breakForce)
            {
                other.attachedRigidbody.velocity = Vector3.zero;

                Explode();

                Destroy(gameObject);
            }
        }
    }

    public void Explode()
    {
        // Find all colliders within the explosion radius.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {

            // Apply an explosion force to rigidbodies.
            Rigidbody rb = collider.attachedRigidbody;// .GetComponentInChildren<Rigidbody>();
            if (rb != null)
            {

                // Calculate direction from the explosion center to the rigidbody.
                Vector3 explosionDir = rb.transform.position - transform.position;

                Debug.Log("Exploded: " + rb.transform);
                if (rb.TryGetComponent(out Knockback knockback))
                {
                    knockback.KnockBack(explosionDir);
                }
                else
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);

                    //Apply vertical launch force.
                    rb.AddForce(Vector3.up * verticalLaunchForce, ForceMode.Impulse);

                    Vector3 randomRotation = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    rb.AddTorque(randomRotation * rotationForce);
                }

            }


        }
    }
}
