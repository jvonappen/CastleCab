using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace URNTS
{
    [CustomEditor(typeof(PathFinder))]
    public class PathFinderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Find Route"))
            {
                PathFinder pathFinder = (PathFinder)target;
                float dist = 0;
                (pathFinder.path, dist) = PathFinder.FindPath(pathFinder.startPos, pathFinder.endPos);//pathFinder.FindPathNonStatic(pathFinder.startNode, pathFinder.endNode);
            }
        }

        private void OnValidate()
        {
            PathFinder pathFinder = (PathFinder)target;
            PathFinder.trafficNodeLayerMask = pathFinder.m_trafficNodeLayerMask;
            PathFinder.nodeContainer = pathFinder.roadNodeContainer;
        }

        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        public static void OnDrawSceneGizmo(PathFinder pathFinder, GizmoType gizmoType)
        {
            Gizmos.color = Color.yellow;
            if (pathFinder.path != null)
            {
                bool inv = false;
                RoadConnection con;
                for (int i = 0; i < pathFinder.path.Count - 1; i++)
                {
                    // for each road node
                    (con, inv) = pathFinder.path[i].GetConnection(pathFinder.path[i + 1]);
                    Gizmos.DrawLine(pathFinder.path[i].transform.position, con.trafficNodes[!inv ? 0 : con.trafficNodes.Count - 1].GetNodePos(1, con.lanes, inv));
                    if (!inv)
                    {
                        for (int j = 0; j < con.trafficNodes.Count - 1; j++)
                        {
                            // for each traffic node
                            Gizmos.DrawLine(con.trafficNodes[j].GetNodePos(1, con.lanes, inv), con.trafficNodes[j + 1].GetNodePos(1, con.lanes, inv));
                        }
                    }
                    else
                    {
                        for (int j = con.trafficNodes.Count - 1; j > 0; j--)
                        {
                            // for each traffic node
                            Gizmos.DrawLine(con.trafficNodes[j].GetNodePos(1, con.lanes, inv), con.trafficNodes[j - 1].GetNodePos(1, con.lanes, inv));
                        }
                    }
                    Gizmos.DrawLine(con.trafficNodes[inv ? 0 : con.trafficNodes.Count - 1].GetNodePos(1, con.lanes, inv), pathFinder.path[i + 1].transform.position);

                    //Gizmos.DrawLine(pathFinder.path[i].transform.position, pathFinder.path[i + 1].transform.position);
                }
            }
            Gizmos.color = Color.cyan;
            if (pathFinder.startPos != null)
            {
                Gizmos.DrawSphere(pathFinder.startPos.position, 2f);
            }
            if (pathFinder.endPos != null)
            {
                Gizmos.DrawSphere(pathFinder.endPos.position, 2f);
            }
        }

        void OnSceneGUI()
        {
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.MouseDown:
                    {
                        //RoadNode point = Selection.activeGameObject.GetComponent<RoadNode>();
                        if (e.alt)//Event.current.keyCode == RoadTrafficManager.addNode)
                        {
                            if (Selection.activeGameObject != null)
                            {
                                RoadNodeEditor.AddRoadNodePoint();
                            }
                        }

                    }
                    break;
            }
        }
    }
}