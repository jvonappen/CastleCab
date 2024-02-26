using UnityEditor;
using UnityEngine;

namespace URNTS
{
    [CustomEditor(typeof(TrafficManager))]
    public class TrafficManagerEditor : Editor
    {
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