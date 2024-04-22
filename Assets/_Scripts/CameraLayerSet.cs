using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLayerSet : MonoBehaviour
{
    private void Start()
    {
        List<GameObject> players = GameManager.Instance.players;
        int playerCount = players.Count;
        if (playerCount == 0) playerCount++;

        int layer = 0;

        if (playerCount == 1) layer = LayerMask.NameToLayer("P1");
        else if (playerCount == 2) layer = LayerMask.NameToLayer("P2");
        else if (playerCount == 3) layer = LayerMask.NameToLayer("P3");
        else if (playerCount == 4) layer = LayerMask.NameToLayer("P4");

        foreach (Transform child in transform) child.gameObject.layer = layer;

        Camera cam = GetComponentInChildren<Camera>();
        cam.cullingMask |= (1 << layer);
    }
}
