using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPads : MonoBehaviour
{
    [SerializeField] private Rigidbody _donkeyRB;
    [SerializeField] float upWardsForce = 70f;
    [SerializeField] float forwardForce = 10f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LaunchForward();
        }
    }
    private void LaunchForward()
    {
        // Apply the jump force to lift the player
        _donkeyRB.AddForce(Vector3.up * upWardsForce, ForceMode.Impulse);

        // Apply forward force to propel the player forward
        _donkeyRB.AddForce(transform.forward * forwardForce, ForceMode.Impulse);
        Debug.Log("dectected");
    }
}
