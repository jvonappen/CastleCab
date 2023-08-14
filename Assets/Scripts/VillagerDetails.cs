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

    [SerializeField] private Canvas minimapLocationMarker; //change this temp fix

    [SerializeField] private Canvas minimapQuestMarker;//temp
    [SerializeField] private NavMeshAgent agent; //temp

    private void Start()
    {
        minimapLocationMarker.enabled = false; //temp
        agent = this.gameObject.GetComponent<NavMeshAgent>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!CartDetails.isOccupied && other.tag == "Player")
        {
            AudioManager.Instance.PlaySFX("In");
            this.transform.parent = this.cartTarget.transform;
            this.transform.position = this.cartTarget.transform.position;

            CartDetails.cartDestinationTarget = destination;
            CartDetails.isOccupied = true;

            CompassBar.objectiveObjectTransform = destination.transform;
            //atDestination = false;
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false; 
            this.gameObject.GetComponentInChildren<Canvas>().enabled = false;

            //this.gameObject.GetComponent<NavMeshAgent>().enabled = false; //temp
            this.agent.enabled = false; //temp
            isInCart = true;

            minimapLocationMarker.enabled = true; //temp
            minimapQuestMarker.enabled = false ; //temp
        }
    }
}
