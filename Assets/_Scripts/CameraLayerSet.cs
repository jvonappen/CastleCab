using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLayerSet : MonoBehaviour
{
    [SerializeField] PlayerInput m_input;

    private void Start()
    {
        int playerNum = m_input.user.index + 1;

        //List<GameObject> players = GameManager.Instance.players;
        //int playerNum = players.Count;
        //if (playerNum == 0) playerNum++;

        int layer = 0;

        if (playerNum == 1) layer = LayerMask.NameToLayer("P1");
        else if (playerNum == 2) layer = LayerMask.NameToLayer("P2");
        else if (playerNum == 3) layer = LayerMask.NameToLayer("P3");
        else if (playerNum == 4) layer = LayerMask.NameToLayer("P4");

        foreach (Transform child in transform) child.gameObject.layer = layer;

        Camera cam = GetComponentInChildren<Camera>();
        cam.cullingMask |= (1 << layer);
    }
}
