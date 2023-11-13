using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc : MonoBehaviour
{
    void FixedUpdate()
    {
        // Check if the GameObject has no children
        if (transform.childCount == 0)
        {
            // Destroy the empty GameObject
            Destroy(gameObject);
        }
    }
}
