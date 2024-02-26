
using UnityEngine;

namespace URNTS
{
    public class TestAI : MonoBehaviour
    {
        Rigidbody rb;
        public float Kp = 1, Ki = 1, Kd = 1;

        public int targetNodeInd = 0;
        public RoadConnection currentConnection;
        RoadConnection newCon;
        Vector3 nodePos;
        bool atJunction = false;
        TrafficNode[] junctionRoute;
        RoadNode currentJunctionNode;
        bool invConn = false;
        public byte lane = 1;
        float laneFrac;
        bool signal = true;

        bool obstacle = false;

        [Header("Vehicle Components")]
        public Transform steerPoint;
        // Start is called before the first frame update
        void Start()
        {
            //float turnRad = wheelBase/Mathf.Sin(Mathf.Deg2Rad*40);
            rb = GetComponentInChildren<Rigidbody>();
            nodePos = GetNodePos();
            steerPoint = transform;
        }

        // Update is called once per frame
        void Update()
        {
            //float u = levitateController.Output(1.5f - rb.transform.position.y, Time.deltaTime);
            //rb.AddForce(Vector3.up * u);
            UpdateTarget();

            SteerTo(nodePos, !signal);
        }

        public void FixedUpdate()
        {
            RaycastHit hit;
            if (Physics.BoxCast(steerPoint.position, Vector3.one, transform.forward, out hit, Quaternion.LookRotation(transform.forward), 3f))
            {
                //obstacle = true;
            }
            else
            {
                obstacle = false;
            }
        }

        private void UpdateTarget()
        {
            if (!signal)
            {
                if (atJunction) signal = junctionRoute[0].signal;
                else signal = true;
            }
            else if ((nodePos - rb.position).sqrMagnitude < 9f)
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
                nodePos = GetNodePos();
            }
        }

        public void SetRandomRoute(bool inv)
        {
            atJunction = true;
            targetNodeInd = 0;
            if (inv)
            {
                currentJunctionNode = currentConnection.node1;
                int newConInd = currentConnection.node1.GetRandomConnectionIndex(currentConnection.node1Index);
                junctionRoute = currentConnection.node1.GetRouteFromConnectivityTable(currentConnection.node1Index, newConInd).ToArray();
                newCon = currentJunctionNode.connections[newConInd];
            }
            else
            {
                currentJunctionNode = currentConnection.node2;
                int newConInd = currentConnection.node2.GetRandomConnectionIndex(currentConnection.node2Index);
                junctionRoute = currentConnection.node2.GetRouteFromConnectivityTable(currentConnection.node2Index, newConInd).ToArray();
                newCon = currentJunctionNode.connections[newConInd];
            }
            laneFrac = (2 * lane - 1) / (float)currentConnection.lanes;
            lane = (byte)Random.Range(1, newCon.lanes * 0.5f + 0.99f);
            signal = junctionRoute[0].signal;
        }

        private void SteerTo(Vector3 pos, bool stopAtPos = false)
        {
            if (rb.velocity.sqrMagnitude > 25) rb.rotation = Quaternion.LookRotation(pos - rb.position);
            rb.velocity = obstacle ? Vector3.zero : rb.transform.forward * (stopAtPos ? Mathf.Clamp((pos - rb.position).magnitude - 2, 0, 10) : 10f);
        }

        Vector3 GetNodePos()
        {
            Vector3 nodePos;
            if (!atJunction)
            {
                nodePos = currentConnection.trafficNodes[targetNodeInd].GetNodePos(lane, currentConnection.lanes, invConn);
            }
            else
            {
                //nodePos = junctionRoute[targetNodeInd].GetNodePos((float)lane / (float)newCon.lanes, false);

                nodePos = junctionRoute[targetNodeInd].GetNodePos(Mathf.Lerp(laneFrac, (2 * lane - 1) / (float)newCon.lanes, (float)targetNodeInd / junctionRoute.Length), false);
            }
            return nodePos;
        }
    }
}

//public class PIDController{
//    public float Kp = 1;
//    public float Ki = 1;
//    public float Kd = 1;
//    float ierr = 0;
//    float prevError = 0;
//    public PIDController(float Kp,float Ki,float Kd)
//    {
//        this.Kp = Kp;
//        this.Ki = Ki;
//        this.Kd = Kd;
//    }

//    public void Reset()
//    {
//        ierr= 0;
//        prevError = 0;
//    }

//    public float Output(float err, float deltaT)
//    {
//        float p = Kp * err;
//        ierr += err * deltaT;
//        float i = Ki * ierr;
//        float d = Kd * (err-prevError)/deltaT;
//        prevError = err;
//        return p + i + d;
//    }
//}
