using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FEyes
{
    public partial class FEyesAnimator
    {
        public bool _gizmosDrawMaxDist = true;

        public bool DrawGizmos = true;


        Vector3[] _editor_eyeForwards;
        protected void ComputeReferences()
        {
            if (Application.isPlaying) return;

            _editor_eyeForwards = new Vector3[Eyes.Count];

            for (int i = 0; i < _editor_eyeForwards.Length; i++)
            {
                if (Eyes[i] == null) { _editor_eyeForwards[i] = Vector3.zero; continue; }
                Vector3 rootPos = Eyes[i].position;
                Vector3 targetPos = Eyes[i].position + Vector3.Scale(transform.forward, Eyes[i].transform.lossyScale);
                _editor_eyeForwards[i] = (Eyes[i].InverseTransformPoint(targetPos) - Eyes[i].InverseTransformPoint(rootPos)).normalized;
            }

            //headForward = Quaternion.FromToRotation(HeadReference.TransformDirection(Vector3.forward), BaseTransform.forward) * transform.forward;
            headForwardFromTo = Quaternion.FromToRotation(HeadReference.InverseTransformDirection(transform.forward), Vector3.forward) * Vector3.forward;
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (!DrawGizmos) return;

            float scaleRef = 0.6f;
            if (HeadReference) if (Eyes != null) if (Eyes.Count > 0) if (Eyes[0] != null) scaleRef = Vector3.Distance(HeadReference.position, Eyes[0].position);

            if (EyesTarget != null && HeadReference != null)
            {
                Vector3 lookStartPositionBase;

                if ( StaticLookStartPosition)
                {
                    lookStartPositionBase = BaseTransform.position;
                    lookStartPositionBase.y = HeadReference.position.y;
                }
                else
                {
                    lookStartPositionBase = HeadReference.position;
                }

                lookStartPositionBase += HeadReference.TransformVector(StartLookOffset);

                Gizmos.color = new Color(0.3f, 1f, 0.3f, 0.7f);
                Gizmos.DrawLine(lookStartPositionBase, (EyesTarget.position + targetLookPositionOffset));
            }


            if (HeadReference != null)
            {
                ComputeReferences();
                Vector3 f;

                Gizmos.color = new Color(0.3f, 0.3f, 1f, 0.7f);
                for (int i = 0; i < Eyes.Count; i++)
                {
                    if (Eyes[i] == null) continue;

                    Vector3 eyeF;
                    if (!Application.isPlaying) eyeF = _editor_eyeForwards[i]; else eyeF = eyesData[i].forward;

                    Quaternion eyeForwarded = Eyes[i].rotation * Quaternion.Inverse(Quaternion.FromToRotation(eyeF, Vector3.forward));
                    f = eyeForwarded * Vector3.forward * (scaleRef * 2.75f);

                    Gizmos.DrawRay(Eyes[i].position, f);
                    Gizmos.DrawLine(Eyes[i].position + f, Eyes[i].position + f * 0.6f + eyeForwarded * Vector3.right * (scaleRef / 2f));
                    Gizmos.DrawLine(Eyes[i].position + f, Eyes[i].position + f * 0.6f + eyeForwarded * Vector3.left * (scaleRef / 2f));
                }

                Vector3 middle = Vector3.Lerp(transform.position, HeadReference.position, 0.5f);
                Gizmos.DrawSphere(middle, scaleRef * 0.25f);
                Quaternion headForwarded = HeadReference.rotation * Quaternion.FromToRotation(headForwardFromTo, Vector3.forward);
                f = headForwarded * Vector3.forward * (scaleRef * 2.75f);

                Gizmos.DrawRay(middle, f);
                Gizmos.DrawLine(middle + f, middle + f * 0.6f + headForwarded * Vector3.right * (scaleRef / 2f));
                Gizmos.DrawLine(middle + f, middle + f * 0.6f + headForwarded * Vector3.left * (scaleRef / 2f));
            }

            Gizmos_DrawMaxDistance();
        }


        private void Gizmos_DrawMaxDistance()
        {
            if (MaxTargetDistance <= 0f) return;
            if (!_gizmosDrawMaxDist) return;

            Vector3 startLook = GetStartLookPosition();
            float a = 0.525f;
            Gizmos.color = new Color(.1f, .835f, .08f, a);

            Gizmos.DrawWireSphere(GetStartLookPosition(), MaxTargetDistance);

            if (GoOutFactor > 0f)
            {
                Gizmos.color = new Color(.835f, .135f, .08f, a);
                Gizmos.DrawWireSphere(GetStartLookPosition(), MaxTargetDistance + MaxTargetDistance * GoOutFactor);
            }

            Gizmos.color = new Color(0.02f, .65f, 0.2f, a);
            Gizmos.DrawLine(startLook - Vector3.right * (MaxTargetDistance + MaxTargetDistance * GoOutFactor), startLook + Vector3.right * (MaxTargetDistance + MaxTargetDistance * GoOutFactor));
            Gizmos.DrawLine(startLook - BaseTransform.forward.normalized * (MaxTargetDistance + MaxTargetDistance * GoOutFactor), startLook + Vector3.forward * (MaxTargetDistance + MaxTargetDistance * GoOutFactor));
        }
    }
}