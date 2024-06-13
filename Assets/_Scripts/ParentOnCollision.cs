using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentOnCollision : MonoBehaviour
{
    List<GameObject> m_collidingObjects = new();

    [SerializeField] bool m_useColliderParent;
    [SerializeField] LayerMask m_collisionLayers;

    private void OnTriggerEnter(Collider other) => HandleCollision(other, true);
    private void OnTriggerExit(Collider other) => HandleCollision(other, false);

    void HandleCollision(Collider _collider, bool _isEnter)
    {
        Rigidbody rb = _collider.attachedRigidbody;
        if (rb)
        {
            GameObject go;

            if (m_useColliderParent) go = rb.transform.parent.gameObject;
            else go = rb.gameObject;

            if (go)
            {
                if (m_collisionLayers == (m_collisionLayers | (1 << go.layer)))
                {
                    if (_isEnter)
                    {
                        Debug.Log("Parented");
                        go.transform.parent = transform;
                    }
                    else
                    {
                        Debug.Log("Unparented");
                        go.transform.parent = null;
                    }
                }
            }
        }
    }
}
