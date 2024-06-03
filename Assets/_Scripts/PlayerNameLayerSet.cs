using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNameLayerSet : MonoBehaviour
{
    [SerializeField] PlayerInput m_input;
    private void Awake()
    {
        int playerIndex = m_input.user.index;
        if (playerIndex == 0) gameObject.layer = LayerMask.NameToLayer("!P1");
        if (playerIndex == 1) gameObject.layer = LayerMask.NameToLayer("!P2");
        if (playerIndex == 2) gameObject.layer = LayerMask.NameToLayer("!P3");
        if (playerIndex == 3) gameObject.layer = LayerMask.NameToLayer("!P4");
    }
}
