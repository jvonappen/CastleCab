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

    private int listLength;
    private float X;
    private float Y;
    private float Z;

    private WagonData wagonData;
    private GameObject _wagonSlot;

    //[Header("Timer")]
    //[SerializeField] private GameObject timerObject;
    //[SerializeField] private GameTimer timeValue;

    //[Header("Respawn")]
    ////private Transform _ogTrans;
    //[SerializeField] private float resetDelay = 5;




    [Header("Debug")]
    [SerializeField] public GameObject destination;


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
       // agent = this.gameObject.GetComponent<NavMeshAgent>();
        listLength = destinationList.Length;
        
    }
    private void Start()
    {
        
        //_animator = this.gameObject.GetComponent<Animator>();
        //ChangeAnimation(NPC_ATTENTION);

        int randomDestination = UnityEngine.Random.Range(0, listLength);
        destination = destinationList[randomDestination];

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
        wagonData = other.GetComponent<WagonData>();

        _wagonSlot = wagonData.wagonSlot;
        Debug.Log(wagonData.wagonSlot);

        if (!wagonData.isOccupied && destination != null)
        {
            Debug.Log(destination);
            //fareText.text = dollarsGiven.ToString();
            //AudioManager.Instance.PlaySFX("In");
            //this.transform.parent = this.customerSeat.transform;
            //this.transform.position = this.customerSeat.transform.position;

            this.transform.parent = _wagonSlot.transform;
            this.transform.position = this._wagonSlot.transform.position;



            //ChangeAnimation(NPC_FLAP);
            //targetParticles.SetActive(true);

            wagonData.destinationTarget = destination;
            wagonData.destinationTarget = debugDestination;
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
}
