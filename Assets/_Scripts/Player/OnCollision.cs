using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollision : MonoBehaviour
{
    public UnityEvent<Collider> onTriggerEnter;
    public UnityEvent<Collision> onCollisionEnter;

    private void OnTriggerEnter(Collider other) => onTriggerEnter?.Invoke(other);
    private void OnCollisionEnter(Collision collision) => onCollisionEnter?.Invoke(collision);
}
