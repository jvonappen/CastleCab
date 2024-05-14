using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using TMPro;
using UnityEngine.Timeline;

public class WagonService : MonoBehaviour
{
    //[SerializeField] private GameObject customerSeat;  
    
    [SerializeField] public int scoreGiven;
    //[SerializeField] public GameObject targetParticles;
    //public bool isInCart = false;
    [Space]
    [SerializeField] private GameObject[] destinationList;
    [Space]



    private int listLength;
 

    private WagonData wagonData;
    private GameObject _wagonSlot;

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
    [SerializeField] private int thisZoneNumber;
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
        //if(DM == null)
        //{
        //    DM = DestinationManager.Instance;
        //    StartRefs();
        //}



        // agent = this.gameObject.GetComponent<NavMeshAgent>();
        listLength = destinationList.Length;

        zoneSelect = RandomIntExcept(1, 5, thisZoneNumber);



        ////////////////////////////////////////////////////////////////////////////TimerManager.RunAfterTime(() =>
        ////////////////////////////////////////////////////////////////////////////{
        ////////////////////////////////////////////////////////////////////////////    // Do stuff
        ////////////////////////////////////////////////////////////////////////////}, 1);

        ////////////////////////////////////////////////////////////////////////////TimerManager.RunAfterTime(Start, 1);
        
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

        //customerSeat = WagonData.wagonSlot;


    }

    void LateUpdate()
    {
        //transform.LookAt(_wagonSlot.transform);
        //transform.Rotate(X, Y, Z);
       
        //fareText.text = dollarsGiven.ToString();
       // if(isAtTarget == true) { ChangeAnimation(NPC_DANCE); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Wagon") return;
        if (isAtTarget) return;
        wagonData = other.GetComponent<WagonData>();

        _wagonSlot = wagonData.wagonSlot;

        if (zonedDeliveriesToggle == true && !wagonData.isOccupied)
        {
            ZoneSelector(zoneSelect);
            wagonData.destinationTarget = destination;
            wagonData.isOccupied = true;
            this.transform.parent = _wagonSlot.transform;
            this.transform.position = this._wagonSlot.transform.position;
            transform.rotation = new Quaternion(X, Y, Z, 0);
            ChangeAnimation(NPC_FLAP);
        }


        if (captureFlagToggle == true && !wagonData.isOccupied)
        {
            destination = playerBaseList[wagonData.thisPlayerNumber - 1];
            wagonData.destinationTarget = destination;
            wagonData.isOccupied = true;
            this.transform.parent = _wagonSlot.transform;
            this.transform.position = this._wagonSlot.transform.position;            
            transform.rotation = new Quaternion(X, Y, Z, 0);

        }

        if (!wagonData.isOccupied && destination != null && !captureFlagToggle)
        {
            //fareText.text = dollarsGiven.ToString();
            //AudioManager.Instance.PlaySFX("In");
            //this.transform.parent = this.customerSeat.transform;
            //this.transform.position = this.customerSeat.transform.position;

            this.transform.parent = _wagonSlot.transform;
            this.transform.position = this._wagonSlot.transform.position;
            transform.rotation = new Quaternion(X, Y, Z,0);


            //ChangeAnimation(NPC_FLAP);
            //targetParticles.SetActive(true);

            wagonData.destinationTarget = destination;
         
            wagonData.isOccupied = true;

           // CompassBar.objectiveObjectTransform = destination.transform;
            //this.gameObject.GetComponent<CapsuleCollider>().enabled = false; 
            //this.gameObject.GetComponentInChildren<Canvas>().enabled = false;
            //this.gameObject.GetComponentInChildren<ParticleSystem>().Stop();

           // this.agent.enabled = false;
            //isInCart = true;

            //SetTargetParticlesPosition();
            //timeValue.inService = true;
            //timeValue.timerValue = 45;
            
            
            //timerObject.SetActive(true);

        }

        PlayerMarkerSelect(wagonData.thisPlayerNumber);
    }

    public void SetTargetParticlesPosition()
    {
        //targetParticles.transform.position = destination.transform.position;
    }

    public void ChangeAnimation(string newAnimation)
    {
        //prevents interupting the animation
        if (_currentAnimation == newAnimation) return;

        _animator.Play(newAnimation);
        _currentAnimation = newAnimation;
    }


    //public void ResetTaxiPickUp()
    //{
    //    StartCoroutine(ResetWait());
    //}

    //IEnumerator ResetWait()
    //{
    //   // Debug.Log("Doing a wait");
    //   //// yield return new WaitForSeconds(resetDelay);
    //   // //this.gameObject.transform.position = _ogTrans.position;
    //   // isAtTarget = false;
    //   // int randomDestination = UnityEngine.Random.Range(0, listLength);
    //   // destination = destinationList[randomDestination];
    //   // //ChangeAnimation(NPC_ATTENTION);
    //   // this.gameObject.GetComponentInChildren<Canvas>().enabled = true;
    //   // this.gameObject.GetComponentInChildren<ParticleSystem>().Play();
    //   // Debug.Log("Did a reset");
    //}

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

    private void PlayerMarkerSelect(int pn)
    {
        if (pn == 1) { thisPlayerMarker = tmP1; thisPlayerBeam = bmP1; MarkerPlacement(thisPlayerMarker, thisPlayerBeam); }
        if (pn == 2) { thisPlayerMarker = tmP2; thisPlayerBeam = bmP2; MarkerPlacement(thisPlayerMarker, thisPlayerBeam); }
        if (pn == 3) { thisPlayerMarker = tmP3; thisPlayerBeam = bmP3; MarkerPlacement(thisPlayerMarker, thisPlayerBeam); }
        if (pn == 4) { thisPlayerMarker = tmP4; thisPlayerBeam = bmP4; MarkerPlacement(thisPlayerMarker, thisPlayerBeam); }

    }

    private void MarkerPlacement(GameObject marker, GameObject beam)
    {
        marker.transform.position = destination.transform.position;
        marker.transform.position = new Vector3(destination.transform.position.x, mapY, destination.transform.position.z);
        marker.SetActive(true);

        beam.transform.position = destination.transform.position;
        beam.SetActive(true);
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
}


