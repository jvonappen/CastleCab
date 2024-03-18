using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (!rb) return;

        Knockback kb = rb.GetComponent<Knockback>();
        if (kb) kb.KnockBack((rb.transform.position - collision.GetContact(0).point).normalized);
    }
}
