using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using TMPro;

public class WagonService : MonoBehaviour
{
    //[SerializeField] private GameObject customerSeat;  
    
    [SerializeField] public int scoreGiven;
    //[SerializeField] public GameObject targetParticles;
    //public bool isInCart = false;
    [Space]
    [SerializeField] private GameObject[] destinationList;
    [Space]

    [Header("Capture Flag Toggle")]
    [SerializeField] public GameObject destinationMapIcon;

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
    [SerializeField] private GameObject[] zone1Destinations;
    [SerializeField] private GameObject[] zone2Destinations;
    [SerializeField] private GameObject[] zone3Destinations;
    [SerializeField] private GameObject[] zone4Destinations;
    [SerializeField] private GameObject[] zone5Destinations;
    private int zoneSelect;


    [Header("Debug")]
    [SerializeField] private float mapY = 300;
    [SerializeField] public GameObject destination;
    [SerializeField] private float X = 0;
    [SerializeField] private float Y = 0;
    [SerializeField] private float Z = 0;


    //[Header("Fare")]
    //[SerializeField] private TextMeshProUGUI fareText;



    ////Animations
    //private Animator _animator;
    //private string _currentAnimation;

    //const string NPC_ATTENTION = "Attention";
    //const string NPC_DANCE = "Dance";
    //const string NPC_FLAP = "Flap";
    //const string NPC_GRANNY = "Granny";
    //const string NPC_IDLE = "Idle";
    //const string NPC_SIREN = "Siren";
    //const string NPC_WALK = "Walk";

    public bool isAtTarget = false;

    private void Awake()
    {
        destinationMapIcon.SetActive(false);
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
        
        //_animator = this.gameObject.GetComponent<Animator>();
        //ChangeAnimation(NPC_ATTENTION);
        if(!captureFlagToggle && !zonedDeliveriesToggle)
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
        if (isAtTarget) { Debug.Log(isAtTarget); return; }
        wagonData = other.GetComponent<WagonData>();

        _wagonSlot = wagonData.wagonSlot;
        Debug.Log(wagonData.wagonSlot);

        if (zonedDeliveriesToggle == true && !wagonData.isOccupied)
        {
            ZoneSelector(zoneSelect);
            wagonData.destinationTarget = destination;
            Debug.Log("The destination is: " + destination);
            wagonData.isOccupied = true;
            this.transform.parent = _wagonSlot.transform;
            this.transform.position = this._wagonSlot.transform.position;
            transform.rotation = new Quaternion(X, Y, Z, 0);
        }


            if (captureFlagToggle == true && !wagonData.isOccupied)
        {
            destination = playerBaseList[wagonData.thisPlayerNumber - 1];
            Debug.Log("Flag picked up by player: " + wagonData.thisPlayerNumber);
            wagonData.destinationTarget = destination;
            Debug.Log("The destination is: " + destination);
            wagonData.isOccupied = true;
            this.transform.parent = _wagonSlot.transform;
            this.transform.position = this._wagonSlot.transform.position;            
            transform.rotation = new Quaternion(X, Y, Z, 0);

        }

        if (!wagonData.isOccupied && destination != null && !captureFlagToggle)
        {
            Debug.Log(destination);
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

        destinationMapIcon.transform.position = destination.transform.position;
        destinationMapIcon.transform.position = new Vector3(destination.transform.position.x, mapY, destination.transform.position.z);


        destinationMapIcon.SetActive(true);
    }

    public void SetTargetParticlesPosition()
    {
        //targetParticles.transform.position = destination.transform.position;
    }

    private void ChangeAnimation(string newAnimation)
    {
        ////prevents interupting the animation
        //if (_currentAnimation == newAnimation) return;

        //_animator.Play(newAnimation);
        //_currentAnimation = newAnimation;
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
        if (zn == 1) { list = zone1Destinations; RandomZoneList(list); }
        if (zn == 2) { list = zone2Destinations; RandomZoneList(list); }
        if (zn == 3) { list = zone3Destinations; RandomZoneList(list); }
        if (zn == 4) { list = zone4Destinations; RandomZoneList(list); }
        if (zn == 5) { list = zone5Destinations; RandomZoneList(list); }
    }

    private void RandomZoneList(GameObject[] list)
    {
        listLength = list.Length;
        int randomDestination = UnityEngine.Random.Range(0, listLength);
        destination = list[randomDestination];
    }

}


