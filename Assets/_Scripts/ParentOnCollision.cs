using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentOnCollision : MonoBehaviour
{
    [SerializeField] bool m_useColliderParent, m_ignoreTrigger = true;
    [SerializeField] LayerMask m_collisionLayers;

    private void OnTriggerEnter(Collider other) => HandleCollision(other, true);
    private void OnTriggerExit(Collider other) => HandleCollision(other, false);

    void HandleCollision(Collider _collider, bool _isEnter)
    {
        if (!m_ignoreTrigger || !_collider.isTrigger)
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
                            go.transform.parent = transform;
                        }
                        else
                        {
                            if (go.transform.parent == transform) go.transform.parent = null;
                        }
                    }
                }
            }
        }
    }
}
