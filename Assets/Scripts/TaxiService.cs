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
    [SerializeField] private GameObject customerSeat;
    [SerializeField] public GameObject destination;
    [SerializeField] public int dollarsGiven;

    public static bool isInCart = false;

    [SerializeField] private Canvas minimapLocationMarker; //change this temp fix
    [SerializeField] private Canvas minimapQuestMarker;//temp
    
    private NavMeshAgent agent;
    private PlayerData chair;

    private void Awake()
    {
        minimapLocationMarker.enabled = false; //temp
        agent = this.gameObject.GetComponent<NavMeshAgent>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (!PlayerData.isOccupied && other.tag == "Player" && destination != null)
        {
            AudioManager.Instance.PlaySFX("In");
            this.transform.parent = this.customerSeat.transform;
            this.transform.position = this.customerSeat.transform.position;

            PlayerData.cartDestinationTarget = destination;
            PlayerData.isOccupied = true;

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
