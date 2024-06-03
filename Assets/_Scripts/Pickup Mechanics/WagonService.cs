using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using TMPro;
using UnityEngine.Timeline;

public class WagonService : MonoBehaviour
{   
    [SerializeField] public int scoreGiven;

    [Space]
    [SerializeField] private GameObject[] destinationList;
    [Space]

    [SerializeField] private GameObject m_pickupMarker;

    private int listLength;
 

    private WagonData wagonData_A;
    private WagonData wagonData_B;

    private GameObject m_wagonSlot;

    //[Header("Timer")]
    //[SerializeField] private GameObject timerObject;
    //[SerializeField] private GameTimer timeValue;

    //[Header("Respawn")]
    ////private Transform _ogTrans;
    //[SerializeField] private float resetDelay = 5;

    [Header("Capture Flag Toggle")]
    [SerializeField] public bool captureFlagToggle;
    [SerializeField] private GameObject[] playerBaseList;
    private int playerListNumberPos;

    [Header("Zoned Deliveries Toggle")]
    [SerializeField] public bool zonedDeliveriesToggle;
    public bool canSteal = true;
    [SerializeField] private int thisZoneNumber;

    [SerializeField] private bool currentlyInCart;
    [SerializeField] private int currentPlayer;

    [SerializeField] private bool m_canVanish = true;

    [SerializeField] private float m_vanishTimer = 10;

    [Header("DM - Debug")]
    [SerializeField] private DestinationManager DM;
    private GameObject[] z1;
    private GameObject[] z2;
    private GameObject[] z3;
    private GameObject[] z4;
    private GameObject[] z5;
    private int zoneSelect;

    //Minimap Marker
    private GameObject tmP1, tmP2, tmP3, tmP4;
    [SerializeField]public GameObject thisPlayerMarker;
    //Beams
    private GameObject bmP1, bmP2, bmP3, bmP4;
    [SerializeField] public GameObject thisPlayerBeam;


    [Header("Debug")]
    [SerializeField] private float mapY = 300;
    [SerializeField] public GameObject destination;
    [SerializeField] private float X = 0;
    [SerializeField] private float Y = 0;
    [SerializeField] private float Z = 0;


    //[Header("Fare")]
    //[SerializeField] private TextMeshProUGUI fareText;



    //Animations
    private Animator _animator;
    private string _currentAnimation;

    const string NPC_ATTENTION = "Attention";
    const string NPC_DANCE = "Dance";
    const string NPC_FLAP = "Flap";
    const string NPC_GRANNY = "Granny";
    const string NPC_IDLE = "Idle";
    const string NPC_SIREN = "Siren";
    const string NPC_WALK = "Walk";

    public bool isAtTarget = false;

