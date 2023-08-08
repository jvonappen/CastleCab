using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public bool TouchinGround => touchingGround > 0;
    public Rigidbody Body => rb;

    private int touchingGround;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        touchingGround++;
    }

    private void OnCollisionExit(Collision collision)
    {
        touchingGround--;
    }
}
