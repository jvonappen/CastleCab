
using System.Collections.Generic;
using UnityEngine;

namespace URNTS
{
    public class PathFinder : MonoBehaviour
    {
        public static Transform nodeContainer;
        public static LayerMask trafficNodeLayerMask;
        public Transform roadNodeContainer;
        public LayerMask m_trafficNodeLayerMask;
        public Transform startPos;
        public Transform endPos;
        public RoadNode startNode;
        public RoadNode endNode;
        public List<RoadNode> path;

        private void Start()
        {
            nodeContainer = roadNodeContainer;
            trafficNodeLayerMask = m_trafficNodeLayerMask;
            //(path, dist) = FindPath(startNode, endNode);
        }

        public static TrafficNode GetNearestTrafficNode(Vector3 from, float searchRadius = 100)
        {
            Collider[] cols = Physics.OverlapSphere(from, searchRadius, trafficNodeLayerMask);
            Debug.Log(cols.Length);
            if (cols.Length < 1) return null;
            float nearestDist = (cols[0].transform.position - from).sqrMagnitude;
            Transform nearest = cols[0].transform;
            for (int i = 0; i < cols.Length; i++)
            {
                if ((cols[i].transform.position - from).sqrMagnitude < nearestDist)
                {
                    nearest = cols[i].transform;
                    nearestDist = (cols[i].transform.position - from).sqrMagnitude;
                }
            }
            return nearest.GetComponent<TrafficNode>();
        }

        public static (List<RoadNode>, int) FindPath(Transform startTransform, Transform endTransform)
        {
            TrafficNode startNode = GetNearestTrafficNode(startTransform.position);
            TrafficNode endNode = GetNearestTrafficNode(endTransform.position);
            if (startNode == null) { Debug.Log("Not Found"); return (null, 0); }
            RoadNode startRoadNode = startNode.IsOnRight(startTransform.position) ? startNode.transform.parent.GetComponent<RoadConnection>().node2 : startNode.transform.parent.GetComponent<RoadConnection>().node1;
            RoadNode endRoadNode = endNode.IsOnRight(endTransform.position) ? endNode.transform.parent.GetComponent<RoadConnection>().node1 : endNode.transform.parent.GetComponent<RoadConnection>().node2;
            return FindPath(startRoadNode, endRoadNode);
        }

        public static (List<RoadNode>, int) FindPath(RoadNode startNode, RoadNode endNode)
        {
            if (true)
            {
                if (startNode == null || endNode == null) return (null, 0);
                List<RoadNode> openListTemp = new List<RoadNode>();
                List<RoadNode> closedListTemp = new List<RoadNode>();
                Initialize(openListTemp, closedListTemp, startNode, endNode);
                while (openListTemp.Count > 0)
                {
                    RoadNode currentNode = GetLowestFCostNode(openListTemp);
                    if (currentNode == endNode)
                    {
                        //reached :)
                        float distance = endNode.gCost;
                        //Debug.Log(distance + " km");
                        return (CalculatePath(endNode), (int)distance);
                    }
                    openListTemp.Remove(currentNode);
                    closedListTemp.Add(currentNode);

                    List<RoadNode> branches = currentNode.GetConnectedRoadNodes();
                    for (int i = 0; i < branches.Count; i++)
                    {
                        RoadNode branch = branches[i];
                        if (closedListTemp.Contains(branch) || branch == null) continue;

                        // if not traversible --> add branch to closed list, continue

                        //
                        //int multiplier = (branch.nodeType == Node.NodeType.divert) ? 120 : 100;
                        float newGCost = currentNode.gCost + CalculateDistance(currentNode, branch);
                        if (newGCost < branch.gCost)
                        {
                            branch.cameFrom = currentNode;
                            branch.gCost = newGCost;
                            branch.hCost = 100 * CalculateDistance(branch, endNode);///30;
                            branch.fCost = CalculateFCost(branch);

                            if (!openListTemp.Contains(branch))
                            {
                                openListTemp.Add(branch);
                            }
                        }
                    }
                }
                //could not find path
                return (null, 0);
            }
        }