    private void Awake()
    {
        listLength = destinationList.Length;
        zoneSelect = RandomIntExcept(1, 5, thisZoneNumber);
    }
    private void Start()
    {
        if (DM == null)
        {
            DM = DestinationManager.Instance;
            StartRefs();        
        }

        _animator = this.gameObject.GetComponentInChildren<Animator>();
        ChangeAnimation(NPC_ATTENTION);
        if (!captureFlagToggle && !zonedDeliveriesToggle)
        {
            int randomDestination = UnityEngine.Random.Range(0, listLength);
            destination = destinationList[randomDestination];
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Wagon") return;
        if (isAtTarget) return;
        wagonData_A = other.GetComponent<WagonData>();

        m_wagonSlot = wagonData_A.wagonSlot;

        if (zonedDeliveriesToggle == true) { PassengerPickupMode(); }
        if (zonedDeliveriesToggle == true && canSteal == true) { StealPassengerMode(); }
        if(!zonedDeliveriesToggle && captureFlagToggle == true) { CaptureTheFlagMode(); }
    }

    public void ChangeAnimation(string newAnimation)
    {
        //prevents interupting the animation
        if (_currentAnimation == newAnimation) return;

        _animator.Play(newAnimation);
        _currentAnimation = newAnimation;
    }

    public void OnDropOff(GameObject particles)
    {
        transform.parent = null;
        isAtTarget = true;
        destination = null;

        ChangeAnimation("Dance");

        if (m_canVanish)
        {
            TimerManager.RunAfterTime(() =>
            {
                if (particles != null) { particles.SetActive(true); }
                gameObject.SetActive(false);
            }, m_vanishTimer);
        }
    }


    private int RandomIntExcept(int min, int max, int except)
    {
        int result = Random.Range(min, max - 1);
        if (result >= except) result += 1;
        if (result > max) result = 1;
        return result;
    }

    private void ZoneSelector(int zn)
    {
        GameObject[] list;
        if (zn == 1) { list = z1; RandomZoneList(list); }
        if (zn == 2) { list = z2; RandomZoneList(list); }
        if (zn == 3) { list = z3; RandomZoneList(list); }
        if (zn == 4) { list = z4; RandomZoneList(list); }
        if (zn == 5) { list = z5; RandomZoneList(list); }
    }

    private void RandomZoneList(GameObject[] list)
    {
        listLength = list.Length;
        int randomDestination = UnityEngine.Random.Range(0, listLength);
        destination = list[randomDestination];
    }

    private void PlayerMarkerSelect(int pn, bool active)
    {
        if (pn == 1) { thisPlayerMarker = tmP1; thisPlayerBeam = bmP1; MarkerPlacement(thisPlayerMarker, thisPlayerBeam, active); }
        if (pn == 2) { thisPlayerMarker = tmP2; thisPlayerBeam = bmP2; MarkerPlacement(thisPlayerMarker, thisPlayerBeam, active); }
        if (pn == 3) { thisPlayerMarker = tmP3; thisPlayerBeam = bmP3; MarkerPlacement(thisPlayerMarker, thisPlayerBeam, active); }
        if (pn == 4) { thisPlayerMarker = tmP4; thisPlayerBeam = bmP4; MarkerPlacement(thisPlayerMarker, thisPlayerBeam, active); }

    }

    private void MarkerPlacement(GameObject marker, GameObject beam, bool active)
    {
        marker.transform.position = destination.transform.position;
        marker.transform.position = new Vector3(destination.transform.position.x, mapY, destination.transform.position.z);
        marker.SetActive(active);

        beam.transform.position = destination.transform.position;
        beam.SetActive(active);
    }

    private void StartRefs()
    {
        z1 = DM.zone1DestinationsValley;
        z2 = DM.zone2DestinationsDock;
        z3 = DM.zone3DestinationsGraveyard;
        z4 = DM.zone4DestinationsTown;
        z5 = DM.zone5DestinationsHilltop;

        tmP1 = DM.targetMarkerP1; tmP1.SetActive(false);
        tmP2 = DM.targetMarkerP2; tmP2.SetActive(false);
        tmP3 = DM.targetMarkerP3; tmP3.SetActive(false);
        tmP4 = DM.targetMarkerP4; tmP4.SetActive(false);

        bmP1 = DM.beamP1; bmP1.SetActive(false);
        bmP2 = DM.beamP2; bmP2.SetActive(false);
        bmP3 = DM.beamP3; bmP3.SetActive(false);
        bmP4 = DM.beamP4; bmP4.SetActive(false);
    }

    private void CaptureTheFlagMode()
    {
        if (captureFlagToggle == true && !wagonData_A.isOccupied) //Capture Flag
        {
            m_pickupMarker.SetActive(false);
            wagonData_A.PlayPickUpParticle();
            destination = playerBaseList[wagonData_A.thisPlayerNumber - 1];
            wagonData_A.destinationTarget = destination;
            wagonData_A.isOccupied = true;
            this.transform.parent = m_wagonSlot.transform;
            this.transform.position = this.m_wagonSlot.transform.position;
            transform.rotation = new Quaternion(X, Y, Z, 0);

        }
    }

    private void PassengerPickupMode()
    {
        if (zonedDeliveriesToggle == true && !wagonData_A.isOccupied && !currentlyInCart)
        {
            m_pickupMarker.SetActive(false);
            wagonData_A.PlayPickUpParticle();
            AudioManager.Instance.PlaySFX("In");
            currentlyInCart = true;
            ZoneSelector(zoneSelect);
            wagonData_A.destinationTarget = destination;
            wagonData_A.isOccupied = true;
            this.transform.parent = m_wagonSlot.transform;
            this.transform.position = this.m_wagonSlot.transform.position;
            transform.rotation = new Quaternion(X, Y, Z, 0);
            ChangeAnimation(NPC_FLAP);
            PlayerMarkerSelect(wagonData_A.thisPlayerNumber, true);
            currentPlayer = wagonData_A.thisPlayerNumber;
            wagonData_B = wagonData_A;
        }
    }

    private void StealPassengerMode()
    {
        if (zonedDeliveriesToggle == true && !wagonData_A.isOccupied && currentlyInCart == true)
        {
            m_pickupMarker.SetActive(false);
            wagonData_A.PlayPickUpParticle();
            AudioManager.Instance.PlaySFX("In");
            currentlyInCart = true;
            wagonData_A.isOccupied = true;
            this.transform.parent = m_wagonSlot.transform;
            this.transform.position = this.m_wagonSlot.transform.position;
            transform.rotation = new Quaternion(X, Y, Z, 0);
            PlayerMarkerSelect(wagonData_A.thisPlayerNumber, true);
            PlayerMarkerSelect(wagonData_B.thisPlayerNumber, false);
            wagonData_B.isOccupied = false;

            currentPlayer = wagonData_A.thisPlayerNumber;
            wagonData_B = wagonData_A;
        }
    }

    private void BasicDeliveryMode()
    {
        if (!wagonData_A.isOccupied && destination != null && !captureFlagToggle)
        {
            m_pickupMarker.SetActive(false);


            this.transform.parent = m_wagonSlot.transform;
            this.transform.position = this.m_wagonSlot.transform.position;
            transform.rotation = new Quaternion(X, Y, Z, 0);

            wagonData_A.destinationTarget = destination;

            wagonData_A.isOccupied = true;
        }
    }
}


