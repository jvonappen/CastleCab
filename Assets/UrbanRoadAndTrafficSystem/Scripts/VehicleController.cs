
using System.Collections.Generic;
using UnityEngine;

namespace URNTS
{
    [SelectionBase]
    public class VehicleController : MonoBehaviour, IPoolObject
    {
        public WheelCollider WheelFL, WheelFR;
        public WheelCollider WheelBL, WheelBR;

        public enum Mode { player, AI_freeroam };
        public Mode mode = Mode.AI_freeroam;

        // Physical Params:
        public Vector3 COM = -0.5f * Vector3.up;
        public Transform steerPoint;
        [HideInInspector]
        public float wheelBase = 3;
        [HideInInspector]
        public float wheelTrack = 2;

        // Vehicle Specs
        [Header("Vehicle Specs")]
        public float motorForce = 1000f;
        public float maxSteerAngle = 40f;
        public float maxVelocity = 30f;
        [Range(0.1f, 2f)]
        public float acceleration = 0.25f;
        public float velocityGain = 0.4f;

        Rigidbody rb;
        float targetVel = 0;
        float currentVel = 0;

        // Navigation Parameters
        [Header("Navigation Parameters")]
        public int targetNodeInd = 0;
        public RoadConnection currentConnection;
        RoadConnection newCon;
        [HideInInspector]
        public Vector3 targetPos;
        bool atJunction = false;
        TrafficNode[] junctionRoute;
        RoadNode currentJunctionNode;
        LayerMask vehicleLayer;
        [HideInInspector]
        public bool invConn = false;
        public byte lane = 1;
        float laneFrac;
        bool signal = true;
        float obstacleDist = float.PositiveInfinity;
        bool triggerStopper = false;

        private void Start()
        {
            vehicleLayer = TrafficManager.instance.vehicleLayer;
            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = COM;
            targetPos = GetNodePos();
        }

        public virtual void OnSpawn()
        {
            atJunction = false;
            newCon = null;
            currentJunctionNode = null;
            junctionRoute = null;
            targetPos = Vector3.zero;
        }

        public virtual void Update()
        {
            if (mode == Mode.AI_freeroam)
            {
                if ((TrafficManager.instance.player.position - transform.position).sqrMagnitude > TrafficManager.instance.spawnEndRadius * TrafficManager.instance.spawnEndRadius + 1000) //instance*
                {
                    ObjectPooler.instance.StoreVehInPool("Vehicles", gameObject);
                }
                UpdateTarget();
                currentVel = Vector3.Dot(rb.velocity, transform.forward);
                ApplyMotorForce(acceleration * rb.mass * (targetVel - currentVel));
                SteerTo(targetPos, !signal);
            }
            else if (mode == Mode.player)
            {
                ApplySteer(40f * Mathf.Deg2Rad * Input.GetAxis("Horizontal"));
                targetVel = maxVelocity * Input.GetAxis("Vertical");
                currentVel = Vector3.Dot(rb.velocity, transform.forward);
                ApplyMotorForce(acceleration * rb.mass * (targetVel - currentVel));
            }
        }

        public void UpdateTarget()
        {
            if (!signal)
            {
                if (atJunction) signal = junctionRoute[0].signal;
                else signal = true;
            }
            else if ((targetPos - rb.position).sqrMagnitude < 9f)
            {
                if (atJunction)
                {
                    if (targetNodeInd == junctionRoute.Length - 1)
                    {
                        // when final traffic node of the junction route is reached
                        atJunction = false;
                        bool inv = !(newCon.node1 == currentJunctionNode);
                        invConn = inv;
                        targetNodeInd = inv ? newCon.trafficNodes.Count - 1 : 0;
                        currentConnection = newCon;
                        if (newCon.trafficNodes.Count == 0) SetRandomRoute(inv);
                    }
                    else
                    {
                        targetNodeInd += 1;
                    }
                }
                else
                {
                    if (invConn)
                    {
                        if (targetNodeInd > 0 && currentConnection.trafficNodes.Count > 0)
                            targetNodeInd -= 1;
                        else
                        {
                            // choose new target connection from node1,check orientation and set invConn
                            SetRandomRoute(invConn);
                        }
                    }
                    else
                    {
                        if (targetNodeInd < currentConnection.trafficNodes.Count - 1 && currentConnection.trafficNodes.Count > 0)
                            targetNodeInd += 1;
                        else
                        {
                            // choose new target connection from node2 ,check orientation and set invConn
                            SetRandomRoute(invConn);
                        }
                    }
                }
                targetPos = GetNodePos();
            }
        }

