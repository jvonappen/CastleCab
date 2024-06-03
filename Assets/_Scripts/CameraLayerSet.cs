using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLayerSet : MonoBehaviour
{
    [SerializeField] PlayerInput m_input;

    private void Start()
    {
        Camera cam = GetComponentInChildren<Camera>();

        int playerNum = m_input.user.index + 1;

        //List<GameObject> players = GameManager.Instance.players;
        //int playerNum = players.Count;
        //if (playerNum == 0) playerNum++;


        int layer = 0;
        if (playerNum == 1)
        {
            layer = LayerMask.NameToLayer("P1");

            cam.cullingMask |= (1 << LayerMask.NameToLayer("!P2"));
            cam.cullingMask |= (1 << LayerMask.NameToLayer("!P3"));
            cam.cullingMask |= (1 << LayerMask.NameToLayer("!P4"));
        }
        else if (playerNum == 2)
        {
            layer = LayerMask.NameToLayer("P2");

            cam.cullingMask |= (1 << LayerMask.NameToLayer("!P1"));
            cam.cullingMask |= (1 << LayerMask.NameToLayer("!P3"));
            cam.cullingMask |= (1 << LayerMask.NameToLayer("!P4"));
        }
        else if (playerNum == 3)
        {
            layer = LayerMask.NameToLayer("P3");

            cam.cullingMask |= (1 << LayerMask.NameToLayer("!P1"));
            cam.cullingMask |= (1 << LayerMask.NameToLayer("!P2"));
            cam.cullingMask |= (1 << LayerMask.NameToLayer("!P4"));
        }
        else if (playerNum == 4)
        {
            layer = LayerMask.NameToLayer("P4");

            cam.cullingMask |= (1 << LayerMask.NameToLayer("!P1"));
            cam.cullingMask |= (1 << LayerMask.NameToLayer("!P2"));
            cam.cullingMask |= (1 << LayerMask.NameToLayer("!P3"));
        }

        foreach (Transform child in transform) child.gameObject.layer = layer;

        
        cam.cullingMask |= (1 << layer);
    }
}
