using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class TaxiService : MonoBehaviour
{
    [SerializeField] private GameObject customerSeat;
    [SerializeField] public GameObject destination;
    [SerializeField] public int dollarsGiven;

    public static bool isInCart = false;

    [SerializeField] private Canvas _npcMapMarker; //change this temp fix
    [SerializeField] private Canvas _npcQuestIcon;//temp
    
    private NavMeshAgent agent;
    private GameObject _player;
    private float X;
    private float Y;
    private float Z;

    private void Awake()
    {
        _npcMapMarker.enabled = true; //temp
        agent = this.gameObject.GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        _player = PlayerData.player;
    }

    void LateUpdate()
    {
        transform.LookAt(_player.transform);
        transform.Rotate(X, Y, Z);
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

            _npcMapMarker.enabled = false; //temp
            _npcQuestIcon.enabled = false ; //temp

            destination.GetComponent<ArriveAtObjective>().minimapMarker.enabled = true;
        }
    }
}
