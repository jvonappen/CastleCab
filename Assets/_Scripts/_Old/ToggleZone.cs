using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleZone : MonoBehaviour
{
    [SerializeField] private GameObject breakableObjects;
    [SerializeField] private GameObject wanderingNPCS;
    [SerializeField] private GameObject otherStuff;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(breakableObjects != null)
            {
                breakableObjects.SetActive(true);
            }
            if (wanderingNPCS != null)
            {
                wanderingNPCS.SetActive(true);
            }
            if (otherStuff != null)
            {
                otherStuff.SetActive(true);
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (breakableObjects != null)
        {
            breakableObjects.SetActive(false);
        }
        if (wanderingNPCS != null)
        {
            wanderingNPCS.SetActive(false);
        }
        if (otherStuff != null)
        {
            otherStuff.SetActive(false);
        }
    }
}
