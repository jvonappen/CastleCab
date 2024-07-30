using UnityEngine;

namespace FIMSpace.FEyes
{
    public partial class FEyesAnimator
    {
        public float HeadToTargetAngle { get; private set; }
        public Vector3 HeadRotation { get; private set; }
        public Vector3 TargetLookDirection { get; private set; }
        public Vector3 LookRotationBase { get; private set; }
        public Vector3 DeltaVector { get; private set; }
        private Vector2 clampDelta;
        public Vector3 LookStartPositionBase { get; private set; }
        public Vector2 LookDeltaAngles { get; private set; }
        public Vector2 LookDeltaAnglesClamped { get; private set; }
        bool blendshapeAnglesRequest = false;

        private void ComputeBaseRotations(ref Quaternion lookRotationBase)
        {
            // Look position referencing from middle of head for unsquinted look rotation
            LookStartPositionBase = GetStartLookPosition();
            TargetLookDirection = targetLookPosition - LookStartPositionBase;

            HeadToTargetAngle = Vector3.Angle(HeadReference.transform.TransformDirection(headForward), TargetLookDirection.normalized);

            Quaternion lookRotationQuatBase = Quaternion.LookRotation(TargetLookDirection, WorldUp);
            Quaternion fadedRot = lookRotationQuatBase;

            // Just look towards target rotation
            if (FollowTargetAmount * conditionalFollowBlend < 1f)
            {   // Look towards target with default eye rotation blend
                Quaternion defaultLookRot = Quaternion.LookRotation(HeadReference.rotation * headForward * 100f, WorldUp);

                if (FollowTargetAmount * conditionalFollowBlend > 0f)
                    fadedRot = Quaternion.SlerpUnclamped(defaultLookRot, Quaternion.LookRotation(targetLookPosition - LookStartPositionBase), FollowTargetAmount * conditionalFollowBlend);
                else
                    fadedRot = defaultLookRot;
            }

            LookRotationBase = fadedRot.eulerAngles;

            // Head rotation to offset clamp ranges in head rotates in animation clip of skeleton
            HeadRotation = (HeadReference.rotation * Quaternion.FromToRotation(headForwardFromTo, Vector3.forward)).eulerAngles;

            CheckLookRanges();

            if( blendshapeAnglesRequest )
            {
                // Target look rotation equivalent for LeadBone's parent
                Vector3 lookDirectionParent = Quaternion.Inverse( HeadReference.rotation ) * ( TargetLookDirection.normalized );

                Vector2 lookDeltaAngle;
                // Getting angle offset in y axis - horizontal rotation
                lookDeltaAngle.y = AngleAroundAxis( headReferenceLookForward, lookDirectionParent, headReferenceUp );

                Vector3 targetRight = Vector3.Cross( headReferenceUp, lookDirectionParent );
                Vector3 horizontalPlaneTarget = lookDirectionParent - Vector3.Project( lookDirectionParent, headReferenceUp );

                lookDeltaAngle.x = AngleAroundAxis( horizontalPlaneTarget, lookDirectionParent, targetRight );
                LookDeltaAngles = lookDeltaAngle;

                Vector2 clamped = LookDeltaAngles;
                if( LookDeltaAngles.x < EyesClampVertical.x ) clamped.x = EyesClampVertical.x;
                else if( LookDeltaAngles.x > EyesClampVertical.y ) clamped.x = EyesClampVertical.y;

                if( LookDeltaAngles.y < EyesClampHorizontal.x ) clamped.y = EyesClampHorizontal.x;
                else if( LookDeltaAngles.y > EyesClampHorizontal.y ) clamped.y = EyesClampHorizontal.y;
                LookDeltaAnglesClamped = clamped;
            }
        }

        /// <summary>
        /// Calculate angle between two directions around defined axis
        /// </summary>
        public static float AngleAroundAxis( Vector3 firstDirection, Vector3 secondDirection, Vector3 axis )
        {
            // Projecting to orthogonal target axis plane
            firstDirection = firstDirection - Vector3.Project( firstDirection, axis );
            secondDirection = secondDirection - Vector3.Project( secondDirection, axis );
            float angle = Vector3.Angle( firstDirection, secondDirection );
            return angle * ( Vector3.Dot( axis, Vector3.Cross( firstDirection, secondDirection ) ) < 0 ? -1 : 1 );
        }


        public bool OutOfRange { get; private set; }
        public bool OutOfDistance { get; private set; }
        private bool? forceOutOfMaxDistance = null;

