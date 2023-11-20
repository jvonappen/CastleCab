using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticToggle : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = false;
        this.gameObject.GetComponent<Rigidbody>().AddExplosionForce(1000, this.transform.position, 20, 500);
    }
}
