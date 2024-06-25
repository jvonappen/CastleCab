using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLayerSet : MonoBehaviour
{
    [SerializeField] PlayerInput m_input;

    private void Awake()
    {
        int layer = 0;

        int playerIndex = m_input.user.index;
        if (playerIndex == 1) layer = LayerMask.NameToLayer("P1");
        if (playerIndex == 2) layer = LayerMask.NameToLayer("P2");
        if (playerIndex == 3) layer = LayerMask.NameToLayer("P3");
        if (playerIndex == 4) layer = LayerMask.NameToLayer("P4");

        SetLayerAllChildren(m_input.transform, layer);
    }

    void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(true);
        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }
}