        /// <summary>
        /// Checking if look target is not out of follow angle range or distance
        /// </summary>
        private void CheckLookRanges()
        {
            if (StopLookAbove >= 180)
            {
                OutOfRange = false;
            }
            else
            {
                // Range blending out eyes animation
                if (Mathf.Abs(HeadToTargetAngle) < StopLookAbove)
                {
                    OutOfRange = false;
                }
                else
                {
                    if (Mathf.Abs(HeadToTargetAngle) > StopLookAbove * 1.2f + 10)
                        OutOfRange = true;
                }
            }

            if (MaxTargetDistance > 0f)
            {
                float distance = Vector3.Distance(LookStartPositionBase, targetLookPosition);

                if (distance < MaxTargetDistance) OutOfDistance = false;
                else
                if (distance > MaxTargetDistance + MaxTargetDistance * GoOutFactor) OutOfDistance = true;
            }
            else
                OutOfDistance = false;


            bool outOfRange;
            if( forceOutOfMaxDistance == null )
            {
                outOfRange = OutOfRange || OutOfDistance;
            }
            else outOfRange = forceOutOfMaxDistance.Value;

            if ( outOfRange )
            {
                conditionalFollowBlend = Mathf.Max(0f, conditionalFollowBlend - Time.deltaTime * 5f);
            }
            else
            {
                conditionalFollowBlend = Mathf.Min(1f, conditionalFollowBlend + Time.deltaTime * 5f);
            }
        }


        /// <summary>
        /// Computing rotation for single eye using shared variables
        /// </summary>
        private void ComputeLookingRotation(ref Quaternion lookRotationBase, EyeSetup eyeSetup, int randomIndex = 0, int lagId = 0)
        {
            Vector3 lookRotation = this.LookRotationBase;
            if (eyesData[randomIndex].randomDir != Vector3.zero) lookRotation += Vector3.Lerp(Vector3.zero, eyesData[randomIndex].randomDir, EyesRandomMovement);

            // Vector with degrees differences to all needed axes
            DeltaVector = new Vector3(Mathf.DeltaAngle(lookRotation.x, HeadRotation.x), Mathf.DeltaAngle(lookRotation.y, HeadRotation.y));

            // Clamping look rotation
            if (!IndividualClamping)
                ClampDetection(DeltaVector, ref lookRotation, HeadRotation, EyesClampHorizontal, EyesClampVertical);
            else
                ClampDetection(DeltaVector, ref lookRotation, HeadRotation, eyeSetup.IndividualClampingHorizontal, eyeSetup.IndividualClampingVertical);

            lookRotationBase = Quaternion.Euler(lookRotation);
        }



        Vector3 GetStartLookPosition()
        {
            Vector3 lookStartPositionBase;

            if ( StaticLookStartPosition )
            {
                lookStartPositionBase = BaseTransform.position;
                lookStartPositionBase.y = HeadReference.position.y;
            }
            else
                lookStartPositionBase = HeadReference.transform.position;

            lookStartPositionBase += HeadReference.TransformVector(StartLookOffset);
            return lookStartPositionBase;
        }

        public int clampedHorizontal = 0;
        public int clampedVertical = 0;
        public bool IsClamping { get; private set; }
        protected virtual void ClampDetection(Vector2 deltaVector, ref Vector3 lookRotation, Vector3 rootOffset, Vector2 clampHor, Vector2 clampVert)
        {
            clampDelta = deltaVector;

            // Limit when looking left or right
            if (deltaVector.y > -clampHor.x)
            {
                clampDelta.y = -clampHor.x;
                lookRotation.y = rootOffset.y + clampHor.x;
                clampedHorizontal = -1;
            }
            else if (deltaVector.y < -clampHor.y)
            {
                clampDelta.y = -clampHor.y;
                lookRotation.y = rootOffset.y + clampHor.y;
                clampedHorizontal = 1;
            }
            else clampedHorizontal = 0;

            // Limit when looking up or down
            if (deltaVector.x > clampVert.y)
            {
                clampDelta.x = clampVert.y;
                clampedVertical = 1;
                lookRotation.x = rootOffset.x - clampVert.y;
            }
            else if (deltaVector.x < clampVert.x)
            {
                clampDelta.x = clampVert.x;
                clampedVertical = -1;
                lookRotation.x = rootOffset.x - clampVert.x;
            }
            else clampedVertical = 0;

            deltaV = deltaVector;

            if (clampedHorizontal != 0 || clampedVertical != 0) IsClamping = true;
        }

    }
}