        public void SetRandomRoute(bool inv)
        {
            atJunction = true;
            targetNodeInd = 0;
            if (inv)
            {
                currentJunctionNode = currentConnection.node1;
                int newConInd = SelectValidConnectionIndex(currentJunctionNode, currentConnection.node1Index);
                junctionRoute = currentJunctionNode.GetRouteFromConnectivityTable(currentConnection.node1Index, newConInd).ToArray();
                newCon = currentJunctionNode.connections[newConInd];
            }
            else
            {
                currentJunctionNode = currentConnection.node2;
                int newConInd = SelectValidConnectionIndex(currentJunctionNode, currentConnection.node2Index);
                junctionRoute = currentConnection.node2.GetRouteFromConnectivityTable(currentConnection.node2Index, newConInd).ToArray();
                newCon = currentJunctionNode.connections[newConInd];
            }
            laneFrac = (2 * lane - 1) / (float)currentConnection.lanes;
            lane = triggerStopper ? (byte)1 : (byte)Random.Range(1, newCon.lanes * 0.5f + 0.99f);
            signal = junctionRoute[0].signal;
        }

        public int SelectValidConnectionIndex(RoadNode node, int nodeIndex)
        {
            // select turn based on current lane
            int newConInd = 0;
            switch (currentConnection.lanes / 2)
            {
                case 1: newConInd = node.GetRandomConnectionIndex(nodeIndex); break;

                case 2:
                    if (node.connections.Count == 4)
                    {
                        if (lane == 1) newConInd = node.GetRandomConnectionIndex(new List<int> { nodeIndex, CyclicMod(nodeIndex + 1, node.connections.Count) });
                        else newConInd = newConInd = CyclicMod(nodeIndex + 1, node.connections.Count);
                    }
                    else if (node.connections.Count > 2)
                    {
                        if (lane == 1) newConInd = node.GetRandomConnectionIndex(new List<int> { nodeIndex, CyclicMod(nodeIndex + 1, node.connections.Count) });
                        else newConInd = node.GetRandomConnectionIndex(new List<int> { nodeIndex, CyclicMod(nodeIndex - 1, node.connections.Count) });
                    }
                    else
                    {
                        newConInd = node.GetRandomConnectionIndex(nodeIndex); break;
                    }
                    break;
                case 3:
                    if (node.connections.Count > 3)
                    {
                        if (lane == 1) newConInd = node.GetRandomConnectionIndex(new List<int> { nodeIndex, CyclicMod(nodeIndex + 1, node.connections.Count), CyclicMod(nodeIndex + 2, node.connections.Count) });
                        else if (lane == 3) newConInd = node.GetRandomConnectionIndex(new List<int> { nodeIndex, CyclicMod(nodeIndex - 1, node.connections.Count), CyclicMod(nodeIndex - 2, node.connections.Count) });
                        else newConInd = node.GetRandomConnectionIndex(new List<int> { nodeIndex, CyclicMod(nodeIndex - 1, node.connections.Count), CyclicMod(nodeIndex + 1, node.connections.Count) });
                    }
                    else if (node.connections.Count == 3)
                    {
                        if (lane == 1) newConInd = node.GetRandomConnectionIndex(new List<int> { nodeIndex, CyclicMod(nodeIndex + 1, node.connections.Count) });
                        else if (lane == 3) newConInd = node.GetRandomConnectionIndex(new List<int> { nodeIndex, CyclicMod(nodeIndex - 1, node.connections.Count) });
                        else newConInd = node.GetRandomConnectionIndex(nodeIndex); break;
                    }
                    else
                    {
                        newConInd = node.GetRandomConnectionIndex(nodeIndex); break;
                    }
                    break;
            }
            return newConInd;
        }

