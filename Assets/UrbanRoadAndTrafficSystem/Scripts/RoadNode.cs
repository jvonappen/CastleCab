using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace URNTS
{
    [SelectionBase]
    public class RoadNode : MonoBehaviour
    {
        public List<RoadConnection> connections = new List<RoadConnection>();
        [HideInInspector]
        public List<Vector3> junctionPivots = new List<Vector3>();
        [HideInInspector]
        public List<Vector3> patchVertices = new List<Vector3>();
        [HideInInspector]
        public List<Vector3> intersectVerts;
        [HideInInspector]
        public List<Vector3> crossingVerts;
        [HideInInspector]
        public List<TrafficSignal> trafficSignals= new List<TrafficSignal>();

        [Header("Road Settings")]
        public int materialIndex = 0;
        public float scaleRoadUV = 1;
        public float crossingWidthMultiplier = 1f;

        [Header("Footpath Settings")]
        public int footPathEdgeMatIndex = 4;
        public int footPathIntersectMatIndex = 5;
        public float scaleFootpathUV = 1;

        [SerializeField, HideInInspector]
        public Route[] connectivityTable;

        [HideInInspector]
        public RoadNode cameFrom = null;
        [HideInInspector]
        public float gCost;
        [HideInInspector]
        public float hCost;
        [HideInInspector]
        public float fCost;


        int signalIndex = 0;
        private void Awake()
        {
            TrafficManager.SignalAction += ChangeSignal;
        }
        private void OnDestroy()
        {
            TrafficManager.SignalAction -= ChangeSignal;
        }
        public void ChangeSignal(bool coolDown)
        {
            int n = connections.Count;
            if (n <= 2) return;
            if (coolDown)
            {
                for (int i = 0; i < connectivityTable.Length; i++)
                {
                    Route route = connectivityTable[i];
                    if (i >= n * signalIndex && i < n * signalIndex + n)
                    {
                        if (i % (n + 1) != 0)
                        {
                            route.route[0].signal = false;
                        }
                    }
                    else if (i % (n + 1) != 0)
                    {
                        route.route[0].signal = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < connectivityTable.Length; i++)
                {
                    Route route = connectivityTable[i];
                    if (i >= n * signalIndex && i < n * signalIndex + n)
                    {
                        if (i % (n + 1) != 0)
                        {
                            route.route[0].signal = true;
                        }
                    }
                    else if (i % (n + 1) != 0)
                    {
                        route.route[0].signal = false;
                    }
                }
                signalIndex = (signalIndex + 1) % n;
            }
            for (int i = 0; i < connections.Count; i++)
            {
                TrafficSignal.SignalType sig = TrafficSignal.SignalType.red;
                if (coolDown && (i == (signalIndex + n) % n || i == (signalIndex + n -1) % n))
                {
                    sig = TrafficSignal.SignalType.yellow;
                }
                else
                {
                    sig = i == (signalIndex + n - 1) % n ? TrafficSignal.SignalType.green : TrafficSignal.SignalType.red;
                }
                trafficSignals[i].OnSignalChange(sig);
                //trafficSignals[i].OnSignalChange(coolDown ? TrafficSignal.SignalType.yellow : i == (signalIndex + n - 1) % n ? TrafficSignal.SignalType.green : TrafficSignal.SignalType.red);
            }
        }

        public List<RoadNode> GetConnectedRoadNodes()
        {
            List<RoadNode> nodes = new List<RoadNode>();
            for (int i = 0; i < connections.Count; i++)
            {
                nodes.Add(connections[i].node1 == this ? connections[i].node2 : connections[i].node1);
            }
            return nodes;
        }

        public Vector3 GetJunctionPivot(RoadConnection connection)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i] == connection)
                {
                    return junctionPivots[i];
                }
            }
            return Vector3.zero;
        }

        public List<Vector3> GetPatchVertices()
        {
            List<Vector3> patchVerts = new List<Vector3>();
            List<Vector3> interVerts = new List<Vector3>();
            junctionPivots.Clear();
            // start with cyclic sorted connections
            for (int i = 0; i < connections.Count; i++)
            {
                Vector3 inDir1 = connections[i].GetOutDirection(this);
                Vector3 p1 = transform.position + Vector3.Cross(inDir1, Vector3.up).normalized * connections[i].GetRoadWidth() * 0.5f;
                RoadConnection nextConnection = i == connections.Count - 1 ? connections[0] : connections[i + 1];
                Vector3 inDir2 = nextConnection.GetOutDirection(this);
                Vector3 p2 = transform.position - Vector3.Cross(inDir2, Vector3.up).normalized * nextConnection.GetRoadWidth() * 0.5f;
                interVerts.Add(LineLineIntersection(p1, inDir1, p2, inDir2));
            }
            intersectVerts = interVerts;
            if (crossingVerts != null) crossingVerts.Clear();
            crossingVerts = new List<Vector3>();
            for (int i = 0; i < connections.Count; i++)
            {
                Vector3 vert2;
                Vector3 point;
                if (i == 0) vert2 = interVerts[connections.Count - 1];
                else vert2 = interVerts[i - 1];
                if (Vector3.Dot(interVerts[i] - transform.position, connections[i].GetOutDirection(this)) > Vector3.Dot(vert2 - transform.position, connections[i].GetOutDirection(this)))
                {
                    point = interVerts[i] - Vector3.Cross(connections[i].GetOutDirection(this), Vector3.up).normalized * connections[i].widthMultiplier * connections[i].lanes * TrafficManager.laneWidth;
                    connections[i].connectorVerts.Add(new Vector3[2] { point, interVerts[i] });
                    Vector3 crossingWidthVec = connections[i].GetOutDirection(this) * TrafficManager.crossingWidth * crossingWidthMultiplier;
                    crossingVerts.AddRange(new List<Vector3> { point + crossingWidthVec, interVerts[i] + crossingWidthVec });
                    junctionPivots.Add((point + interVerts[i]) * 0.5f + crossingWidthVec);
                    if (this == connections[i].node1)
                        connections[i].startPoint = junctionPivots[junctionPivots.Count - 1];
                    else
                        connections[i].endPoint = junctionPivots[junctionPivots.Count - 1];
                }
                else
                {
                    point = vert2 + Vector3.Cross(connections[i].GetOutDirection(this), Vector3.up).normalized * connections[i].widthMultiplier * connections[i].lanes * TrafficManager.laneWidth;
                    connections[i].connectorVerts.Add(new Vector3[2] { vert2, point });
                    Vector3 crossingWidthVec = connections[i].GetOutDirection(this) * TrafficManager.crossingWidth * crossingWidthMultiplier;
                    crossingVerts.AddRange(new List<Vector3> { vert2 + crossingWidthVec, point + crossingWidthVec });
                    junctionPivots.Add((point + vert2) * 0.5f + crossingWidthVec);
                    if (this == connections[i].node1)
                        connections[i].startPoint = junctionPivots[junctionPivots.Count - 1];
                    else
                        connections[i].endPoint = junctionPivots[junctionPivots.Count - 1];
                }

                for (int j = 0; j < crossingVerts.Count; j++)
                {
                    Vector3 vec = crossingVerts[j];
                    vec.y = transform.position.y;
                    crossingVerts[j] = vec;
                }

                patchVerts.Add(point);
                patchVerts.Add(interVerts[i]);

            }
            return patchVerts;
        }

        public void UpdateJunctionNodes()
        {
            int points = 4;
            ClearConnectivityTable();
            connectivityTable = new Route[connections.Count * connections.Count];
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].node2 == this) connections[i].node2Index = i;
                else connections[i].node1Index = i;
                for (int j = 0; j < connections.Count; j++)
                {
                    if (i == j) continue;
                    List<TrafficNode> junctionRoute = new List<TrafficNode>();
                    float t = 0;
                    while (t <= 1f)
                    {
                        Vector3 pos = BezierPoint(junctionPivots[i], transform.position, junctionPivots[j], t);
                        if (t + 0.1f < 1)
                        {
                            Vector3 pos_ = BezierPoint(junctionPivots[i], transform.position, junctionPivots[j], t + 0.1f);
                            Vector3 rightNormal = -Vector3.Cross(pos_ - pos, Vector3.up).normalized * 0.5f * Mathf.Lerp(connections[i].GetRoadWidth(), connections[j].GetRoadWidth(), t);
                            junctionRoute.Add(AddTrafficNode(pos, rightNormal));
                        }
                        else
                        {
                            Vector3 pos_ = BezierPoint(junctionPivots[i], transform.position, junctionPivots[j], t - 0.1f);
                            Vector3 rightNormal = Vector3.Cross(pos_ - pos, Vector3.up).normalized * 0.5f * Mathf.Lerp(connections[i].GetRoadWidth(), connections[j].GetRoadWidth(), t);
                            junctionRoute.Add(AddTrafficNode(pos, rightNormal));
                        }
                        t += 1f / (points - 1);
                    }
                    connectivityTable[connections.Count * i + j] = new Route(junctionRoute);
                }
            }
        }

        public void ClearConnectivityTable()
        {
            if (connectivityTable == null) return;
            for (int i = 0; i < connectivityTable.Length; i++)
            {
                if (connectivityTable[i] != null)
                {
                    if (connectivityTable[i].route != null)
                    {
                        for (int j = 0; j < connectivityTable[i].route.Count; j++)
                        {
                            if (connectivityTable[i].route[j] != null)
                            {
                                DestroyImmediate(connectivityTable[i].route[j].gameObject);
                            }
                        }
                    }
                }
            }
        }

        public TrafficNode AddTrafficNode(Vector3 pos, Vector3 rightVec, bool trigger = false)
        {
            GameObject nodeObject = new GameObject("TrafficNode " + transform.childCount, typeof(TrafficNode));
            TrafficNode node = nodeObject.GetComponent<TrafficNode>();
            node.Initialize(pos, rightVec);
            nodeObject.transform.parent = transform;
            return node;
        }

        Vector3 BezierPoint(Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            return t * t * p3 + 2 * t * (1 - t) * p2 + (1 - t) * (1 - t) * p1;
        }

        // sort connections according to directions they are facing. (Cyclic)
        public List<RoadConnection> CyclicSort(List<RoadConnection> connections)
        {
            List<float> angles = new List<float>();
            for (int i = 0; i < connections.Count; i++)
            {
                Vector3 dir = connections[i].GetOutDirection(this);
                dir = new Vector3(dir.x, 0, dir.z);
                angles.Add(Vector3.SignedAngle(Vector3.right, dir, Vector3.up));
            }
            List<RoadConnection> sortedConnections = connections.Select((value, index) => new { Value = value, Index = index }).OrderBy(item => angles[item.Index]).Select(item => item.Value).ToList();
            sortedConnections.Reverse();
            return sortedConnections;
        }

        public static Vector3 LineLineIntersection(Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
        {
            lineVec1.y = 0;
            lineVec2.y = 0;
            Vector3 intersection = Vector3.positiveInfinity;
            Vector3 lineVec3 = linePoint2 - linePoint1;
            Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
            Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

            float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

            //is coplanar, and not parallel
            if (Mathf.Abs(planarFactor) < 0.0001f
                    && crossVec1and2.sqrMagnitude > 0.0001f)
            {
                float s = Vector3.Dot(crossVec3and2, crossVec1and2)
                        / crossVec1and2.sqrMagnitude;
                intersection = linePoint1 + lineVec1 * s;
            }
            else
            {
                intersection = linePoint1;
            }
            return intersection;
        }

        public List<TrafficNode> GetRouteFromConnectivityTable(int i, int j)
        {
            return connectivityTable[connections.Count * i + j].route;
        }

        public int GetRandomConnectionIndex(int except)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < connections.Count; i++)
            {
                if (i == except) continue;
                indices.Add(i);
            }
            return indices[Random.Range(0, indices.Count)];
        }

        public int GetRandomConnectionIndex(List<int> except)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < connections.Count; i++)
            {
                if (except.Contains(i)) continue;
                indices.Add(i);
            }
            return indices[Random.Range(0, indices.Count)];
        }

        public (RoadConnection, bool) GetConnection(RoadNode node)
        {
            bool inv = false;
            int ind = 0;
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].node1 == node)
                {
                    inv = true;
                    ind = i; break;
                }
                else if (connections[i].node2 == node)
                {
                    inv = false;
                    ind = i; break;
                }
            }
            return (connections[ind], inv);
        }
    }
}