using System.Collections;

using System.Collections.Generic;

using UnityEngine;



[RequireComponent(typeof(Renderer))]

public class ObstructionTransparency : MonoBehaviour
{

    private Renderer rend;



    //public LayerMask raycastLayerMask; // Set this in the Inspector



    void Start()

    {

        rend = GetComponent<Renderer>();

    }



    void Update()

    {

        RaycastHit hit;

        Vector3 directionToObject = transform.position - Camera.main.transform.position;

        float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);



        Debug.DrawRay(Camera.main.transform.position, directionToObject, Color.red);



        if (Physics.Raycast(Camera.main.transform.position, directionToObject, out hit, distanceToCamera))



        {

            if (hit.transform == this.transform)

            {

                // If the ray hits the object this script is attached to before hitting anything else

                rend.material.SetFloat("_DitherThreshold", 0.5f); // Make the object transparent

            }

            else

            {

                rend.material.SetFloat("_DitherThreshold", 0f); // Make the object opaque

            }



            Debug.Log("Raycast hit: " + hit.transform.name);

        }

    }

}