        public int CyclicMod(int x, int m)
        {
            return (x % m + m) % m;
        }

        public void SteerTo(Vector3 pos, bool stopAtPos = false)
        {
            if (targetPos == Vector3.zero)
            {
                targetVel = 0;
                return;
            }
            Vector3 directionToTarget = transform.InverseTransformDirection(targetPos - steerPoint.position);

            // Calculate the angle between the vehicle's forward vector and the direction vector
            float angle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
            // Clamp the angle to the max steer angle
            float steerAngle = Mathf.Clamp(angle, -maxSteerAngle, maxSteerAngle);

            // Apply the steer angle to the front wheels
            WheelFL.steerAngle = steerAngle;
            WheelFR.steerAngle = steerAngle;

            float vel = Mathf.Lerp(0.8f * maxVelocity * (5f / 18f), 2f, Mathf.Abs(steerAngle / maxSteerAngle));
            float stopDist = Mathf.Min(stopAtPos ? Vector3.Dot(targetPos - steerPoint.position, transform.forward) : Mathf.Infinity, obstacleDist - 1);
            targetVel = Mathf.Clamp(velocityGain * (stopDist - 1f), -0.5f * vel, vel);
            targetVel = triggerStopper ? 0 : targetVel;
        }

        Vector3 GetNodePos()
        {
            Vector3 nodePos;
            if (!atJunction)
            {
                if (currentConnection == null) return Vector3.zero;
                nodePos = currentConnection.trafficNodes[targetNodeInd].GetNodePos(lane, currentConnection.lanes, invConn);
            }
            else
            {
                nodePos = junctionRoute[targetNodeInd].GetNodePos(Mathf.Lerp(laneFrac, (2 * lane - 1) / (float)newCon.lanes, (float)targetNodeInd / junctionRoute.Length), false);
            }
            return nodePos;
        }

        public virtual void FixedUpdate()
        {
            RaycastHit hit;
            if (Physics.BoxCast(steerPoint.position, new Vector3(1, 1, 0.1f), transform.forward, out hit, Quaternion.LookRotation(transform.forward), 10f, vehicleLayer))
            {
                obstacleDist = hit.distance;
            }
            else
            {
                obstacleDist = Mathf.Infinity;
            }

            if (Physics.Raycast(WheelFR.transform.position + Vector3.up * 0.5f, transform.right, 0.5f, vehicleLayer))
            {
                triggerStopper = true;
            }
            else
            {
                triggerStopper = false;
            }
            //Debug.DrawRay(WheelFR.transform.position, transform.right * 0.5f, Color.red);
        }

        public void ApplySteer(float steer)
        {
            float newSteerAngle = maxSteerAngle * steer;
            WheelFL.steerAngle = newSteerAngle;
            WheelFR.steerAngle = newSteerAngle;
        }

        public void ApplyMotorForce(float motorForce)
        {
            if (motorForce < 0 && currentVel > 1f)
            {
                WheelBL.brakeTorque = Mathf.Abs(0.1f * motorForce * motorForce);
                WheelBR.brakeTorque = Mathf.Abs(0.1f * motorForce * motorForce);
            }
            else
            {
                WheelBR.brakeTorque = 0;
                WheelBL.brakeTorque = 0;
            }
            WheelBL.motorTorque = motorForce;
            WheelBR.motorTorque = motorForce;
        }

    }
}