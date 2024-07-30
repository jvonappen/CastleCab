using System;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FEyes
{
    public partial class FEyesAnimator : UnityEngine.EventSystems.IDropHandler, IFHierarchyIcon
    {
        public bool debugSwitch = false;
        public string EditorIconPath { get { if (PlayerPrefs.GetInt("EyesH", 1) == 0) return ""; else return "Eyes Animator/EyesAnimator_IconSmall"; } }
        public void OnDrop(UnityEngine.EventSystems.PointerEventData data) { }

        void Reset()
        {
            _baseTransform = transform;
        }

        /// <summary>
        /// TODO: Full implementation of all arrays inside just this class
        /// </summary>
        [System.Serializable]
        public class Eye
        {
            // Info
            public Vector3 forward;
            public Quaternion initLocalRotation;
            public Quaternion lerpRotation;

            // Random motion
            public Vector3 randomDir;
            public float randomTimer;

            // Lag motion
            public float lagTimer;
            public float lagProgress;
            public Quaternion lagStartRotation;
            public void SetLagStartRotation(Transform baseTr, Quaternion worldRot) 
            { lagStartRotation = previousLookRotBase; }
            //{ lagStartRotation = FEngineering.QToLocal(baseTr.rotation, worldRot); }
            public Quaternion GetLagStartRotation(Transform baseTr) 
            { return lagStartRotation; }
            //{ return FEngineering.QToWorld(baseTr.rotation, lagStartRotation); }


            public float changeSmoother;

            /// <summary> Mainly for supporting eye lags </summary>
            public Quaternion previousLookRotBase;
        }

        [System.Serializable]
        public class EyeSetup
        {
            public SkinnedMeshRenderer BlendshapeMesh = null;
            public int EyeRightShape = 0;
            public int EyeLeftShape = 0;
            public int EyeUpShape = 0;
            public int EyeDownShape = 0;
            public Vector2 MinMaxValue = new Vector2(0, 100);

            public Vector2 IndividualClampingHorizontal = new Vector2(-60, 60);
            public Vector2 IndividualClampingVertical = new Vector2(-20,20);

            public enum EEyeControlType
            {
                RotateBone, Blendshape
            }

            public EEyeControlType ControlType = EEyeControlType.RotateBone;

            [HideInInspector] public bool _BlendFoldout = false;

            internal void EyeLeftX(float x)
            {
                float value = Mathf.Lerp(MinMaxValue.x, MinMaxValue.y, Mathf.Abs( Mathf.Min(0f, x)));
                BlendshapeMesh.SetBlendShapeWeight(EyeLeftShape, value);
            }
            internal void EyeRightX(float x)
            {
                float value = Mathf.Lerp(MinMaxValue.x, MinMaxValue.y, Mathf.Max(0f, x));
                BlendshapeMesh.SetBlendShapeWeight(EyeRightShape, value);
            }
            internal void EyeDownY(float y)
            {
                float value = Mathf.Lerp(MinMaxValue.x, MinMaxValue.y, Mathf.Abs(Mathf.Min(0f, y)));
                BlendshapeMesh.SetBlendShapeWeight(EyeDownShape, value);
            }
            internal void EyeUpY(float y)
            {
                float value = Mathf.Lerp(MinMaxValue.x, MinMaxValue.y, Mathf.Max(0f, y));
                BlendshapeMesh.SetBlendShapeWeight(EyeUpShape, value);
            }
        }

        [HideInInspector] public List<EyeSetup> EyeSetups = new List<EyeSetup>();

    }
}