        public (List<RoadNode>, int) FindPathNonStatic(RoadNode startNode, RoadNode endNode)
        {
            if (true)
            {
                if (startNode == null || endNode == null) return (null, 0);
                List<RoadNode> openListTemp = new List<RoadNode>();
                List<RoadNode> closedListTemp = new List<RoadNode>();
                InitializeNonStatic(openListTemp, closedListTemp, startNode, endNode);
                while (openListTemp.Count > 0)
                {
                    RoadNode currentNode = GetLowestFCostNode(openListTemp);
                    if (currentNode == endNode)
                    {
                        //reached :)
                        float distance = endNode.gCost;
                        //Debug.Log(distance + " km");
                        return (CalculatePath(endNode), (int)distance);
                    }
                    openListTemp.Remove(currentNode);
                    closedListTemp.Add(currentNode);

                    List<RoadNode> branches = currentNode.GetConnectedRoadNodes();
                    for (int i = 0; i < branches.Count; i++)
                    {
                        RoadNode branch = branches[i];
                        if (closedListTemp.Contains(branch) || branch == null) continue;

                        // if not traversible --> add branch to closed list, continue

                        //
                        //int multiplier = (branch.nodeType == Node.NodeType.divert) ? 120 : 100;
                        float newGCost = currentNode.gCost + CalculateDistance(currentNode, branch);
                        if (newGCost < branch.gCost)
                        {
                            branch.cameFrom = currentNode;
                            branch.gCost = newGCost;
                            branch.hCost = 100 * CalculateDistance(branch, endNode);///30;
                            branch.fCost = CalculateFCost(branch);

                            if (!openListTemp.Contains(branch))
                            {
                                openListTemp.Add(branch);
                            }
                        }
                    }
                }
                //could not find path
                return (null, 0);
            }
        }

        private static List<RoadNode> CalculatePath(RoadNode endNode)
        {
            List<RoadNode> path = new List<RoadNode>();
            path.Add(endNode);
            RoadNode n = endNode;
            while (n.cameFrom != null)
            {
                path.Add(n.cameFrom);
                n = n.cameFrom;
            }
            path.Reverse();
            return path;
        }

        private static RoadNode GetLowestFCostNode(List<RoadNode> openList)
        {
            RoadNode node = openList[0];
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].fCost < node.fCost)
                {
                    node = openList[i];
                }
            }
            return node;
        }

        public static void Initialize(List<RoadNode> openList, List<RoadNode> closedList, RoadNode startNode, RoadNode endNode)
        {
            openList.Clear();
            openList.Add(startNode);
            closedList.Clear();
            foreach (RoadNode node in nodeContainer.GetComponentsInChildren<RoadNode>())
            {
                node.gCost = int.MaxValue;
                node.cameFrom = null;
            }

            startNode.gCost = 0;
            startNode.hCost = 100 * CalculateDistance(startNode, endNode);///30;
            startNode.fCost = CalculateFCost(startNode);
        }

        public void InitializeNonStatic(List<RoadNode> openList, List<RoadNode> closedList, RoadNode startNode, RoadNode endNode)
        {
            openList.Clear();
            openList.Add(startNode);
            closedList.Clear();
            foreach (RoadNode node in roadNodeContainer.GetComponentsInChildren<RoadNode>())
            {
                node.gCost = int.MaxValue;
                node.cameFrom = null;
            }

            startNode.gCost = 0;
            startNode.hCost = 100 * CalculateDistance(startNode, endNode);///30;
            startNode.fCost = CalculateFCost(startNode);
        }

        public static float CalculateDistance(RoadNode from, RoadNode to)
        {
            float distSqr = (from.transform.position.x - to.transform.position.x) * (from.transform.position.x - to.transform.position.x) +
            (from.transform.position.z - to.transform.position.z) * (from.transform.position.z - to.transform.position.z);
            return Mathf.Sqrt(distSqr);
        }

        public static float CalculateFCost(RoadNode n)
        {
            return n.gCost + n.hCost;
        }
    }
}