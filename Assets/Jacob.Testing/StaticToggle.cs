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
    }
}
