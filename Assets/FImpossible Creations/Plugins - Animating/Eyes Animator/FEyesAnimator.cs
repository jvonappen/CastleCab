using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FEyes
{
    /// <summary>
    /// FM: Class which is controlling eyes spheres to make them move to follow objects positions
    /// simulate random eyes movement, simulating eye movement lags etc.
    /// </summary>
    [AddComponentMenu("FImpossible Creations/Eyes Animator 2")]
    [DefaultExecutionOrder(16)] // Must be Executed after Look Animator and other Fimpossible Creations procedural animation packages
    public partial class FEyesAnimator : MonoBehaviour
    {
        public Transform BaseTransform { get { if (_baseTransform == null) return transform; else return _baseTransform; } }
        [Tooltip("Main reference object for eyes, must face forward (blue-Z) axis with it's rotation")]
        [SerializeField] private Transform _baseTransform = null;

        [Tooltip("Target to look for eyes")]
        public Transform EyesTarget;

        [Tooltip("Head transform reference for look start position and also reference to limit range how much eyes can rotate")]
        public Transform HeadReference;

        [Tooltip("Sometimes head bone can be too low or too high and you can correct this with StartLookOffset (depends of head bone rotations and scale etc. position axes can behave unusual)")]
        public Vector3 StartLookOffset;

        [Tooltip("Eyes transforms / bones (origin/pivot should be in center of the sphere")]
        public List<Transform> Eyes = new List<Transform>();

        [Tooltip("If you want component to not compute it's algorithm when certain renderer is not visible, drag & drop it here")]
        public Renderer OptimizeWithMesh = null;

        [Tooltip("All eyes animator features usage blend slider. Change if you want to disable all look animator features simultaneously (looking, random eyes movement and blinking.\n(Optimization) Eyes Animator will internally disable itself when this value is zero.")]
        [FPD_Suffix(0f, 1f)]
        public float EyesAnimatorAmount = 1f;

        [Tooltip("You can smoothly change it to 0 if you want to disable eyes animation looking at target behaviour")]
        [FPD_Suffix(0f, 1f)]
        public float FollowTargetAmount = 1f;
        [Tooltip("How fast eyes should rotate to desired rotations")]
        [Range(0.0f, 2f)]
        public float EyesSpeed = 1f;
        protected float eyesSpeedValue = 1f; // For quicker access in derived classes for calculations etc.

        [Tooltip("Compensating rotations for eyes to avoid squinting (eyes crossing), sometimes you will want to have some of this")]
        [Range(0.0f, 1f)]
        public float SquintPreventer = 1f;

        [Tooltip("Additional random movement for eyes giving more natural feel - you can crank it up for example when there is no target for eyes, or when character is talking with someone")]
        [FPD_Suffix(0f, 1f)]
        public float EyesRandomMovement = 0.3f;
        public Vector2 RandomMovementAxisScale = Vector2.one;

        public FERandomMovementType RandomMovementPreset = FERandomMovementType.Default;

        [Tooltip("How frequently should occur rotation change for random eyes movement")]
        [Range(0f, 3f)]
        public float RandomizingSpeed = 1f;

        [Tooltip("Option for monsters, each eye will have individual random rotation direction")]
        public bool EyesRandomMovementIndividual = false;


        [Tooltip("When we rotate eyes in real life, they're reaching target with kinda jumpy movement, but for more toon effect you can left this value at 0")]
        [FPD_Suffix(0f, 1f)]
        public float EyesLagAmount = 0.65f;
        [Tooltip("Making lags a bit smaller and more frequent when setted to lower value")]
        [Range(0.1f, 1f)]
        public float LagStiffness = 1f;
        [Tooltip("Option for monsters, each eye will have individual random delay for movement")]
        public bool IndividualLags = false;

        [Range(0f, 1f)]
        public float GoOutFactor = 0f;

        [Tooltip("In what angle eyes should go back to deafult position")]
        [FPD_Suffix(5, 180, FPD_SuffixAttribute.SuffixMode.FromMinToMaxRounded, "°")]
        public float StopLookAbove = 180f;
        [Tooltip("Maximum distance of target to look at, when exceed eyes will go back to default rotation. When max distance is equal 0, distance limit is infinite")]
        public float MaxTargetDistance = 0f;
        [Range(0.25f, 4f)]
        [Tooltip("Fading in/out eyes blend when max range is exceeded")]
        public float BlendTransitionSpeed = 1f;

        public Vector2 EyesClampHorizontal = new Vector2(-35f, 35f);
        public Vector2 EyesClampVertical = new Vector2(-35f, 35f);

        public List<Vector3> CorrectionOffsets = new List<Vector3>();

        public bool IndividualClamping = false;

        private Vector3 targetLookPosition;

        private float conditionalFollowBlend = 1f;
        private Vector3 targetLookPositionOffset = Vector3.zero;

        // Animation variables
        private Eye[] eyesData;
        private Vector3 headForwardFromTo;
        private Vector3 headForward;

        public Vector3 headReferenceLookForward;
        public Vector3 headReferenceUp;

        Vector3 WorldUp = Vector3.up;
        public bool WorldUpIsBaseTransformUp = false;
        [Tooltip("Makes start look reference position static, making look stable, but it will not work if your character will lie in bed or do other dramatic root bone transformations.")]
        public bool StaticLookStartPosition = false;

        /// <summary> Placeholder for custom forward vector (world space) </summary>
        Vector3 Forward { get { return Vector3.forward; } }
        /// <summary> Placeholder for custom up vector (world space) </summary>
        Vector3 Up { get { return Vector3.up; } }

        /// <summary>
        /// Preparing all needed variables and references
        /// </summary>
        protected virtual void Start()
        {
            UpdateLists();

            eyesData = new Eye[Eyes.Count];

            for (int i = 0; i < eyesData.Length; i++)
            {
                eyesData[i] = new Eye();
                Eye eye = eyesData[i];

                Vector3 rootPos = Eyes[i].position;
                Vector3 targetPos = Eyes[i].position + Vector3.Scale(transform.forward, Eyes[i].transform.lossyScale);
                eye.forward = (Eyes[i].InverseTransformPoint(targetPos) - Eyes[i].InverseTransformPoint(rootPos)).normalized;

                eye.initLocalRotation = Eyes[i].localRotation;
                eye.lerpRotation = Eyes[i].rotation;
                eye.SetLagStartRotation(BaseTransform, Eyes[i].rotation);

                eye.randomTimer = 0f;
                eye.randomDir = Vector3.zero;
                eye.lagTimer = 0f;
                eye.lagProgress = 1f;
                eye.changeSmoother = 1f;
            }

            AdjustCount(EyeSetups, Eyes.Count);

            headForward = HeadReference.InverseTransformDirection(transform.forward);
            headForwardFromTo = Quaternion.FromToRotation(headForward, Vector3.forward) * Vector3.forward;

            headReferenceLookForward = Quaternion.Inverse( HeadReference.rotation ) * BaseTransform.rotation * Forward;
            headReferenceUp = Quaternion.Inverse( HeadReference.rotation ) * BaseTransform.rotation * Up;

            OutOfDistance = true;
            OutOfRange = true;

            SetupBlinking();
            StartLookAnim();
        }

        private void Update()
        {
            if (EyesAnimatorAmount <= 0f) return;
            if (EyesRandomMovement <= 0f && FollowTargetAmount <= 0f) return;

            if( WorldUpIsBaseTransformUp ) WorldUp = BaseTransform.up;

            // Calibrate eyes before unity animator
            for (int i = 0; i < Eyes.Count; i++)
            {
                if (EyeSetups[i].ControlType == EyeSetup.EEyeControlType.Blendshape) continue;
                Eyes[i].localRotation = eyesData[i].initLocalRotation;
            }
        }


        /// <summary>
        /// Executing procedural animation
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (OptimizeWithMesh) if (OptimizeWithMesh.isVisible == false) return;
            if (EyesAnimatorAmount <= 0f) return;

            IsClamping = false;

            Quaternion lookRotationBase = Quaternion.identity;

            if (EyesTarget)
                targetLookPosition = EyesTarget.position + targetLookPositionOffset;
            else
                targetLookPosition = HeadReference.position + BaseTransform.forward * 10f;

            ComputeBaseRotations(ref lookRotationBase);

            if (UseBlinking) if (BlinkingBlend > 0f) UpdateBlinking();


            // Calculations for each eye
            for (int i = 0; i < Eyes.Count; i++)
            {

                #region Additional features calculations like Lag motion and randomize values

                int lagId = 0;
                int randomId = 0;

                if (i == 0)
                {
                    eyesData[i].changeSmoother = Mathf.Lerp(eyesData[i].changeSmoother, 1f, Time.deltaTime * 1f);
                    ComputeLookingRotation(ref lookRotationBase, EyeSetups[i], 0, 0);

                    CalculateLagTimer(0);
                    CalculateRandomTimer(0);
                }
                else
                {
                    if (EyesRandomMovementIndividual)
                    {
                        eyesData[i].changeSmoother = Mathf.Lerp(eyesData[i].changeSmoother, 1f, Time.deltaTime * 1f);
                        ComputeLookingRotation(ref lookRotationBase, EyeSetups[i], i, lagId);
                        CalculateRandomTimer(i);
                        randomId = i;
                    }
                    else
                    {
                        if (IndividualClamping)
                        {
                            ComputeLookingRotation(ref lookRotationBase, EyeSetups[i], 0, lagId);
                        }

                        eyesData[i].changeSmoother = eyesData[0].changeSmoother;
                    }

                    if (IndividualLags)
                    {
                        lagId = i;
                        CalculateLagTimer(i);
                    }
                    else
                    {
                        if( IsClamping ) eyesData[i].SetLagStartRotation( BaseTransform, eyesData[i].lerpRotation );
                        else CalculateLagTimerNonIndividualEvent( i );
                    }
                }

                #endregion


                #region Blendshape animating (iteration continue)

                var eyeSetup = EyeSetups[i];
                if (eyeSetup.ControlType == EyeSetup.EEyeControlType.Blendshape)
                {
                    if (eyeSetup.BlendshapeMesh == null) continue;
                    blendshapeAnglesRequest = true;

                    Vector3 lookDir = Quaternion.Euler( LookDeltaAnglesClamped ) * Vector3.forward;
                    eyeSetup.EyeLeftX(lookDir.x);
                    eyeSetup.EyeRightX(lookDir.x);

                    eyeSetup.EyeUpY(lookDir.y);
                    eyeSetup.EyeDownY(lookDir.y);

                    continue;
                }

                #endregion



                Quaternion initRot = Eyes[i].rotation;

                #region Not squinted rotation (base rotation for this eye)

                eyesData[i].previousLookRotBase = lookRotationBase;
                Quaternion notSquintedRotation = lookRotationBase;

                if (EyesLagAmount > 0f)
                {
                    if (eyesData[lagId].lagProgress > 0f) notSquintedRotation = Quaternion.Slerp(notSquintedRotation, eyesData[i].GetLagStartRotation(BaseTransform), eyesData[lagId].lagProgress * EyesLagAmount);
                }

                //Quaternion notSquintedRotation = lookRotationBase;

                notSquintedRotation *= Quaternion.FromToRotation(eyesData[i].forward, Vector3.forward);
                notSquintedRotation *= eyesData[i].initLocalRotation;

                Eyes[i].rotation = notSquintedRotation;
                Eyes[i].rotation *= Quaternion.Inverse(eyesData[i].initLocalRotation);
                notSquintedRotation = Eyes[i].rotation;

                #endregion


                Quaternion targetEyeLookRotation = notSquintedRotation;

                #region Individual rotation with squint prevent lower than 1

                if (SquintPreventer < 1f)
                {
                    Quaternion individualRotation;
                    Quaternion lookRotationQuatInd;

                         lookRotationQuatInd = Quaternion.LookRotation( targetLookPosition - Eyes[i].position, WorldUp );
                    

                    Vector3 lookRotationInd = lookRotationQuatInd.eulerAngles;

                    if (eyesData[randomId].randomDir != Vector3.zero) lookRotationInd += Vector3.LerpUnclamped(Vector3.zero, eyesData[randomId].randomDir, EyesRandomMovement);

                    // Additional features calculations before clamping
                    Vector2 deltaVectorInd = new Vector3(Mathf.DeltaAngle(lookRotationInd.x, HeadRotation.x), Mathf.DeltaAngle(lookRotationInd.y, HeadRotation.y));

                    if (!IndividualClamping)
                        ClampDetection(deltaVectorInd, ref lookRotationInd, HeadRotation, EyesClampHorizontal, EyesClampVertical);
                    else
                        ClampDetection(deltaVectorInd, ref lookRotationInd, HeadRotation, EyeSetups[i].IndividualClampingHorizontal, EyeSetups[i].IndividualClampingVertical);

                    // Getting clamped rotation
                    individualRotation = Quaternion.Euler(lookRotationInd);

                    individualRotation *= Quaternion.FromToRotation(eyesData[i].forward, Vector3.forward);
                    individualRotation *= eyesData[i].initLocalRotation;

                    Eyes[i].rotation = individualRotation;
                    Eyes[i].rotation *= Quaternion.Inverse(eyesData[i].initLocalRotation);
                    individualRotation = Eyes[i].rotation;

                    targetEyeLookRotation = Quaternion.SlerpUnclamped(individualRotation, notSquintedRotation, SquintPreventer);
                }


                #endregion


                if (CorrectionOffsets[i] != Vector3.zero) targetEyeLookRotation *= Quaternion.Euler(CorrectionOffsets[i]);

                // Eye lag feature if not clamped
                //if (EyesLagAmount > 0f) /*if (!IsClamping) */if (eyesData[lagId].lagProgress > 0f) targetEyeLookRotation = Quaternion.SlerpUnclamped(targetEyeLookRotation, eyesData[i].GetLagStartRotation(BaseTransform), eyesData[lagId].lagProgress * EyesLagAmount);
                //

                eyesSpeedValue = Mathf.LerpUnclamped(2f, 60f, EyesSpeed);

                float deltaLimit = Time.deltaTime * eyesSpeedValue * Mathf.Lerp(1f, eyesData[i].changeSmoother, EyesRandomMovement);
                if (deltaLimit > 1f) deltaLimit = 1f;


                if (EyeSetups[i].ControlType == EyeSetup.EEyeControlType.RotateBone)
                {

                    eyesData[i].lerpRotation = // Rotating eyes towards target rotation with certain speed with lag influence etc.
                        Quaternion.SlerpUnclamped(
                            eyesData[i].lerpRotation, targetEyeLookRotation, // Transitioning towards desired eye rotation
                            deltaLimit);

                    if (GetUseAmountWeight() >= 1f)
                        Eyes[i].rotation = eyesData[i].lerpRotation; // No blending
                    else
                        Eyes[i].rotation = Quaternion.SlerpUnclamped(initRot, eyesData[i].lerpRotation, GetUseAmountWeight()); // Blending with amount controll

                }

            } // End of for for each eye

            changeFlag = false;

            if (UseBlinking) UpdateBlinking();

            UpdateLookAnim(); // Updating look animator implementation (my other package)
            
        }


        /// <summary>
        /// 0f to 1f value of use blend amount
        /// </summary>
        private float GetUseAmountWeight()
        {
#if EYES_LOOKANIMATOR_IMPORTED
            return EyesAnimatorAmount * lookSyncBlend;
#else
            return EyesAnimatorAmount;
#endif
        }


        #region Public handy methods

        /// <summary>
        /// Setting target to look at for eyes with option to offset point of interest
        /// </summary>
        public void SetEyesTarget(Transform target, Vector3? offset = null)
        {
            EyesTarget = target;
            if (offset != null) targetLookPositionOffset = (Vector3)offset; else targetLookPositionOffset = Vector3.zero;
        }

        /// <summary>
        /// Changing blend value of component down to disabled state
        /// </summary>
        public void BlendOutEyesAnimation(float timeInSeconds = 0.5f)
        {
            StopAllCoroutines();
            StartCoroutine(BlendInOut(0f, timeInSeconds));
        }

        /// <summary>
        /// Changing blend value of component up to fully enabled state
        /// </summary>
        public void BlendInEyesAnimation(float timeInSeconds = 0.5f)
        {
            StopAllCoroutines();
            StartCoroutine(BlendInOut(1f, timeInSeconds));
        }

        #endregion


        #region Helper Methods


        private IEnumerator BlendInOut(float blendTo, float time)
        {
            float elapsed = 0f;
            float startVal = EyesAnimatorAmount;

            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                EyesAnimatorAmount = Mathf.Lerp(startVal, blendTo, elapsed / time);

                yield return null;
            }

            EyesAnimatorAmount = blendTo;
            yield break;
        }


        protected virtual void OnValidate()
        {
            UpdateLists();
        }


        public virtual void UpdateLists()
        {
            if (Eyes == null) Eyes = new List<Transform>();
            if (CorrectionOffsets == null) CorrectionOffsets = new List<Vector3>();
            
            if (Eyes.Count != CorrectionOffsets.Count)
            {
                if (CorrectionOffsets.Count > Eyes.Count)
                    for (int i = 0; i < CorrectionOffsets.Count - Eyes.Count; i++) CorrectionOffsets.RemoveAt(CorrectionOffsets.Count - 1);
                else
                {
                    while (CorrectionOffsets.Count < Eyes.Count) CorrectionOffsets.Add(Vector3.zero);
                }
            }

            if (EyeSetups == null) EyeSetups = new List<EyeSetup>();
            AdjustCount(EyeSetups, Eyes.Count);

            // BLINKING
            if (EyeLids == null) EyeLids = new List<Transform>();

            if (UpEyelids != null)
                for (int i = 0; i < UpEyelids.Count; i++)
                    if (!EyeLids.Contains(UpEyelids[i]))
                        EyeLids.Add(UpEyelids[i]);


            if (DownEyelids != null)
                for (int i = 0; i < DownEyelids.Count; i++)
                    if (!EyeLids.Contains(DownEyelids[i])) EyeLids.Add(DownEyelids[i]);


            if (EyeLidsCloseRotations == null) EyeLidsCloseRotations = new List<Vector3>();

            if (EyeLids.Count != EyeLidsCloseRotations.Count)
            {
                EyeLidsCloseRotations.Clear();

                for (int i = 0; i < EyeLids.Count; i++)
                {
                    if (EyeLids[i] == null) continue;
                    EyeLidsCloseRotations.Add(EyeLids[i].localRotation.eulerAngles);
                }
            }

            if (EyeLidsClosePositions == null) EyeLidsClosePositions = new List<Vector3>();
            if (EyeLids.Count != EyeLidsClosePositions.Count)
            {
                EyeLidsClosePositions.Clear();

                for (int i = 0; i < EyeLids.Count; i++)
                {
                    if (EyeLids[i] == null) continue;
                    EyeLidsClosePositions.Add(Vector3.zero);
                }
            }

            if (EyeLidsCloseScales == null) EyeLidsCloseScales = new List<Vector3>();
            if (EyeLids.Count != EyeLidsCloseScales.Count)
            {
                EyeLidsCloseScales.Clear();

                for (int i = 0; i < EyeLids.Count; i++)
                {
                    if (EyeLids[i] == null) continue;
                    EyeLidsCloseScales.Add(EyeLids[i].localScale);
                }
            }

            if( UpEyelidsBlendShapes == null ) UpEyelidsBlendShapes = new List<EyesAnimator_BlenshapesInfo>();
            if( DownEyelidsBlendShapes == null ) DownEyelidsBlendShapes = new List<EyesAnimator_BlenshapesInfo>();
        }


        #endregion


#if EYES_LOOKANIMATOR_IMPORTED

#else
        void StartLookAnim() { }
        void UpdateLookAnim() { }
#endif


        public static void AdjustCount<T>(List<T> list, int targetCount, bool addNulls = false) where T : class, new()
        {
            if (list.Count == targetCount) return;

            if (list.Count < targetCount)
            {
                if (addNulls)
                {
                    while (list.Count < targetCount) list.Add(null);
                }
                else
                {
                    while (list.Count < targetCount) list.Add(new T());
                }
            }
            else
            {
                while (list.Count > targetCount) list.RemoveAt(list.Count - 1);
            }
        }


    }
}