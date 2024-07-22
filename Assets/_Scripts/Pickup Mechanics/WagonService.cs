using UnityEngine;

public class WagonService : MonoBehaviour
{
    #region Variables

    [SerializeField] public int scoreGiven;

    //[Space]
    //[SerializeField] private GameObject[] destinationList;
    //[Space]

    [SerializeField] private GameObject m_pickupMarker;

    private int listLength;
 

    private WagonData currentWagonData;
    private WagonData previousWagonData;

    private GameObject m_wagonSlot;


    [Header("Zoned Deliveries Toggle")]
    [SerializeField] public bool zonedDeliveriesToggle;
    
    [SerializeField] private int thisZoneNumber;
    [SerializeField] private float m_stolenCoolDownTimer = 5;

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
    [SerializeField] private Vector3 og_transform;

    [SerializeField] private bool canBeStolen = true;

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

    #endregion



    #region Init
    private void Awake()
    {
        //listLength = destinationList.Length;
        zoneSelect = RandomIntExcept(1, 5, thisZoneNumber);

        og_transform = transform.position;
    }
    private void Start()
    {
        if (DM == null)
        {
            DM = DestinationManager.Instance;
            StartRefs();        
        }

        _animator = GetComponentInChildren<Animator>();
        ChangeAnimation(NPC_ATTENTION);
        //if (!captureFlagToggle && !zonedDeliveriesToggle)
        //{
        //    int randomDestination = UnityEngine.Random.Range(0, listLength);
        //    destination = destinationList[randomDestination];
        //}

    }
    #endregion

    #region Animation
    public void ChangeAnimation(string newAnimation)
    {
        //prevents interupting the animation
        if (_currentAnimation == newAnimation) return;

        _animator.Play(newAnimation);
        _currentAnimation = newAnimation;
    }
    #endregion

    #region Markers/Zones
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

    #endregion



    #region Callbacks
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Wagon") return;
        if (isAtTarget) return;
        WagonData collidingWagon = other.GetComponent<WagonData>();

        if (!collidingWagon.isOccupied)
        {
            if (zonedDeliveriesToggle == true)
            {
                if (currentlyInCart && canBeStolen) StealPassengerMode(collidingWagon);
                else if (!currentlyInCart) PassengerPickupMode(collidingWagon);
            }

            //if(!zonedDeliveriesToggle && captureFlagToggle == true) { CaptureTheFlagMode(collidingWagon); }
        }
        else
        {
            collidingWagon.playerHealth.transform.parent.GetComponentInChildren<PopupText>(true).DisplayText("Cart is already occupied!");
        }
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

                ResetRespawn();

            }, m_vanishTimer);
        }

        currentWagonData.OnDropOff();
    }
    #endregion

    #region PassengerPickup

    #region PassengerSteal
    private void StealPassengerMode(WagonData _wagon)
    {
        PickupObjective(_wagon);

        StealPassenger();
    }

    void StealPassenger()
    {
        PlayerMarkerSelect(previousWagonData.thisPlayerNumber, false);

        canBeStolen = false;
        TimerManager.RunAfterTime(() => { canBeStolen = true; }, m_stolenCoolDownTimer);

        previousWagonData.isOccupied = false;

        previousWagonData.OnPassengerStolen();

        SetPassenger();
    }
    #endregion

    private void PassengerPickupMode(WagonData _wagon)
    {
        ZoneSelector(zoneSelect); // Sets destination

        PickupObjective(_wagon);

        ChangeAnimation(NPC_FLAP);

        SetPassenger();
    }

    void SetPassenger() //Pickup Passenger
    {
        AudioManager.Instance.PlaySFX("In");
        currentlyInCart = true;

        PlayerMarkerSelect(currentWagonData.thisPlayerNumber, true);

        currentPlayer = currentWagonData.thisPlayerNumber;
        previousWagonData = currentWagonData;
    }

    void PickupObjective(WagonData _wagon)
    {
        currentWagonData = _wagon;
        m_wagonSlot = currentWagonData.wagonSlot;

        m_pickupMarker.SetActive(false);
        currentWagonData.PlayPickUpParticle();

        currentWagonData.destinationTarget = destination;
        currentWagonData.isOccupied = true;
        transform.parent = m_wagonSlot.transform;
        transform.position = m_wagonSlot.transform.position;
        transform.rotation = new Quaternion(X, Y, Z, 0);

        currentWagonData.OnPickup(destination.transform);
    }

    //private void CaptureTheFlagMode(WagonData _wagon)
    //{
    //    destination = playerBaseList[_wagon.thisPlayerNumber - 1];
    //
    //    PickupObjective();
    //}
    #endregion

    #region Respawn
    private void ResetRespawn()
    {
        transform.position = og_transform;
        
        isAtTarget = false;
        currentlyInCart = false;

        canBeStolen = true;

        zoneSelect = RandomIntExcept(1, 5, thisZoneNumber);

        m_pickupMarker.SetActive(true);
    
        _animator = GetComponentInChildren<Animator>();
        

        gameObject.SetActive(true);

        ChangeAnimation(NPC_ATTENTION);
    }
    #endregion
}


