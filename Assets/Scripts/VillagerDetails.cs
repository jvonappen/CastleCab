using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillagerDetails : MonoBehaviour
{
    [SerializeField] private GameObject cartTarget;
    [SerializeField] public GameObject destination;

    [SerializeField] public int dollarsGiven;

    public static bool isInCart = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!CartDetails.isOccupied && other.tag == "Player")
        {
            AudioManager.Instance.PlaySFX("In");
            transform.parent = cartTarget.transform;
            transform.position = cartTarget.transform.position;

            CartDetails.cartDestinationTarget = destination;
            CartDetails.isOccupied = true;

            CompassBar.objectiveObjectTransform = destination.transform;
            //atDestination = false;
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false; 
            this.gameObject.GetComponentInChildren<Canvas>().enabled = false;

            this.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            isInCart = true;
        }
    }
}
