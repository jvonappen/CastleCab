using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollision : MonoBehaviour
{
    public UnityEvent<Collider> onTriggerEnter;
    public UnityEvent<Collision> onCollisionEnter;
    public UnityEvent<Collider> onTriggerExit;
    public UnityEvent<Collision> onCollisionExit;

    private void OnTriggerEnter(Collider other) => onTriggerEnter?.Invoke(other);
    private void OnCollisionEnter(Collision collision) => onCollisionEnter?.Invoke(collision);
    private void OnTriggerExit(Collider other) => onTriggerExit?.Invoke(other);
    private void OnCollisionExit(Collision collision) => onCollisionExit?.Invoke(collision);
}
