using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using TMPro;

public class TaxiService : MonoBehaviour
{
    [SerializeField] private GameObject customerSeat;
    [SerializeField] public GameObject destination;
    [SerializeField] public int dollarsGiven;

    [SerializeField] public GameObject targetParticles;

    public static bool isInCart = false;

    //[SerializeField] private Canvas _npcMapMarker; //change this temp fix
    //[SerializeField] private Canvas _npcQuestIcon;//temp
    
    private NavMeshAgent agent;
    private GameObject _player;
    private float X;
    private float Y;
    private float Z;

    [Header("Timer")]
    [SerializeField] private GameObject timerObject;
    [SerializeField] private Timer timeValue;

    [Header("Fare")]
    [SerializeField] private TextMeshProUGUI fareText;

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
       // _npcMapMarker.enabled = true; //temp
        agent = this.gameObject.GetComponent<NavMeshAgent>();

    }
    private void Start()
    {
        _player = PlayerData.player;
        _animator = this.gameObject.GetComponent<Animator>();
        ChangeAnimation(NPC_ATTENTION);


    }

    void LateUpdate()
    {
        transform.LookAt(_player.transform);
        transform.Rotate(X, Y, Z);
       
        //fareText.text = dollarsGiven.ToString();
        if(isAtTarget == true) { ChangeAnimation(NPC_DANCE); }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!PlayerData.isOccupied && other.tag == "Player" && destination != null)
        {
            //fareText.text = dollarsGiven.ToString();
            AudioManager.Instance.PlaySFX("In");
            this.transform.parent = this.customerSeat.transform;
            this.transform.position = this.customerSeat.transform.position;

            ChangeAnimation(NPC_FLAP);

            PlayerData.cartDestinationTarget = destination;
            PlayerData.isOccupied = true;

            CompassBar.objectiveObjectTransform = destination.transform;
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false; 
            this.gameObject.GetComponentInChildren<Canvas>().enabled = false;

            this.agent.enabled = false;
            isInCart = true;

            //_npcMapMarker.enabled = false; //temp
            //_npcQuestIcon.enabled = false ; //temp

            //destination.GetComponent<ArriveAtObjective>().minimapMarker.enabled = true;

            SetTargetParticlesPosition();
            timeValue.inService = true;
            timeValue.timerValue = 60;
            
            
            timerObject.SetActive(true);

        }
    }

    public void SetTargetParticlesPosition()
    {
        targetParticles.transform.position = destination.transform.position;
    }

    private void ChangeAnimation(string newAnimation)
    {
        //prevents interupting the animation
        if (_currentAnimation == newAnimation) return;

        _animator.Play(newAnimation);
        _currentAnimation = newAnimation;
    }
}
