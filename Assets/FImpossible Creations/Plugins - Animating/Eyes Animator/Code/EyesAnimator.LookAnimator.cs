#if EYES_LOOKANIMATOR_IMPORTED
using FIMSpace.FLook;
using UnityEngine;

namespace FIMSpace.FEyes
{
    public partial class FEyesAnimator
    {
        // If you are using also look animator, you can simply uncomment this and one LateUpdate() line for this feature
        public FLookAnimator LookAnimator = null;

        [Tooltip("Syncing object to follow")]
        public bool SyncTarget = true;
        [Tooltip("Syncing Clamping Ranges but in most cases eyes can use bigger ranges so please check it")]
        public bool SyncClamping = false;
        [Tooltip("Syncing 'Stop Look Above' and max distance ranges")]
        public bool SyncRanges = true;
        [Tooltip("Syncing 'Look Animator Amount' to blend eyes animator simultaneously")]
        public bool SyncUseAmount = true;
        private float lookSyncBlend = 1f;

        private void StartLookAnim()
        {
            // Queueing for component to be executed after look animator
            if (LookAnimator)
            {
                LookAnimator.enabled = false;
                LookAnimator.enabled = true;
            }

            enabled = false;
            enabled = true;

            if (LookAnimator == null) LookAnimator = GetComponentInChildren<FLookAnimator>();
        }


        public void UpdateLookAnim()
        {
            if (!LookAnimator) return;

            if (SyncClamping)
            {
                EyesClampHorizontal = LookAnimator.XRotationLimits;
                EyesClampVertical = LookAnimator.YRotationLimits;
            }

            if (SyncRanges)
            {
                if( LookAnimator.LookState == FLookAnimator.EFHeadLookState.OutOfMaxDistance || LookAnimator.LookState == FLookAnimator.EFHeadLookState.OutOfMaxRotation )
                {
                    forceOutOfMaxDistance = true;
                }
                else forceOutOfMaxDistance = false;

                StopLookAbove = LookAnimator.StopLookingAbove;
            }
            else
            {
                forceOutOfMaxDistance = null;
            }

            if (SyncUseAmount)
            {
                lookSyncBlend = LookAnimator.LookAnimatorAmount;
            }
            else lookSyncBlend = 1f;

            SyncTheTarget();
        }


        public void SyncTheTarget()
        {
            if (SyncTarget)
            {
                if (LookAnimator != null)
                {
                    Transform target = LookAnimator.GetEyesTarget();
                    EyesTarget = target;
                }
            }
        }

    }
}
#endif
