
using UnityEngine;
using UnityEditor;

namespace URNTS
{
    [CustomEditor(typeof(RoadNode))]
    public class RoadNodeEditor : Editor
    {
        public static float verticalOffset = 0.1f;

        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        public static void OnDrawSceneGizmo(RoadNode node, GizmoType gizmoType)
        {
            float size = Mathf.Clamp((Camera.current.transform.position - node.transform.position).magnitude * 0.03f, 0.4f, 4);//0.4f;
                                                                                                                               //float size = Mathf.Clamp(Camera.current.orthographicSize/10, 0.4f, 5f);
            Color nodeColor = Color.blue;
            if ((gizmoType & GizmoType.Selected) != 0)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < node.patchVertices.Count; i++)
                {
                    Gizmos.DrawLine(node.patchVertices[i], i == node.patchVertices.Count - 1 ? node.patchVertices[0] : node.patchVertices[i + 1]);
                }
                Gizmos.color = nodeColor;
            }
            else
            {
                Gizmos.color = nodeColor * 0.5f;
            }
            Gizmos.DrawSphere(node.transform.position, size);

            if (node == RoadTrafficManager.selectedRoadNode)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(node.transform.position, size * 1.2f);
                Vector3 pos = RoadTrafficManager.GetMouseWorldPos();
                Gizmos.DrawLine(node.transform.position, pos != Vector3.zero ? RoadTrafficManager.GetMouseWorldPos() : node.transform.position);
            }
            for (int i = 0; i < node.junctionPivots.Count; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(node.junctionPivots[i], 0.4f);
            }
        }

        //private void OnDestroy()
        //{
        //    RoadNode node = (RoadNode)target;
        //    for (int i = 0; i < node.connections.Count; i++)
        //    {
        //        DestroyImmediate(node.connections[i].gameObject);
        //    }
        //}

        void OnSceneGUI()
        {
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.MouseDown:
                    {
                        RoadNode point = Selection.activeGameObject.GetComponent<RoadNode>();
                        if (e.alt)//Event.current.keyCode == RoadTrafficManager.addNode)
                        {
                            if (Selection.activeGameObject != null)
                            {
                                if (point != null)
                                {
                                    //AddNodePoint(point, sim.NodeContainer);
                                    AddRoadNodePoint();
                                }
                            }
                        }

                    }
                    break;
                case EventType.KeyDown:
                    {
                        RoadNode point = Selection.activeGameObject.GetComponent<RoadNode>();
                        if (Event.current.keyCode == RoadTrafficManager.connectNode)
                        {
                            if (Selection.activeGameObject != null)
                            {
                                if (point != null)
                                {
                                    if (RoadTrafficManager.selectedRoadNode == null)
                                    {
                                        RoadTrafficManager.selectedRoadNode = point;
                                    }
                                    else if (RoadTrafficManager.selectedRoadNode != point)
                                    {
                                        //selectedNode.branches.Add(point);
                                        // ADD CONNECTION
                                        AddConnection(RoadTrafficManager.selectedRoadNode, point);
                                        RoadTrafficManager.selectedRoadNode = null;
                                    }
                                    else
                                    {
                                        RoadTrafficManager.selectedRoadNode = null;
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
        }

        public static RoadNode AddRoadNodePoint()
        {
            Vector3 pos = RoadTrafficManager.GetMouseWorldPos();
            if (pos == Vector3.zero) return null;
            GameObject nodeObject = new GameObject("RoadNode " + RoadTrafficManager.roadNodeContainer.childCount, typeof(RoadNode));

            //Set Transform
            nodeObject.transform.position = pos + Vector3.up * verticalOffset;
            nodeObject.transform.SetParent(RoadTrafficManager.roadNodeContainer);
            Selection.activeGameObject = nodeObject;

            return nodeObject.GetComponent<RoadNode>();
        }

        public RoadConnection AddConnection(RoadNode node1, RoadNode node2)
        {
            GameObject connectObject = new GameObject("RoadConnection " + RoadTrafficManager.roadConnectionContainer.childCount, typeof(RoadConnection));
            connectObject.transform.position = (node1.transform.position + node2.transform.position) * 0.5f;
            connectObject.transform.parent = RoadTrafficManager.roadConnectionContainer;
            RoadConnection connection = connectObject.GetComponent<RoadConnection>();
            connection.node1 = node1;
            connection.node2 = node2;
            return connection;
        }
    }
}