using UnityEngine;
using UnityEngine.AI;
using URNTS;

public class PoliceAI : MonoBehaviour
{
#pragma warning disable CS0414
    [SerializeField] private Transform _playerTransform;

    [Header("Wander")]
    [Tooltip("The center of the wandering radius. Use a fixed object to keep the wander area fixed. Use the object itself, to have complete free roam.")]
    [SerializeField] private Transform wanderTransform;
    [Tooltip("How far the AI will wander from the wander tranform.")]
    [SerializeField] private float wanderRange = 25;
    [Tooltip("The normal speed the gaurd moves. If left at 0, the speed will default to the navMesh speed.")]
    [SerializeField] private float movementSpeed = 3.5f;

    [SerializeField] private GuardChaseData data;

    [HideInInspector] public NavMeshAgent agent;
    Transform thisTransform;

    GameManager m_manager;

    [Header("Debug")]
    [SerializeField] bool m_showDebug;
    [ConditionalHide("m_showDebug")] [SerializeField] private float chasingRange0, chaseSpeed0, searchRange0;
    [ConditionalHide("m_showDebug")] [SerializeField] private bool sirenToggle;
    [ConditionalHide("m_showDebug")] [SerializeField] private bool inPursuit = false;

    [SerializeField] GameObject m_despawnPoofParticle;

    bool m_isAggressive;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        thisTransform = agent.transform;
        
    }
    private void Start()
    {
        m_manager = GameManager.Instance;
    }

    void SetPlayerTarget()
    {
        Transform closestPlayer = null;
        float closestDist = float.MaxValue;
        //foreach (GameObject player in m_manager.players)
        foreach (PlayerData data in m_manager.players)
        {
            Transform horseTransform = data.player.transform.GetChild(1);
            float playerDist = Vector3.Distance(thisTransform.position, horseTransform.position);
            if (playerDist < closestDist)
            {
                closestDist = playerDist;
                closestPlayer = horseTransform;
            }
        }

        _playerTransform = closestPlayer;
    }

    private void Update()
    {
        if (agent.enabled)
        {
            DishonourEvaluate();
            SetPlayerTarget(); // TODO - optimize
        }
    }


    private void DishonourEvaluate()
    {
        chaseSpeed0 = data.chaseSpeed1;
        searchRange0 = data.searchRange1;
        chasingRange0 = data.chasingRange1;

        InRange();

        agent.speed = chaseSpeed0;
    }

    public void SetAggressive(bool _isAggressive)
    {
        bool willRespawn = false;
        VehicleController controller = GetComponent<VehicleController>();

        if (m_isAggressive && !_isAggressive) // Stop being aggressive
        {
            agent.enabled = false;
            if (controller) willRespawn = true;
        }
        else if (!m_isAggressive && _isAggressive) // Start being aggresive
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(thisTransform.position, out hit, 2, 1))
            {
                agent.Warp(hit.position);
                agent.enabled = true;
            }
        }

        m_isAggressive = _isAggressive;

        if (willRespawn) RespawnVehicle(controller);
    }

    Transform m_particleParent = null;
    void RespawnVehicle(VehicleController _controller)
    {
        RespawnParticle();
        _controller.RespawnVehicle();
    }

    void RespawnParticle()
    {
        if (!m_despawnPoofParticle.scene.isLoaded) // is prefab
        {
            m_despawnPoofParticle = Instantiate(m_despawnPoofParticle);
            m_despawnPoofParticle.transform.position = transform.position;

            GameObject go = GameObject.Find("----Particles");
            if (go) m_particleParent = go.transform;
        }
        else
        {
            m_despawnPoofParticle.transform.SetParent(m_particleParent);
            m_despawnPoofParticle.SetActive(true);
        }

        // Plays despawn particle
        m_despawnPoofParticle.GetComponent<ParticleSystem>().Play();

        // Resets despawn particle
        TimerManager.RunAfterTime(() =>
        {
            m_despawnPoofParticle.transform.SetParent(transform);
            m_despawnPoofParticle.transform.position = transform.position;
        }, 3);
    }

    private void WanderAllOver()
    {
        //Debug.Log("Wandering");

        float RD = agent.remainingDistance;
        float SD = agent.stoppingDistance;

        if (RD <= SD)
        {
            
            Vector3 point;
            if (FindRandomPoint(wanderTransform.position, searchRange0, out point)) //pass in centrepoint and radius of area
            {
                agent.SetDestination(point);
                thisTransform.LookAt(point);
                thisTransform.Rotate(point);
                thisTransform.forward = point;
            }
        }
    }

    bool FindRandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
    private void Chase()
    {
        if (_playerTransform)
        {
            //Debug.Log("Chasing");

            float distance = Vector3.Distance(_playerTransform.position, agent.transform.position);
            if (distance < chasingRange0)
            {
                Vector3 lookAtDonkey = new Vector3(_playerTransform.position.x, agent.transform.position.y, _playerTransform.position.z);
                agent.SetDestination(_playerTransform.position);

                thisTransform.LookAt(lookAtDonkey);
            }
        }
    }
    private void InRange()
    {
        if (_playerTransform)
        {
            float distance = Vector3.Distance(_playerTransform.position, transform.position);
            if (distance < chasingRange0)
            {
                Chase();
                inPursuit = true;
            }
            if (distance > chasingRange0)
            {
                WanderAllOver();
                inPursuit = false;
            }
        }
    }


}
