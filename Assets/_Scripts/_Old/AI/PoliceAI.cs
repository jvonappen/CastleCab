using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using URNTS;

public class PoliceAI : MonoBehaviour
{
#pragma warning disable CS0414
    [SerializeField] private Transform m_target;

    [Tooltip("Amount the stars required for enemy targetting")]
    [SerializeField] private int m_dishonourThreshold; 

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
    [ConditionalHide("m_showDebug")] [SerializeField] private bool sirenToggle;

    [SerializeField] GameObject m_despawnPoofParticle;

    bool m_isAggressive;

    #region Init
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        thisTransform = agent.transform;
        
    }
    private void Start() => m_manager = GameManager.Instance;
    #endregion

    private void Update()
    {
        SetTarget();
        
        if (agent.enabled)
        {
            agent.speed = data.chaseSpeed1;
            Chase();
        }
    }

    void SetTarget()
    {
        #region Optimisations
        // Checks to optimise performance (The rest of the function doesn't have to run otherwise)
        if (m_playersInRange.Count == 0)
        {
            if (m_isAggressive) SetAggressive(false);
            m_target = null;
            return;
        }
        if (m_playersInRange.Count == 1 && m_playersInRange[0] == m_target) return;
        #endregion

        #region FindTarget
        float closestTargetDist = float.MaxValue;
        Transform newTarget = null;

        // Only loops through players in range to avoid looping through other players at the other end of the map for optimisaiton
        for (int i = 0; i < m_playersInRange.Count; i++)
        {
            Dishonour playerDishonour = m_playersInRange[i].GetComponentInParent<Dishonour>();

            // Checks to see if the player has reached minimum dishonour threshold for this enemy
            if (playerDishonour.currentDishonour >= m_dishonourThreshold)
            {
                // Before setting the newTarget, this checks to make sure it is the closest one so far,
                // (in case there are multiple players that meet the previous requirements in range, and targets the nearest one)
                // - Might change later on to preference higher dishonour level player, but not currently
                float targetDist = Vector3.Distance(m_playersInRange[i].position, transform.position);
                if (targetDist < closestTargetDist)
                {
                    closestTargetDist = targetDist;
                    newTarget = m_playersInRange[i];
                }
            }
        }
        #endregion

        #region SetTarget
        // If a target is found
        if (newTarget)
        {
            m_target = newTarget;
            if (!m_isAggressive) SetAggressive(true);
        }
        else
        {
            if (m_isAggressive) SetAggressive(false);
        }
        #endregion
    }

    #region OnRange
    [SerializeField] List<Transform> m_playersInRange = new();

    public void OnPlayerEnterRange(Collider _collider)
    {
        if (_collider.isTrigger) return;

        Rigidbody rb = _collider.attachedRigidbody;
        if (rb && rb.transform.tag == "Player")
        {
            if (!m_playersInRange.Contains(rb.transform))
            {
                m_playersInRange.Add(rb.transform);
            }
        }
    }

    public void OnPlayerExitRange(Collider _collider)
    {
        if (_collider.isTrigger) return;

        Rigidbody rb = _collider.attachedRigidbody;
        if (rb && rb.transform.tag == "Player")
        {
            if (m_playersInRange.Contains(rb.transform))
            {
                m_playersInRange.Remove(rb.transform);
            }
        }
    }
    #endregion

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

    //private void WanderAllOver()
    //{
    //    float RD = agent.remainingDistance;
    //    float SD = agent.stoppingDistance;
    //
    //    if (RD <= SD)
    //    {
    //        
    //        Vector3 point;
    //        if (FindRandomPoint(wanderTransform.position, data.searchRange1, out point)) //pass in centrepoint and radius of area
    //        {
    //            agent.SetDestination(point);
    //            thisTransform.LookAt(point);
    //            thisTransform.Rotate(point);
    //            thisTransform.forward = point;
    //        }
    //    }
    //}

    //bool FindRandomPoint(Vector3 center, float range, out Vector3 result)
    //{
    //    Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
    //    NavMeshHit hit;
    //    if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas))
    //    {
    //        result = hit.position;
    //        return true;
    //    }
    //
    //    result = Vector3.zero;
    //    return false;
    //}
    private void Chase()
    {
        //if (m_target)
        //{
        //    float distance = Vector3.Distance(m_target.position, agent.transform.position);
        //    if (distance < chasingRange0)
        //    {
        //        Vector3 lookAtDonkey = new Vector3(m_target.position.x, agent.transform.position.y, m_target.position.z);
        //        agent.SetDestination(m_target.position);
        //
        //        thisTransform.LookAt(lookAtDonkey);
        //    }
        //}

        Vector3 lookAtDonkey = new Vector3(m_target.position.x, agent.transform.position.y, m_target.position.z);
        agent.SetDestination(m_target.position);

        thisTransform.LookAt(lookAtDonkey);
    }


}
