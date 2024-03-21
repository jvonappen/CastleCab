using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Explosive : Health
{
    [Header("Explosive")]
    public LayerMask m_explodeLayer;
    public float explosionRadius = 30f;
    public float explosionForce = 200f;
    public float verticalLaunchForce = 100f; // Adjust the vertical launch force as needed.
    public float rotationForce = 500f; // Adjust the rotation force as needed.
    //PlayerMovement movement;

    // TODO <- float Break force
    //public float breakForce = 30f;

    //private void OnTriggerEnter(Collider other) => CheckCollision(other);
    //private void OnCollisionEnter(Collision collision) => CheckCollision(collision.collider);
    //
    //void CheckCollision(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
    //    {
    //        float playerForce = other.attachedRigidbody.velocity.magnitude;
    //
    //        // TODO <- if playerforce > breakforce
    //        if (playerForce > breakForce)
    //        {
    //            other.attachedRigidbody.velocity = Vector3.zero;
    //
    //            Explode();
    //
    //            Destroy(gameObject);
    //        }
    //    }
    //}

    protected override void Die()
    {
        Explode();

        base.Die();
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

                if (rb.TryGetComponent(out Knockback knockback))
                {
                    if (knockback is PlayerKnockback)
                    {
                        ((PlayerKnockback)knockback).playerMovement.EndHurricane();
                    }
                    knockback.KnockBack(explosionDir, Vector3.up);
                }
                else
                {
                    if (m_explodeLayer == (m_explodeLayer | (1 << rb.gameObject.layer)))
                    {
                        rb.AddExplosionForce(explosionForce * 1000, transform.position, explosionRadius);

                        //Apply vertical launch force.
                        rb.AddForce(Vector3.up * verticalLaunchForce, ForceMode.Impulse);

                        Vector3 randomRotation = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                        rb.AddTorque(randomRotation * rotationForce);
                    }
                }

            }


        }
    }
}
