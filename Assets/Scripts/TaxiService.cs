using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//public class TaxiDetails : MonoBehaviour
//{
//    public static bool isOccupied;
//    public static GameObject cartDestinationTarget;
//}

public class TaxiService : MonoBehaviour
{
    [SerializeField] private GameObject cartTarget;
    [Space]
    [SerializeField] public GameObject destination;
    [SerializeField] public int dollarsGiven;

    public static bool isInCart = false;

    [SerializeField] private Canvas minimapLocationMarker; //change this temp fix
    [SerializeField] private Canvas minimapQuestMarker;//temp
    
    private NavMeshAgent agent;

    private void Awake()
    {
        minimapLocationMarker.enabled = false; //temp
        agent = this.gameObject.GetComponent<NavMeshAgent>();

       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!CartDetails.isOccupied && other.tag == "Player" && destination != null)
        {
            AudioManager.Instance.PlaySFX("In");
            this.transform.parent = this.cartTarget.transform;
            this.transform.position = this.cartTarget.transform.position;

            CartDetails.cartDestinationTarget = destination;
            CartDetails.isOccupied = true;

            CompassBar.objectiveObjectTransform = destination.transform;
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false; 
            this.gameObject.GetComponentInChildren<Canvas>().enabled = false;

            this.agent.enabled = false;
            isInCart = true;

            minimapLocationMarker.enabled = true; //temp
            minimapQuestMarker.enabled = false ; //temp
        }
    }
}
