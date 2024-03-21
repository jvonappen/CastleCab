using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace URNTS
{
    public class TrafficManager : MonoBehaviour
    {
        public static TrafficManager instance;

        public static Action<bool> SignalAction;
        public List<Transform> players;
        List<Transform> playersToAddEndOfFrame = new();
        List<Transform> playersToRemoveEndOfFrame = new();
        public bool spawnVehicles = true;
        [Range(20, 80), Tooltip("Min distance to the player above which vehicles will be spawned")]
        public int spawnStartRadius = 40;
        [Range(20, 300), Tooltip("Max distance to the player below which vehicles will be spawned")]
        public int spawnEndRadius = 80;

        public LayerMask trafficNodeLayer;
        public LayerMask vehicleLayer;

        [Tooltip("Signal duartion of each road in seconds")]
        public bool SignalEnabled = true;
        public int signalDuration = 15;
        public int coolDownTime = 5;

        [Header("Vehicle Pool")]
        public List<VehiclePoolEntry> vehiclePool;

        [HideInInspector, SerializeField]
        public static float laneWidth = 6f;
        [HideInInspector, SerializeField]
        public static float crossingWidth = 6f;

        private void Awake()
        {
            if (instance != null) Destroy(gameObject);
            else
                instance = this;
                
            
        }

        private void Start()
        {
            GetComponent<MeshCollider>().enabled = false;

            StartCoroutine(SignalRoutine());

            InitializeVehiclePool();

            StartCoroutine(VehicleSpawner());
        }

        public void AddPlayer(Transform _player) => playersToAddEndOfFrame.Add(_player);
        public void RemovePlayer(Transform _player) => playersToRemoveEndOfFrame.Add(_player);

        void InitializeVehiclePool()
        {
            //Initialize Vehicle Pool
            List<GameObject> vehs = new List<GameObject>();
            for (int i = 0; i < vehiclePool.Count; i++)
            {
                if (vehiclePool[i].vehiclePrefab != null)
                {
                    for (int j = 0; j < vehiclePool[i].amount; j++)
                    {
                        vehs.Add(vehiclePool[i].vehiclePrefab);
                    }
                }
            }
            ObjectPooler.instance.InitializeVehPool("Vehicles", vehs.ToArray());
        }

        void Update()
        {
            
                
        }

        public IEnumerator SignalRoutine()
        {
            while (SignalEnabled)
            {
                SignalAction(false);
                yield return new WaitForSeconds(signalDuration);
                SignalAction(true);
                yield return new WaitForSeconds(coolDownTime);
            }
        }


        public IEnumerator VehicleSpawner()
        {
            while (spawnVehicles)
            {
                if (players.Count > 0)
                {
                    foreach (Transform player in players)
                    {
                        Collider[] cols = Physics.OverlapSphere(player.position, spawnEndRadius, trafficNodeLayer);
                        for (int i = 0; i < cols.Length; i++)
                        {
                            if ((player.position - cols[i].transform.position).sqrMagnitude > spawnStartRadius * spawnStartRadius)
                            {
                                TrafficNode node = cols[i].GetComponent<TrafficNode>();
                                bool inv = UnityEngine.Random.value > 0.5f;
                                byte spawnLane = (byte)UnityEngine.Random.Range(1, node.maxLanes * 0.5f + 0.99f);
                                Vector3 targetPos = node.GetNodePos(spawnLane, node.maxLanes, inv);
                                yield return new WaitForFixedUpdate();
                                Collider[] vehs = Physics.OverlapSphere(targetPos, 8, vehicleLayer);
                                if (vehs.Length == 0)
                                {
                                    GameObject Veh = ObjectPooler.instance.SpawnVehFromPool("Vehicles", targetPos + Vector3.up * 0.5f, Quaternion.LookRotation((inv ? -1 : 1) * Vector3.Cross(node.right, Vector3.up)));
                                    if (Veh != null)
                                    {
                                        VehicleController controller = Veh.GetComponentInParent<VehicleController>();
                                        controller.currentConnection = node.transform.parent.GetComponent<RoadConnection>();
                                        controller.targetNodeInd = node.nodeInd;
                                        controller.lane = spawnLane;
                                        controller.invConn = inv;
                                        controller.targetPos = targetPos;
                                    }
                                }

                            }
                            yield return new WaitForFixedUpdate();
                        }
                        yield return new WaitForFixedUpdate();
                    }

                    AddNewPlayers();
                }
                else
                {
                    AddNewPlayers();
                    yield return new WaitForFixedUpdate();
                }
            }
        }

        void AddNewPlayers()
        {
            if (playersToAddEndOfFrame.Count > 0)
            {
                foreach (Transform player in playersToAddEndOfFrame) players.Add(player);
                playersToAddEndOfFrame.Clear();
            }

            if (playersToRemoveEndOfFrame.Count > 0)
            {
                foreach (Transform player in playersToRemoveEndOfFrame) players.Remove(player);
                playersToRemoveEndOfFrame.Clear();
            }
        }
    }
}