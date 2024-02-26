using UnityEditor;
using UnityEngine;

namespace URNTS
{
    [CustomEditor(typeof(VehicleController))]
    public class VehicleEditor : Editor
    {
        private Vector3 newCenterOfMass;


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            if (GUILayout.Button("Set Parameters"))
            {
                VehicleController cont = (VehicleController)target;

                // Set Steer Point
                if (cont.steerPoint != null)
                {
                    cont.steerPoint.position = 0.5f * (cont.WheelFL.transform.position + cont.WheelFR.transform.position);
                }
                else
                {
                    GameObject steerPoint = new GameObject("SteerPoint");
                    steerPoint.transform.position = 0.5f * (cont.WheelFL.transform.position + cont.WheelFR.transform.position);
                    steerPoint.transform.rotation = cont.transform.rotation;
                    steerPoint.transform.parent = cont.transform;
                }

                // Calculate WheelBase
                cont.wheelBase = (cont.WheelBL.transform.position - cont.WheelFL.transform.position).magnitude;

                // Calculate WheelTrack
                cont.wheelTrack = (cont.WheelFL.transform.position - cont.WheelFR.transform.position).magnitude;

                Debug.Log("Vehicle Parameters Updated!");
            }
        }

        public virtual void OnSceneGUI()
        {
            VehicleController cont = (VehicleController)target;
            newCenterOfMass = Handles.PositionHandle(cont.transform.TransformPoint(cont.COM), cont.transform.rotation);
            Handles.Label(cont.transform.TransformPoint(cont.COM), "COM");
            if (GUI.changed)
            {
                Undo.RecordObject(cont, "Move Center Of Mass");
                cont.COM = cont.transform.InverseTransformPoint(newCenterOfMass);
                EditorUtility.SetDirty(target);
            }
        }
    }
}