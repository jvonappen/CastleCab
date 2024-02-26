using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace URNTS
{
    [CustomEditor(typeof(RoadConnection))]
    public class RoadConnectionEditor : Editor
    {

        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        public static void OnDrawSceneGizmo(RoadConnection rc, GizmoType gizmoType)
        {
            float size = Mathf.Clamp((Camera.current.transform.position - rc.transform.position).magnitude * 0.03f, 0.4f, 3);
            Gizmos.color = Color.green;
            if (rc.node1 != null && rc.node2 != null)
            {
                Gizmos.DrawLine(rc.node1.transform.position, rc.node2.transform.position);
                rc.transform.position = (rc.node1.transform.position + rc.node2.transform.position) * 0.5f;
            }
            //Gizmos.DrawCube(rc.transform.position, Vector3.one*size*0.5f);
            Gizmos.DrawSphere(rc.transform.position, size * 0.5f);

            if ((gizmoType & GizmoType.Selected) != 0)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < rc.trafficNodes.Count; i++)
                {
                    Gizmos.DrawSphere(rc.trafficNodes[i].transform.position, 0.2f);
                    Gizmos.DrawRay(rc.trafficNodes[i].transform.position + Vector3.up * 0.2f, rc.trafficNodes[i].right);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Split Connection"))
            {
                RoadConnection connection = (RoadConnection)target;
                RoadNode node = AddRoadNodePoint(connection.transform.position);
                RoadNode prevNode2 = connection.node2;

                RoadConnection newCon = AddConnection(node, connection.node2);
                connection.node2 = node;


                for (int i = 0; i < prevNode2.connections.Count; i++)
                {
                    if (prevNode2.connections[i] == connection)
                    {
                        prevNode2.connections[i] = newCon;
                        break;
                    }
                }
                node.connections.AddRange(new List<RoadConnection> { connection, newCon });
            }
        }

        public RoadNode AddRoadNodePoint(Vector3 pos)
        {
            if (pos == Vector3.zero) return null;
            GameObject nodeObject = new GameObject("RoadNode " + RoadTrafficManager.roadNodeContainer.childCount, typeof(RoadNode));

            //Set Transform
            nodeObject.transform.position = pos;
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

        private void OnSceneGUI()
        {

            RoadConnection con = (RoadConnection)target;
            if (con.prevCPCount != con.controlPoints)
            {
                PositionCPs(con);
                con.CPPositions = con.newCPpos;
                con.prevCPCount = con.controlPoints;
            }
            if (con.CPPositions != null)
            {
                for (int i = 0; i < con.controlPoints; i++)
                {
                    con.newCPpos[i] = Handles.PositionHandle(con.CPPositions[i], Quaternion.LookRotation(con.GetOutDirection(con.node1)));
                    Handles.Label(con.CPPositions[i] + Vector3.up, i.ToString());
                    if (GUI.changed)
                    {
                        Undo.RecordObject(con, "Move Control Point");
                        con.CPPositions[i] = con.newCPpos[i];
                        EditorUtility.SetDirty(target);
                    }
                }
            }
            
        }

        void PositionCPs(RoadConnection con)
        {
            con.newCPpos = new Vector3[con.controlPoints];
            float frac = 1f/(1+con.controlPoints);
            for (int i = 0; i < con.controlPoints; i++)
            {
                con.newCPpos[i] = Vector3.Lerp(con.startPoint, con.endPoint, frac * (i + 1));
            }
        }
    }
}