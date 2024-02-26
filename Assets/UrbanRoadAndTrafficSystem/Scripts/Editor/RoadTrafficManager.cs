
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace URNTS
{
    public class RoadTrafficManager : EditorWindow
    {
        public static Transform roadNodeContainer;
        public static Transform roadConnectionContainer;
        public static Transform propsContainer;
        public static RoadNode selectedRoadNode;
        public static float laneWidth = 6f;
        public static KeyCode addNode = KeyCode.LeftAlt;
        public static KeyCode connectNode = KeyCode.C;
        public static float crossingWidth = 6f;
        public static float footPathWidth = 6f;
        public static float sidewalkElevation = 0.2f;
        public static float footPathEdgeWidth = 0.3f;

        public MeshFilter meshFilter;
        public Transform roadTrafficManager;
        public int materialsCount = 6;
        public float m_laneWidth = 6f;
        public float m_crossingWidth = 6f;
        public float m_footPathWidth = 6f;
        public float m_sidewalkElevation = 0.2f;
        public float m_footPathEdgeWidth = 0.3f;

        public GameObject trafficSignal;
        public GameObject streetLight;


        [MenuItem("Window/GameDevStuff/Urban Roads and Traffic")]
        public static void ShowWindow()
        {
            GetWindow<RoadTrafficManager>("Urban Roads and Traffic");
        }

        private void OnEnable()
        {
            if (roadTrafficManager == null) return;
            roadNodeContainer = roadTrafficManager.GetChild(0);
            roadConnectionContainer = roadTrafficManager.GetChild(1);
            meshFilter = roadTrafficManager.GetComponent<MeshFilter>();
        }

        private void OnValidate()
        {
            UpdateFields();
        }

        public void UpdateFields()
        {
            if (roadTrafficManager == null)
            {
                TrafficManager man = FindObjectOfType<TrafficManager>();
                if (man == null)
                    return;
                roadTrafficManager = man.transform;
            }
            roadNodeContainer = roadTrafficManager.GetChild(0);
            roadConnectionContainer = roadTrafficManager.GetChild(1);
            propsContainer = roadTrafficManager.GetChild(2);
            meshFilter = roadTrafficManager.GetComponent<MeshFilter>();

            laneWidth = m_laneWidth;
            footPathWidth = m_footPathWidth;
            crossingWidth = m_crossingWidth;
            sidewalkElevation = m_sidewalkElevation;
            footPathEdgeWidth = m_footPathEdgeWidth;

            TrafficManager.laneWidth= laneWidth;
            TrafficManager.crossingWidth= crossingWidth;
        }

        [System.Obsolete]
        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            roadTrafficManager = (Transform)EditorGUILayout.ObjectField("RoadTrafficManager", roadTrafficManager, typeof(Transform));
            // End the code block and update the label if a change occurred

            //roadNodeContainer = (Transform)EditorGUILayout.ObjectField("Node Container", roadNodeContainer, typeof(Transform));
            //roadConnectionContainer = (Transform)EditorGUILayout.ObjectField("Connections Container", roadConnectionContainer, typeof(Transform));
            //meshFilter = (MeshFilter)EditorGUILayout.ObjectField("Mesh Filter", meshFilter, typeof(MeshFilter));
            EditorGUILayout.Space();
            GUILayout.Label("Customize Roads: ");
            m_laneWidth = EditorGUILayout.Slider("Lane Width", m_laneWidth, 1f, 10f);// (float)EditorGUILayout.FloatField("Lane Width", m_laneWidth);
            m_footPathWidth = EditorGUILayout.Slider("Sidewalk Width", m_footPathWidth, 1f, 10f);
            m_crossingWidth = EditorGUILayout.Slider("Crossing Width", m_crossingWidth, 1f, 10f);
            m_sidewalkElevation = EditorGUILayout.Slider("Sidewalk Elevation", m_sidewalkElevation, 0.01f, 1f);
            m_footPathEdgeWidth = EditorGUILayout.Slider("Sidewalk Edge Width", m_footPathEdgeWidth, 0.01f, 1f);

            materialsCount = EditorGUILayout.IntField("Total Materials ", materialsCount);

            EditorGUILayout.Space(); EditorGUILayout.Space();
            EditorGUILayout.LabelField("PROPS: ");
            EditorGUILayout.Space();
            trafficSignal = (GameObject) EditorGUILayout.ObjectField("Traffic Signal Prefab", trafficSignal, typeof(GameObject));
            streetLight = (GameObject)EditorGUILayout.ObjectField("Street Light Prefab", streetLight, typeof(GameObject));

            if (EditorGUI.EndChangeCheck())
            {
                UpdateFields();
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.Space();
            if (GUILayout.Button("Clear All traffic Nodes"))
            {
                if (roadTrafficManager != null)
                    ClearTrafficNodes();
                else
                    Debug.LogError("RoadTrafficManager not assigned");
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Update Roads"))
            {
                if (roadTrafficManager != null)
                {
                    UpdateNodeConnections();
                    DrawRoadMesh();
                    UpdateNodeConnections();
                }
                else
                    Debug.LogError("RoadTrafficManager not assigned");
            }
        }

        private void ClearTrafficNodes()
        {
            for (int i = 0; i < roadNodeContainer.childCount; i++)
            {
                while (roadNodeContainer.GetChild(i).childCount > 0)
                {
                    DestroyImmediate(roadNodeContainer.GetChild(i).GetChild(0).gameObject);
                }
            }
            for (int i = 0; i < roadConnectionContainer.childCount; i++)
            {
                while (roadConnectionContainer.GetChild(i).childCount > 0)
                {
                    DestroyImmediate(roadConnectionContainer.GetChild(i).GetChild(0).gameObject);
                }
            }
        }

        public static Vector3 GetMouseWorldPos()
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(mouseRay, out raycastHit, 500))
            {
                return raycastHit.point;
            }
            else
                return Vector3.zero;
        }

        public void UpdateNodeConnections()
        {
            foreach (RoadNode node in roadNodeContainer.GetComponentsInChildren<RoadNode>())
            {
                node.connections.Clear();
                node.patchVertices.Clear();
            }
            foreach (RoadConnection con in roadConnectionContainer.GetComponentsInChildren<RoadConnection>())
            {
                con.connectorVerts.Clear();
                con.node1.connections.Add(con);
                con.node2.connections.Add(con);
                con.EvaluateTrafficNodes();
            }
            foreach (RoadNode node in roadNodeContainer.GetComponentsInChildren<RoadNode>())
            {
                if (node.connections.Count > 1)
                {
                    node.connections = node.CyclicSort(node.connections);
                    node.patchVertices = node.GetPatchVertices();
                    node.UpdateJunctionNodes();
                }
            }
        }

        private void DrawRoadMesh()
        {
            int[] matStartIndices = new int[materialsCount];
            Mesh mesh = new Mesh();


            //if (meshFilter.sharedMesh != null)
            //{
            //    mesh = meshFilter.sharedMesh;
            //}
            //else
            //{
            //    mesh = new Mesh();
            //    meshFilter.sharedMesh = mesh;
            //}
            List<Vector3> meshVerts = new List<Vector3>();
            List<int>[] meshTriangles = new List<int>[materialsCount];
            int sInd = 0;
            for (int i = 0; i < materialsCount; i++)
            {
                meshTriangles[i] = new List<int>();
            }
            List<Vector2> uvList = new List<Vector2>();
            for (int i = 0; i < roadNodeContainer.childCount; i++)
            {
                RoadNode node = roadNodeContainer.GetChild(i).GetComponent<RoadNode>();
                if (node.connections.Count <= 1)
                {
                    Debug.LogError("Make sure the road network forms closed loops!");
                    continue;
                }
                meshVerts.AddRange(node.patchVertices);
                meshTriangles[node.materialIndex].AddRange(Triangulator.Triangulate(node.patchVertices.ToArray(), sInd));
                sInd += node.patchVertices.Count;
                for (int j = 0; j < node.patchVertices.Count; j++)
                {
                    uvList.Add(new Vector2(node.patchVertices[j].x, node.patchVertices[j].z) * node.scaleRoadUV);
                }

                // Generate Footpath turns
                for (int j = 0; j < node.connections.Count; j++)
                {
                    int smoothPoints = 8;

                    RoadConnection con = node.connections[j];
                    RoadConnection nextCon = node.connections[(j + 1) % node.connections.Count];
                    Vector3 outDir1 = con.GetOutDirection(node);
                    Vector3 p1 = node.transform.position + Vector3.Cross(outDir1, Vector3.up).normalized * (con.GetRoadWidth() * 0.5f + (con.node1 == node ? con.leftFootpathWidthMultiplier : con.rightFootpathWidthMultiplier) * footPathWidth) + Vector3.up * sidewalkElevation;
                    Vector3 outDir2 = nextCon.GetOutDirection(node);
                    Vector3 p2 = node.transform.position - Vector3.Cross(outDir2, Vector3.up).normalized * (nextCon.GetRoadWidth() * 0.5f + (nextCon.node1 == node ? nextCon.rightFootpathWidthMultiplier : nextCon.leftFootpathWidthMultiplier) * footPathWidth) + Vector3.up * sidewalkElevation;
                    Vector3 interVert = LineLineIntersection(p1, outDir1, p2, outDir2);
                    p1 = node.transform.position + Vector3.Cross(outDir1, Vector3.up).normalized * (con.GetRoadWidth() * 0.5f + footPathEdgeWidth) + Vector3.up * sidewalkElevation;
                    p2 = node.transform.position - Vector3.Cross(outDir2, Vector3.up).normalized * (nextCon.GetRoadWidth() * 0.5f + footPathEdgeWidth) + Vector3.up * sidewalkElevation;
                    Vector3 edgeCP = LineLineIntersection(p1, outDir1, p2, outDir2);

                    edgeCP.y = node.transform.position.y + sidewalkElevation; interVert.y = node.transform.position.y + sidewalkElevation;

                    List<Vector3> outerEdgeVerts = new List<Vector3>();
                    p1 = node.crossingVerts[2 * ((j + 1) % node.connections.Count)] + Vector3.up * sidewalkElevation;
                    p2 = node.crossingVerts[2 * j + 1] + Vector3.up * sidewalkElevation;
                    for (int k = 0; k <= smoothPoints; k++)
                    {
                        outerEdgeVerts.Add(BezierPoint(p1, node.intersectVerts[j] + Vector3.up * sidewalkElevation, p2, k / (float)smoothPoints));
                    }
                    Vector3 p3 = node.junctionPivots[j] + Vector3.Cross(outDir1, Vector3.up).normalized * (con.GetRoadWidth() * 0.5f + footPathEdgeWidth);
                    Vector3 p4 = node.junctionPivots[(j + 1) % node.junctionPivots.Count] - Vector3.Cross(outDir2, Vector3.up).normalized * (nextCon.GetRoadWidth() * 0.5f + footPathEdgeWidth);

                    p3.y = node.transform.position.y + sidewalkElevation; p4.y = node.transform.position.y + sidewalkElevation;

                    List<Vector3> innerEdgeVerts = new List<Vector3>();
                    for (int k = 0; k <= smoothPoints; k++)
                    {
                        innerEdgeVerts.Add(BezierPoint(p3, edgeCP, p4, k / (float)smoothPoints));
                    }
                    Vector3 p5, p6;
                    List<Vector3> faceVerts = new List<Vector3>();

                    if (Vector3.Dot(p2 - interVert, outDir1) > 0)
                    {
                        p5 = p2 + Vector3.Cross(outDir1, Vector3.up).normalized * (con.node1 == node ? con.leftFootpathWidthMultiplier : con.rightFootpathWidthMultiplier) * footPathWidth;
                        if (con.node1 == node)
                        {
                            con.leftFPConnectorVerts[0, 0] = p2;
                            con.leftFPConnectorVerts[0, 1] = p3;
                            con.leftFPConnectorVerts[0, 2] = p5;
                        }
                        else
                        {
                            con.rightFPConnectorVerts[1, 0] = p2;
                            con.rightFPConnectorVerts[1, 1] = p3;
                            con.rightFPConnectorVerts[1, 2] = p5;
                        }
                        faceVerts.Add(p5);
                    }
                    else
                    {
                        p5 = interVert - Vector3.Cross(outDir1, Vector3.up).normalized * (con.node1 == node ? con.leftFootpathWidthMultiplier : con.rightFootpathWidthMultiplier) * footPathWidth;
                        Vector3 p5_ = p5 + Vector3.Cross(outDir1, Vector3.up).normalized * footPathEdgeWidth;
                        if (con.node1 == node)
                        {
                            con.leftFPConnectorVerts[0, 0] = p5;
                            con.leftFPConnectorVerts[0, 1] = p5_;
                            con.leftFPConnectorVerts[0, 2] = interVert;
                        }
                        else
                        {
                            con.rightFPConnectorVerts[1, 0] = p5;
                            con.rightFPConnectorVerts[1, 1] = p5_;
                            con.rightFPConnectorVerts[1, 2] = interVert;
                        }
                        innerEdgeVerts.Insert(0, p5_);
                        outerEdgeVerts.Add(p5);
                    }
                    faceVerts.Add(interVert);
                    if (Vector3.Dot(p1 - interVert, outDir2) > 0)
                    {
                        p6 = p1 - Vector3.Cross(outDir2, Vector3.up).normalized * (nextCon.node2 == node ? nextCon.leftFootpathWidthMultiplier : nextCon.rightFootpathWidthMultiplier) * footPathWidth;
                        if (nextCon.node1 == node)
                        {
                            nextCon.rightFPConnectorVerts[0, 0] = p1;
                            nextCon.rightFPConnectorVerts[0, 1] = p4;
                            nextCon.rightFPConnectorVerts[0, 2] = p6;
                        }
                        else
                        {
                            nextCon.leftFPConnectorVerts[1, 0] = p1;
                            nextCon.leftFPConnectorVerts[1, 1] = p4;
                            nextCon.leftFPConnectorVerts[1, 2] = p6;
                        }
                        faceVerts.Add(p6);
                    }
                    else
                    {
                        p6 = interVert + Vector3.Cross(outDir2, Vector3.up).normalized * (nextCon.node2 == node ? nextCon.leftFootpathWidthMultiplier : nextCon.rightFootpathWidthMultiplier) * footPathWidth;
                        Vector3 p6_ = p6 - Vector3.Cross(outDir2, Vector3.up).normalized * footPathEdgeWidth;
                        if (nextCon.node1 == node)
                        {
                            nextCon.rightFPConnectorVerts[0, 0] = p6;
                            nextCon.rightFPConnectorVerts[0, 1] = p6_;
                            nextCon.rightFPConnectorVerts[0, 2] = interVert;
                        }
                        else
                        {

                            nextCon.leftFPConnectorVerts[1, 0] = p6;
                            nextCon.leftFPConnectorVerts[1, 1] = p6_;
                            nextCon.leftFPConnectorVerts[1, 2] = interVert;
                        }
                        innerEdgeVerts.Add(p6_);
                        outerEdgeVerts.Insert(0, p6);
                    }

                    innerEdgeVerts.Reverse();
                    meshVerts.AddRange(innerEdgeVerts);
                    meshVerts.AddRange(outerEdgeVerts);
                    List<Vector3> footPathEdgeVerts = new List<Vector3>();
                    footPathEdgeVerts.AddRange(innerEdgeVerts);
                    footPathEdgeVerts.AddRange(outerEdgeVerts);
                    meshTriangles[node.footPathEdgeMatIndex].AddRange(Triangulator.StripTriangulate(outerEdgeVerts.ToArray(), innerEdgeVerts.ToArray(), sInd));
                    float soFar = 0;
                    uvList.Add(new Vector2(1f, 0));
                    for (int k = 1; k < innerEdgeVerts.Count; k++)
                    {
                        uvList.Add(new Vector2(1f, soFar + (outerEdgeVerts[k] - outerEdgeVerts[k - 1]).magnitude));
                        soFar += (outerEdgeVerts[k] - outerEdgeVerts[k - 1]).magnitude;
                    }
                    soFar = 0;
                    uvList.Add(new Vector2(0.5f, soFar));
                    for (int k = 1; k < innerEdgeVerts.Count; k++)
                    {
                        uvList.Add(new Vector2(0.5f, soFar + (outerEdgeVerts[k] - outerEdgeVerts[k - 1]).magnitude));
                        soFar += (outerEdgeVerts[k] - outerEdgeVerts[k - 1]).magnitude;
                    }

                    sInd += footPathEdgeVerts.Count;

                    // vertical outer strip
                    footPathEdgeVerts.Clear();
                    List<Vector3> loweredOuterEdgeVerts = new List<Vector3>();
                    for (int k = 0; k < outerEdgeVerts.Count; k++)
                    {
                        loweredOuterEdgeVerts.Add(outerEdgeVerts[k] - Vector3.up * sidewalkElevation);
                    }
                    footPathEdgeVerts.AddRange(outerEdgeVerts);
                    footPathEdgeVerts.AddRange(loweredOuterEdgeVerts);
                    meshVerts.AddRange(footPathEdgeVerts);
                    meshTriangles[node.footPathEdgeMatIndex].AddRange(Triangulator.StripTriangulate(outerEdgeVerts.ToArray(), loweredOuterEdgeVerts.ToArray(), sInd));
                    //soFar = 0;
                    soFar = 0;
                    uvList.Add(new Vector2(0.5f, soFar));
                    for (int k = 1; k < outerEdgeVerts.Count; k++)
                    {
                        uvList.Add(new Vector2(0.5f, soFar + (outerEdgeVerts[k] - outerEdgeVerts[k - 1]).magnitude));
                        soFar += (outerEdgeVerts[k] - outerEdgeVerts[k - 1]).magnitude;
                    }
                    soFar = 0;
                    uvList.Add(new Vector2(0, soFar));
                    for (int k = 1; k < loweredOuterEdgeVerts.Count; k++)
                    {
                        uvList.Add(new Vector2(0f, soFar + (outerEdgeVerts[k] - outerEdgeVerts[k - 1]).magnitude));
                        soFar += (outerEdgeVerts[k] - outerEdgeVerts[k - 1]).magnitude;
                    }
                    sInd += footPathEdgeVerts.Count;

                    // vertical inner strip
                    outerEdgeVerts = new List<Vector3> { p5, interVert, p6 };
                    loweredOuterEdgeVerts = new List<Vector3> { p5 - Vector3.up * sidewalkElevation, interVert - Vector3.up * sidewalkElevation, p6 - Vector3.up * sidewalkElevation };
                    meshVerts.AddRange(outerEdgeVerts);
                    meshVerts.AddRange(loweredOuterEdgeVerts);
                    meshTriangles[node.footPathIntersectMatIndex].AddRange(Triangulator.StripTriangulate(outerEdgeVerts.ToArray(), loweredOuterEdgeVerts.ToArray(), sInd));
                    for (int k = 0; k < 3; k++)
                    {
                        uvList.Add(new Vector2(outerEdgeVerts[k].x, outerEdgeVerts[k].z) * node.scaleFootpathUV);
                    }
                    for (int k = 0; k < 3; k++)
                    {
                        uvList.Add(new Vector2(loweredOuterEdgeVerts[k].x, loweredOuterEdgeVerts[k].z) * node.scaleFootpathUV);
                    }
                    sInd += 6;

                    // top face of foothpath turn
                    //innerEdgeVerts.Reverse();

                    faceVerts.AddRange(innerEdgeVerts);
                    meshVerts.AddRange(faceVerts);
                    meshTriangles[node.footPathIntersectMatIndex].AddRange(Triangulator.Triangulate(faceVerts.ToArray(), sInd));
                    for (int k = 0; k < faceVerts.Count; k++)
                    {
                        uvList.Add(new Vector2(faceVerts[k].x, faceVerts[k].z) * node.scaleFootpathUV);
                    }
                    sInd += faceVerts.Count;

                    // triangular road segment
                    if (Vector3.Dot(Vector3.Cross(con.GetOutDirection(node), Vector3.up), nextCon.GetOutDirection(node)) > 0)
                    {
                        faceVerts.Clear();
                        faceVerts = new List<Vector3> { p1 - Vector3.up * sidewalkElevation, p2 - Vector3.up * sidewalkElevation, node.intersectVerts[j] };
                        meshVerts.AddRange(faceVerts);
                        for (int k = 0; k < faceVerts.Count; k++)
                        {
                            uvList.Add(new Vector2(faceVerts[k].x, faceVerts[k].z) * node.scaleFootpathUV);
                        }
                        meshTriangles[node.materialIndex].AddRange(new int[] { sInd, sInd + 1, sInd + 2 });
                        sInd += 3;
                    }
                }
            }
            for (int i = 0; i < roadConnectionContainer.childCount; i++)
            {
                // Generate Crossings
                RoadConnection connection = roadConnectionContainer.GetChild(i).GetComponent<RoadConnection>();
                for (int j = 0; j < 2; j++)
                {
                    Vector3[] verts = new Vector3[4];
                    verts[0] = connection.connectorVerts[j][0];
                    verts[1] = connection.connectorVerts[j][1];
                    Vector3 dir = Vector3.Cross(verts[1] - verts[0], Vector3.up).normalized;
                    verts[2] = verts[1] - dir * crossingWidth;
                    verts[3] = verts[0] - dir * crossingWidth;
                    meshVerts.AddRange(verts);
                    meshTriangles[connection.crossingMaterialIndex].AddRange(Triangulator.QuadTriangles(0, 1, 2, 3, sInd));
                    sInd += 4;
                    uvList.AddRange(new List<Vector2> { new Vector2(0, crossingWidth), new Vector2(connection.lanes * 0.5f, crossingWidth), new Vector2(connection.lanes * 0.5f, 0), Vector2.zero });
                }

                // Generate road ways
                List<Vector3> roadEndVerts = new List<Vector3> { meshVerts[sInd - 1], meshVerts[sInd - 2], meshVerts[sInd - 5], meshVerts[sInd - 6] };
                meshVerts.AddRange(roadEndVerts.ToArray());
                meshTriangles[connection.roadMaterialIndex].AddRange(Triangulator.QuadTriangles(sInd, sInd + 1, sInd + 2, sInd + 3));
                float roadLength = (meshVerts[sInd - 2] - meshVerts[sInd - 5]).magnitude;
                uvList.AddRange(new List<Vector2> { new Vector2(connection.lanes * 0.5f, 0), Vector2.zero, new Vector2(0, roadLength), new Vector2(connection.lanes * 0.5f, roadLength) });
                sInd += 4;

                // Generate Footpaths
                // left footpath
                List<Vector3> fpEdgeRail1 = new List<Vector3> { connection.leftFPConnectorVerts[0, 0] - Vector3.up * sidewalkElevation, connection.leftFPConnectorVerts[0, 0], connection.leftFPConnectorVerts[0, 0], connection.leftFPConnectorVerts[0, 1] };
                List<Vector3> fpEdgeRail2 = new List<Vector3> { connection.leftFPConnectorVerts[1, 0] - Vector3.up * sidewalkElevation, connection.leftFPConnectorVerts[1, 0], connection.leftFPConnectorVerts[1, 0], connection.leftFPConnectorVerts[1, 1] };
                meshVerts.AddRange(fpEdgeRail1);
                meshVerts.AddRange(fpEdgeRail2);
                float dist = (fpEdgeRail1[0] - fpEdgeRail2[0]).magnitude;
                meshTriangles[connection.footPathEdgeMatIndex].AddRange(Triangulator.QuadTriangles(0, 1, 5, 4, sInd));
                meshTriangles[connection.footPathEdgeMatIndex].AddRange(Triangulator.QuadTriangles(2, 3, 7, 6, sInd));
                uvList.AddRange(new List<Vector2> { new Vector2(1, 0), new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 0) });
                uvList.AddRange(new List<Vector2> { new Vector2(1, dist), new Vector2(0.5f, dist), new Vector2(0.5f, dist), new Vector2(0, dist) });

                sInd += fpEdgeRail1.Count + fpEdgeRail2.Count;
                List<Vector3> prevRail = new List<Vector3> { connection.leftFPConnectorVerts[0, 2] - Vector3.up * sidewalkElevation, connection.leftFPConnectorVerts[0, 2], connection.leftFPConnectorVerts[0, 1] };
                List<Vector3> rail = new List<Vector3> { connection.leftFPConnectorVerts[1, 2] - Vector3.up * sidewalkElevation, connection.leftFPConnectorVerts[1, 2], connection.leftFPConnectorVerts[1, 1] };

                meshVerts.AddRange(prevRail);
                uvList.AddRange(new Vector2[] { new Vector2(1.1f, 0), new Vector2(1, 0), new Vector2(0, 0) });
                dist = (prevRail[0] - rail[0]).magnitude;
                meshVerts.AddRange(rail);
                uvList.AddRange(new Vector2[] { new Vector2(1.1f, dist * connection.scaleFootpathUV), new Vector2(1, dist * connection.scaleFootpathUV), new Vector2(0, dist * connection.scaleFootpathUV) });
                meshTriangles[connection.footPathMatIndex].AddRange(Triangulator.StripTriangulate(prevRail.ToArray(), rail.ToArray(), sInd, true));

                sInd += 6;

                //right Footpath
                fpEdgeRail2 = new List<Vector3> { connection.rightFPConnectorVerts[0, 0] - Vector3.up * sidewalkElevation, connection.rightFPConnectorVerts[0, 0], connection.rightFPConnectorVerts[0, 0], connection.rightFPConnectorVerts[0, 1] };
                fpEdgeRail1 = new List<Vector3> { connection.rightFPConnectorVerts[1, 0] - Vector3.up * sidewalkElevation, connection.rightFPConnectorVerts[1, 0], connection.rightFPConnectorVerts[1, 0], connection.rightFPConnectorVerts[1, 1] };
                meshVerts.AddRange(fpEdgeRail1);
                meshVerts.AddRange(fpEdgeRail2);
                dist = (fpEdgeRail1[0] - fpEdgeRail2[0]).magnitude;
                meshTriangles[connection.footPathEdgeMatIndex].AddRange(Triangulator.QuadTriangles(0, 1, 5, 4, sInd));
                meshTriangles[connection.footPathEdgeMatIndex].AddRange(Triangulator.QuadTriangles(2, 3, 7, 6, sInd));
                uvList.AddRange(new List<Vector2> { new Vector2(1, 0), new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 0) });
                uvList.AddRange(new List<Vector2> { new Vector2(1, dist), new Vector2(0.5f, dist), new Vector2(0.5f, dist), new Vector2(0, dist) });

                sInd += fpEdgeRail1.Count + fpEdgeRail2.Count;
                prevRail = new List<Vector3> { connection.rightFPConnectorVerts[0, 2] - Vector3.up * sidewalkElevation, connection.rightFPConnectorVerts[0, 2], connection.rightFPConnectorVerts[0, 1] };
                rail = new List<Vector3> { connection.rightFPConnectorVerts[1, 2] - Vector3.up * sidewalkElevation, connection.rightFPConnectorVerts[1, 2], connection.rightFPConnectorVerts[1, 1] };


                meshVerts.AddRange(prevRail);
                uvList.AddRange(new Vector2[] { new Vector2(1.1f, 0), new Vector2(1, 0), new Vector2(0, 0) });
                dist = (prevRail[0] - rail[0]).magnitude;
                meshVerts.AddRange(rail);
                uvList.AddRange(new Vector2[] { new Vector2(1.1f, dist * connection.scaleFootpathUV), new Vector2(1, dist * connection.scaleFootpathUV), new Vector2(0, dist * connection.scaleFootpathUV) });
                meshTriangles[connection.footPathMatIndex].AddRange(Triangulator.StripTriangulate(prevRail.ToArray(), rail.ToArray(), sInd));

                sInd += 6;
            }

            //meshFilter.sharedMesh.Clear();
            mesh.subMeshCount = materialsCount;
            mesh.vertices = meshVerts.ToArray();
            for (int i = 0; i < materialsCount; i++)
            {
                mesh.SetTriangles(meshTriangles[i], i);
            }

            //mesh.colors = SetVertexColors(meshVerts.ToArray());
            mesh.uv = uvList.ToArray();
            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;
            meshFilter.GetComponent<MeshCollider>().sharedMesh = mesh;
            Debug.Log("Mesh Updated");

            SpawnProps();
        }

        void SpawnProps()
        {
            for (int i = 0; i < propsContainer.childCount; i++)
            {
                while (propsContainer.GetChild(i).childCount > 0)
                {
                    DestroyImmediate(propsContainer.GetChild(i).GetChild(0).gameObject);
                }
            }
            if (trafficSignal != null)
            {
                for (int i = 0; i < roadNodeContainer.childCount; i++)
                {
                    RoadNode node = roadNodeContainer.GetChild(i).GetComponent<RoadNode>();
                    if (node.connections.Count > 2)
                    {
                        node.trafficSignals = new List<TrafficSignal>();
                        for (int j = 0; j < node.connections.Count; j++)
                        {
                            GameObject tsObject = Instantiate(trafficSignal, propsContainer.GetChild(0));
                            Vector3 flatOutDir = node.connections[j].GetOutDirection(node);
                            flatOutDir.y = 0;
                            Vector3 rightDir = Vector3.Cross(flatOutDir, Vector3.up);
                            tsObject.transform.position = node.crossingVerts[2 * j + 1] + rightDir * (footPathEdgeWidth + 0.5f) + Vector3.up*sidewalkElevation;
                            tsObject.transform.rotation = Quaternion.LookRotation(flatOutDir);
                            node.trafficSignals.Add(tsObject.GetComponent<TrafficSignal>());
                        }
                    }
                }
            }
            if (streetLight != null)
            {
                for (int i = 0; i < roadConnectionContainer.childCount; i++)
                {
                    RoadConnection con = roadConnectionContainer.GetChild(i).GetComponent<RoadConnection>();
                    Vector3 rightDir = Vector3.Cross(con.GetOutDirection(con.node1),Vector3.up);
                    rightDir.y = 0;
                    float length = (con.startPoint - con.endPoint).magnitude;
                    int segments = (int)length / 15;
                    bool sign = true;
                    for (int j = 1; j < segments; j++)
                    {
                        GameObject sl = Instantiate(streetLight, propsContainer.GetChild(1));
                        sl.transform.position = Vector3.Lerp(con.startPoint, con.endPoint, (float)j / (float)segments) + (sign ? 1 : -1) * rightDir * (0.5f*con.GetRoadWidth() + 1) + Vector3.up*sidewalkElevation;
                        sl.transform.rotation = Quaternion.LookRotation(sign?-rightDir:rightDir);
                        sign=!sign;
                    }
                }
            }
            Debug.Log("Props Spawned");
        }

        Vector3 BezierPoint(Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            return t * t * p3 + 2 * t * (1 - t) * p2 + (1 - t) * (1 - t) * p1;
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

        public static float GetMaxSpeed(RoadConnection.SpeedTier speedTier)
        {
            float speed = 40;
            switch (speedTier)
            {
                case RoadConnection.SpeedTier.slow:
                    speed = 15;
                    break;
                case RoadConnection.SpeedTier.medium:
                    speed = 25;
                    break;
                case RoadConnection.SpeedTier.fast:
                    speed = 50;
                    break;
                default:
                    speed = 25;
                    break;
            }
            return speed * (5f / 18f);
        }

    }
}