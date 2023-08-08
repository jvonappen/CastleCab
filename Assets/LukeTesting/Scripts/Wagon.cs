using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    private Rigidbody _wagonRB;
    [SerializeField] private float _multiplier;
    [SerializeField] private Transform[] _anchors = new Transform[4];
    private RaycastHit[] _hits = new RaycastHit[4]; 

    private void Start()
    {
        _wagonRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            ApplyForce(_anchors[i], _hits[i]);
        }
    }

    private void ApplyForce(Transform anchor, RaycastHit hit)
    {
        if (Physics.Raycast(anchor.position, -anchor.up, out hit))
        {
            float force = 0;
            force = Mathf.Abs(1 / (hit.point.y - anchor.position.y));
            _wagonRB.AddForceAtPosition(transform.up * force * _multiplier, anchor.position, ForceMode.Acceleration);
        }
    }
}
