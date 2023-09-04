using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstructionTransparency : MonoBehaviour
{
    public Transform player; // set in the inspector
    private Renderer currentObstructingRenderer; // Renderer of the current obstructing object
    private RaycastHit hit;
    public LayerMask layerMask;

    void Update()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        Debug.DrawRay(transform.position, directionToPlayer, Color.red);

        // Check for obstructions
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, distanceToPlayer, layerMask))
        {
           // Debug.Log("Raycast hit: " + hit.transform.name);
            
            if (hit.transform != player)
            {
                Renderer rend = hit.transform.GetComponent<Renderer>();
                if (rend != null && rend != currentObstructingRenderer) // If the hit object has a Renderer component and is not the current obstructing object
                {
                    // Reset transparency of the previous obstructing object
                    if (currentObstructingRenderer != null)
                    {
                        currentObstructingRenderer.material.SetFloat("_DitherThreshold", 0f);
                    }

                    // Set transparency of the current obstructing object
                    rend.material.SetFloat("_DitherThreshold", 0.5f);
                    currentObstructingRenderer = rend;
                }
            }
        }
        else
        {
            // If no obstruction is found, reset transparency of the previous obstructing object
            if (currentObstructingRenderer != null)
            {
                currentObstructingRenderer.material.SetFloat("_DitherThreshold", 0f);
                currentObstructingRenderer = null;
            }
        }
    }
}
