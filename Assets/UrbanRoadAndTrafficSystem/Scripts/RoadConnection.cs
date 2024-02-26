using System;
using System.Collections.Generic;
using UnityEngine;

namespace URNTS
{
    public class RoadConnection : MonoBehaviour
    {
        public RoadNode node1;
        public RoadNode node2;
        [HideInInspector]
        public int node1Index;
        [HideInInspector]
        public int node2Index;

        public enum SpeedTier { slow, medium, fast };
        //public SpeedTier speedTier = SpeedTier.medium;
        [Range(2, 6), Tooltip("number of lanes in the roadway (must be even)")]
        public byte lanes = 4;
        [Range(0, 2), Tooltip("scales the road width based on this factor")]
        public float widthMultiplier = 1;
        //[Tooltip("Width of divider running between the opposite lanes")]
        //public float dividerWidth = 0f;
        public int roadMaterialIndex = 1;
        public int crossingMaterialIndex = 2;

        [Header("Footpath Settings")]
        public int footPathMatIndex = 3;
        public int footPathEdgeMatIndex = 4;
        public float scaleFootpathUV = 1;
        public float leftFootpathWidthMultiplier = 1f;
        public float rightFootpathWidthMultiplier = 1f;

        [HideInInspector]
        public List<Vector3[]> connectorVerts = new List<Vector3[]>();
        [HideInInspector]
        public Vector3[,] leftFPConnectorVerts = new Vector3[2, 3];
        [HideInInspector]
        public Vector3[,] rightFPConnectorVerts = new Vector3[2, 3];
        [SerializeField]
        public List<TrafficNode> trafficNodes = new List<TrafficNode>();
        float length;

        [Header("Road Curves")]
        [Range(0,3)]
        public int controlPoints = 0;
        public int subDivisions = 0;
        public Vector3[] CPPositions;

        [HideInInspector]
        public Vector3[] CPVectors; // control point vectors
        [HideInInspector]
        public int prevCPCount = 0;
        [HideInInspector]
        public Vector3[] newCPpos;

        [HideInInspector]
        public Vector3 startPoint;
        [HideInInspector]
        public Vector3 endPoint;


        public bool CheckEquality(RoadConnection other)
        {
            bool result = false;
            if (other.node1 == node1 && other.node2 == node2) result = true;
            else if (other.node1 == node2 && other.node2 == node1) result = true;
            return result;
        }

        public Vector3 GetOutDirection(RoadNode node)
        {
            return (node1.transform.position - node2.transform.position).normalized * (node == node1 ? -1 : 1);
        }

        public float GetRoadWidth()
        {
            return widthMultiplier * TrafficManager.laneWidth * lanes;
        }

        public void EvaluateTrafficNodes()
        {
            if (trafficNodes != null)
            {
                for (int i = 0; i < trafficNodes.Count; i++)
                {
                    if (trafficNodes[i] != null)
                        DestroyImmediate(trafficNodes[i].gameObject);
                }
            }
            trafficNodes.Clear();
            // Evaluate Connection Length
            length = (startPoint - endPoint).magnitude;
            int segments = (int)length / 10;
            for (int i = 1; i < segments; i++)
            {
                GameObject nodeObject = new GameObject("TrafficNode " + transform.childCount, typeof(TrafficNode));
                TrafficNode node = nodeObject.GetComponent<TrafficNode>();
                node.Initialize(Vector3.Lerp(startPoint, endPoint, i / (float)segments), Vector3.Cross(GetOutDirection(node2), Vector3.up).normalized * GetRoadWidth() * 0.5f);
                nodeObject.transform.parent = transform;
                SphereCollider col = nodeObject.AddComponent<SphereCollider>();
                col.isTrigger = true;
                nodeObject.layer = LayerMask.NameToLayer("Traffic Nodes");
                node.maxLanes = lanes;
                node.nodeInd = i - 1;
                trafficNodes.Add(node);
            }
        }
    }
}