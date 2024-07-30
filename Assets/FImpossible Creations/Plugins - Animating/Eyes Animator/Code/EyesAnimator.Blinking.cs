using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FEyes
{
    public partial class FEyesAnimator
    {
        public bool UseBlinking = false;

        public enum FE_EyesBlinkingMode { Bones, Blendshapes, Bones_Position, Bones_Scale }
        [Tooltip( "If you have bones on eyelids or you want to use blenshapes" )]
        public FE_EyesBlinkingMode BlinkingMode = FE_EyesBlinkingMode.Bones;


        [Tooltip( "Eyelids game objects to animate their rotation" )]
        public List<Transform> EyeLids = new List<Transform>();
        [Tooltip( "Target rotations for eyelids - closed pose" )]
        public List<Vector3> EyeLidsCloseRotations = new List<Vector3>();
        [Tooltip( "Target positions for eyelids - closed pose" )]
        public List<Vector3> EyeLidsClosePositions = new List<Vector3>();
        [Tooltip( "Target scales for eyelids - closed pose" )]
        public List<Vector3> EyeLidsCloseScales = new List<Vector3>();

        [Tooltip( "Put here mesh of which blendshapes have to be animated" )]
        public SkinnedMeshRenderer BlendShapesMesh;
        [Tooltip( "Define which blendshape should be animated and how values should be animated" )]
        public List<EyesAnimator_BlenshapesInfo> BlendShapes;

        public List<EyesAnimator_BlenshapesInfo> UpEyelidsBlendShapes;
        public List<EyesAnimator_BlenshapesInfo> DownEyelidsBlendShapes;

        [Tooltip( "If you would need to smoothly disable blinking animation in some cases" )]
        [FPD_Suffix( 0f, 1f )]
        public float BlinkingBlend = 1f;

        [Tooltip( "Syncing frequency of blinking with random movement preset choosed in 'Animation Settings' Tab" )]
        public bool SyncWithRandomPreset = true;

        [Range( 0.35f, 4f )]
        [Tooltip( "How fast should occur eyelids blinking" )]
        public float BlinkFrequency = 1f;
        [Range( 0.25f, 5f )]
        [Tooltip( "If you want eyelids movement to be slower or quicker" )]
        public float OpenCloseSpeed = 1f;

        [Tooltip( "When you close eyes, for very short moment you keep them closed - higher value - keeping closed duration is longer" )]
        [Range( 0.05f, .5f )]
        public float HoldClosedTime = 0.15f;


        [Tooltip( "If each eye should blink in individual random timers" )]
        public bool IndividualBlinking = false;

        [Tooltip( "If you want simply animate eyelids beeing a bit closed than opened wide for the character. For example you can easily simulate that character is tired." )]
        [Range( 0f, 1f )]
        public float MinOpenValue = 1f;

        // Version 1.0.5
        [Range( 0f, 1.5f )]
        [Tooltip( "When we look up in real life, our eyelids are opening a bit more, when looking down, upper lid is closing a bit and down lid is shifting a little - this option will simulate this behaviour if eyelids are setted properly (subtle effect)" )]
        public float AdditionalEyelidsMotion = 0f;
        [Range( 1f, 1.9f )]
        public Vector2 EyelidsMotionLimit = new Vector2( -1f, 1.5f );
        public List<Transform> UpEyelids = new List<Transform>();
        public List<Transform> DownEyelids = new List<Transform>();
        private List<int> upEyelidsIndexes;
        private List<int> downEyelidsIndexes;
        private List<float> upEyelidsFactor;
        private List<float> downEyelidsFactor;
        private Vector2 deltaV;

        private BlinkTimers[] blinking;

        private Quaternion[] initEyelidsLocalRotations;
        private Vector3[] initEyelidsLocalPositions;
        private Vector3[] initEyesLocalScales;

        private float blinkScale = 1f;

        /// <summary> Supporting original animation for blinkin eyeslids animation </summary>
        public bool BlinkAnimatedEyelids = false;

        void SetupBlinking()
        {
            if( BlinkingMode != FE_EyesBlinkingMode.Blendshapes )
                blinking = new BlinkTimers[EyeLids.Count];
            else
                blinking = new BlinkTimers[BlendShapes.Count];

            initEyelidsLocalRotations = new Quaternion[EyeLids.Count];
            initEyelidsLocalPositions = new Vector3[EyeLids.Count];
            initEyesLocalScales = new Vector3[EyeLids.Count];

            if( BlinkingMode != FE_EyesBlinkingMode.Blendshapes )
            {
                upEyelidsIndexes = new List<int>();
                downEyelidsIndexes = new List<int>();

                upEyelidsFactor = new List<float>();
                downEyelidsFactor = new List<float>();
            }

            for( int i = 0; i < blinking.Length; i++ )
            {
                blinking[i] = new BlinkTimers
                {
                    timer = 1f,
                    power = 1f,
                    keepCloseTime = 0f,
                    progress = 0f
                };

                if( BlinkingMode != FE_EyesBlinkingMode.Blendshapes )
                {
                    initEyelidsLocalRotations[i] = EyeLids[i].localRotation;
                    initEyelidsLocalPositions[i] = EyeLids[i].localPosition;
                    initEyesLocalScales[i] = EyeLids[i].localScale;

                    for( int j = 0; j < UpEyelids.Count; j++ )
                    {
                        if( UpEyelids[j] == EyeLids[i] ) upEyelidsIndexes.Add( i );
                        upEyelidsFactor.Add( 0f );
                    }

                    for( int j = 0; j < DownEyelids.Count; j++ )
                    {
                        if( DownEyelids[j] == EyeLids[i] ) downEyelidsIndexes.Add( i );
                        downEyelidsFactor.Add( 0f );
                    }
                }
            }
        }

        void UpdateBlinking()
        {
            blinkScale = Mathf.Lerp( 0.65f, 1.5f, BlinkFrequency / 3f );

            if( !IndividualBlinking )
            {
                CalculateBlinking();
                CalculateProgress( 0 );

                if( BlinkingMode != FE_EyesBlinkingMode.Blendshapes )
                {
                    for( int i = 0; i < blinking.Length; i++ )
                    {
                        AnimateBlinkingBones( i, 0 );
                    }
                }
                else
                {


                    for( int i = 0; i < blinking.Length; i++ )
                    {
                        AnimateBlinkingBlendshapes( i, 0 );
                    }

                    if( !OutOfRange )
                    {
                        for( int i = 0; i < UpEyelidsBlendShapes.Count; i++ )
                            AnimateBlendshapedAdditionalMotion( UpEyelidsBlendShapes[i], 0, true );

                        for( int i = 0; i < DownEyelidsBlendShapes.Count; i++ )
                            AnimateBlendshapedAdditionalMotion( DownEyelidsBlendShapes[i], 0, false );
                    }
                }
            }
            else
            {
                if( BlinkingMode != FE_EyesBlinkingMode.Blendshapes )
                {
                    for( int i = 0; i < blinking.Length; i++ )
                    {
                        CalculateProgress( i );
                        CalculateBlinking( i );
                        AnimateBlinkingBones( i, i );
                    }
                }
                else
                {
                    for( int i = 0; i < blinking.Length; i++ )
                    {
                        CalculateProgress( i );
                        CalculateBlinking( i );
                        AnimateBlinkingBlendshapes( i, i );
                    }

                    if( !OutOfRange )
                    {
                        for( int i = 0; i < UpEyelidsBlendShapes.Count; i++ )
                            AnimateBlendshapedAdditionalMotion( UpEyelidsBlendShapes[i], i, true );

                        for( int i = 0; i < DownEyelidsBlendShapes.Count; i++ )
                            AnimateBlendshapedAdditionalMotion( DownEyelidsBlendShapes[i], i, false );
                    }
                }
            }

            if( SyncWithRandomPreset )
            {
                if( (int)RandomMovementPreset < 2 )
                    BlinkFrequency = 1f;
                else if( (int)RandomMovementPreset < 5 )
                    BlinkFrequency = 0.65f;
                else if( (int)RandomMovementPreset == 5 )
                    BlinkFrequency = 2.75f;
                else
                    BlinkFrequency = 1.45f;
            }
        }


        private void CalculateBlinking( int i = 0 )
        {
            if( blinking[i].keepCloseTime <= 0f )
            {
                if( blinking[i].progress <= 0f )
                {
                    blinking[i].timer -= Time.deltaTime * BlinkFrequency;
                }
            }

            if( blinking[i].timer <= 0f )
            {
                blinking[i].timer = Random.Range( 0.75f, 1.50f );
                blinking[i].power = Random.Range( 3f * ( blinkScale ), 5f * blinkScale );
                blinking[i].keepCloseTime = HoldClosedTime + HoldClosedTime * Random.Range( 0.0f, 0.125f );
            }
        }

        private void AnimateBlinkingBones( int i, int varInd )
        {
            if( BlinkingMode == FE_EyesBlinkingMode.Bones )
            {
                // Changing opened eyes lids rotation when using UpDownEyelidsFactor
                Quaternion newBackRotation = BlinkAnimatedEyelids ? EyeLids[i].localRotation : initEyelidsLocalRotations[i];
                Quaternion closeRotation = Quaternion.Euler( EyeLidsCloseRotations[i] );

                if( MinOpenValue < 1f ) newBackRotation = Quaternion.Slerp( closeRotation, BlinkAnimatedEyelids ? EyeLids[i].localRotation : initEyelidsLocalRotations[i], MinOpenValue );

                #region V1.0.5 Additional Eyelids Motion

                if( !OutOfRange )
                    if( AdditionalEyelidsMotion > 0f )
                    {
                        blendshapeAnglesRequest = true;
                        Vector2 delta = -LookDeltaAnglesClamped;

                        bool upDowned = false;
                        if( UpEyelids.Count > 0 )
                        {
                            int j = -1;
                            for( int k = 0; k < upEyelidsIndexes.Count; k++ ) if( upEyelidsIndexes[k] == i ) j = k;
                            float targetFactor;

                            if( j >= 0 )
                            {
                                int lagId = 0;
                                if( IndividualLags ) lagId = i;

                                if( delta.x > 1 )
                                {
                                    float openMoreFactor = Mathf.Lerp( 0f, -0.475f * AdditionalEyelidsMotion, Mathf.InverseLerp( 1, EyesClampVertical.y, delta.x ) );

                                    if( EyesLagAmount > 0f )
                                    {
                                        if( eyesData[lagId].lagProgress > 0f ) targetFactor = Mathf.Lerp( openMoreFactor, upEyelidsFactor[j], eyesData[lagId].lagProgress * EyesLagAmount ); else targetFactor = openMoreFactor;
                                    }
                                    else
                                        targetFactor = openMoreFactor;
                                }
                                else
                                    if( delta.x < -1 )
                                {
                                    float closeFactor = Mathf.Lerp( 0f, 0.4f * AdditionalEyelidsMotion, Mathf.InverseLerp( -1, EyesClampVertical.x, delta.x ) );

                                    if( EyesLagAmount > 0f )
                                    {
                                        if( eyesData[lagId].lagProgress > 0f ) targetFactor = Mathf.Lerp( closeFactor, upEyelidsFactor[j], eyesData[lagId].lagProgress * EyesLagAmount ); else targetFactor = closeFactor;
                                    }
                                    else targetFactor = closeFactor;
                                }
                                else
                                {
                                    if( EyesLagAmount > 0f )
                                    {
                                        if( eyesData[lagId].lagProgress > 0f ) targetFactor = Mathf.Lerp( 0f, upEyelidsFactor[j], eyesData[lagId].lagProgress * EyesLagAmount ); else targetFactor = 0f;
                                    }
                                    else targetFactor = 0f;
                                }

                                upEyelidsFactor[j] = Mathf.Lerp( upEyelidsFactor[j], targetFactor, Time.deltaTime * eyesSpeedValue );

                                newBackRotation = Quaternion.SlerpUnclamped( newBackRotation, Quaternion.Euler( EyeLidsCloseRotations[i] ), upEyelidsFactor[j] );

                                upDowned = true;
                            }
                        }

                        if( !upDowned )
                            if( DownEyelids.Count > 0 )
                            {
                                int j = -1;
                                for( int k = 0; k < downEyelidsIndexes.Count; k++ ) if( downEyelidsIndexes[k] == i ) j = k;
                                float targetFactor;

                                if( j >= 0 )
                                {
                                    int lagId = 0;
                                    if( IndividualLags ) lagId = i;

                                    if( delta.x > 1 )
                                    {
                                        float closeFactor = Mathf.Lerp( 0f, 0.3f * AdditionalEyelidsMotion, Mathf.InverseLerp( 1, EyesClampVertical.y, delta.x ) );

                                        if( EyesLagAmount > 0f )
                                        {
                                            if( eyesData[lagId].lagProgress > 0f ) targetFactor = Mathf.Lerp( closeFactor, downEyelidsFactor[j], eyesData[lagId].lagProgress * EyesLagAmount ); else targetFactor = closeFactor;
                                        }
                                        else
                                            targetFactor = closeFactor;
                                    }
                                    else
                                        if( delta.x < -1 )
                                    {
                                        float openMoreFactor = Mathf.Lerp( -1.9f * AdditionalEyelidsMotion, 0f, Mathf.InverseLerp( EyesClampVertical.x, -1, delta.x ) );

                                        if( EyesLagAmount > 0f )
                                        {
                                            if( eyesData[lagId].lagProgress > 0f ) targetFactor = Mathf.Lerp( openMoreFactor, downEyelidsFactor[j], eyesData[lagId].lagProgress * EyesLagAmount ); else targetFactor = openMoreFactor;
                                        }
                                        else
                                            targetFactor = openMoreFactor;
                                    }
                                    else
                                    {
                                        if( EyesLagAmount > 0f )
                                        {
                                            if( eyesData[lagId].lagProgress > 0f ) targetFactor = Mathf.Lerp( 0f, downEyelidsFactor[j], eyesData[lagId].lagProgress * EyesLagAmount ); else targetFactor = 0f;
                                        }
                                        else
                                            targetFactor = 0f;
                                    }

                                    downEyelidsFactor[j] = Mathf.Lerp( downEyelidsFactor[j], targetFactor, Time.deltaTime * eyesSpeedValue );
                                    downEyelidsFactor[j] = Mathf.Clamp( downEyelidsFactor[j], EyelidsMotionLimit.x, EyelidsMotionLimit.y );

                                    newBackRotation = Quaternion.SlerpUnclamped( newBackRotation, Quaternion.Euler( EyeLidsCloseRotations[i] ), downEyelidsFactor[j] );
                                }
                            }
                    }

                #endregion

                EyeLids[i].localRotation = Quaternion.Lerp( newBackRotation, closeRotation, blinking[varInd].progress * BlinkingBlend );
            }
            else if( BlinkingMode == FE_EyesBlinkingMode.Bones_Position )
            {
                EyeLids[i].localPosition = Vector3.Lerp( initEyelidsLocalPositions[i], initEyelidsLocalPositions[i] + EyeLidsClosePositions[i], blinking[varInd].progress * BlinkingBlend );
            }
            else if( BlinkingMode == FE_EyesBlinkingMode.Bones_Scale )
            {
                EyeLids[i].localScale = Vector3.Lerp( initEyesLocalScales[i], EyeLidsCloseScales[i], blinking[varInd].progress * BlinkingBlend );
            }
        }

        private void CalculateProgress( int varInd )
        {
            if( blinking[varInd].keepCloseTime > 0f )
            {
                blinking[varInd].progress += Time.deltaTime * blinking[varInd].power * OpenCloseSpeed * 2.7f;

                if( blinking[varInd].progress >= 1f )
                {
                    blinking[varInd].progress = 1f;
                    blinking[varInd].keepCloseTime -= Time.deltaTime * blinkScale;
                }
            }
            else
            {
                blinking[varInd].progress -= Time.deltaTime * blinking[varInd].power * 0.87f * OpenCloseSpeed * 2.7f;
            }
        }

        private void AnimateBlendshapedAdditionalMotion( EyesAnimator_BlenshapesInfo lid, int lagId, bool upper )
        {
            if( AdditionalEyelidsMotion > 0f )
            {
                blendshapeAnglesRequest = true;
                Vector2 delta = -LookDeltaAnglesClamped;

                if( upper )
                    if( UpEyelidsBlendShapes.Count > 0 )
                    {
                        float targetFactor;


                        if( delta.x > 1 )
                        {
                            float openMoreFactor = Mathf.Lerp( 0f, -0.475f, Mathf.InverseLerp( 1, EyesClampVertical.y, delta.x ) );

                            if( EyesLagAmount > 0f )
                            {
                                if( eyesData[lagId].lagProgress > 0f ) targetFactor = Mathf.Lerp( openMoreFactor, lid.AdditionalFactor, eyesData[lagId].lagProgress * EyesLagAmount ); else targetFactor = openMoreFactor;
                            }
                            else
                                targetFactor = openMoreFactor;
                        }
                        else
                            if( delta.x < -1 )
                        {
                            float closeFactor = Mathf.Lerp( 0f, 0.4f, Mathf.InverseLerp( -1, EyesClampVertical.x, delta.x ) );

                            if( EyesLagAmount > 0f )
                            {
                                if( eyesData[lagId].lagProgress > 0f ) targetFactor = Mathf.Lerp( closeFactor, lid.AdditionalFactor, eyesData[lagId].lagProgress * EyesLagAmount ); else targetFactor = closeFactor;
                            }
                            else targetFactor = closeFactor;
                        }
                        else
                        {
                            if( EyesLagAmount > 0f )
                            {
                                if( eyesData[lagId].lagProgress > 0f ) targetFactor = Mathf.Lerp( 0f, lid.AdditionalFactor, eyesData[lagId].lagProgress * EyesLagAmount ); else targetFactor = 0f;
                            }
                            else targetFactor = 0f;
                        }

                        lid.AdditionalFactor = Mathf.Lerp( lid.AdditionalFactor, targetFactor, Time.deltaTime * 30f * eyesSpeedValue );

                        if( lid.AdditionalFactor < 0f ) lid.AdditionalFactor = 0f;

                        if( lid.Animated ) lid.PreWeight = BlendShapesMesh.GetBlendShapeWeight( lid.ShapeIndex );
                        else lid.PreWeight = 0;

                        float value = Mathf.Lerp( lid.Open * AdditionalEyelidsMotion, lid.Closed * AdditionalEyelidsMotion, lid.AdditionalFactor );
                        value += lid.PreWeight;

                        BlendShapesMesh.SetBlendShapeWeight( lid.ShapeIndex, value );
                    }

                if( !upper )
                    if( DownEyelidsBlendShapes.Count > 0 )
                    {
                        float targetFactor;

                        if( delta.x > 1 )
                        {
                            float closeFactor = Mathf.Lerp( 0f, 0.3f, Mathf.InverseLerp( 1, EyesClampVertical.y, delta.x ) );

                            if( EyesLagAmount > 0f )
                            {
                                if( eyesData[lagId].lagProgress > 0f ) targetFactor = Mathf.Lerp( closeFactor, lid.AdditionalFactor, eyesData[lagId].lagProgress * EyesLagAmount ); else targetFactor = closeFactor;
                            }
                            else
                                targetFactor = closeFactor;
                        }
                        else
                            if( delta.x < -1 )
                        {
                            float openMoreFactor = Mathf.Lerp( -1.9f, 0f, Mathf.InverseLerp( EyesClampVertical.x, -1, delta.x ) );

                            if( EyesLagAmount > 0f )
                            {
                                if( eyesData[lagId].lagProgress > 0f ) targetFactor = Mathf.Lerp( openMoreFactor, lid.AdditionalFactor, eyesData[lagId].lagProgress * EyesLagAmount ); else targetFactor = openMoreFactor;
                            }
                            else
                                targetFactor = openMoreFactor;
                        }
                        else
                        {
                            if( EyesLagAmount > 0f )
                            {
                                if( eyesData[lagId].lagProgress > 0f ) targetFactor = Mathf.Lerp( 0f, lid.AdditionalFactor, eyesData[lagId].lagProgress * EyesLagAmount ); else targetFactor = 0f;
                            }
                            else
                                targetFactor = 0f;
                        }

                        lid.AdditionalFactor = Mathf.Lerp( lid.AdditionalFactor, targetFactor, Time.deltaTime * 30f * eyesSpeedValue );

                        if( lid.Animated ) lid.PreWeight = BlendShapesMesh.GetBlendShapeWeight( lid.ShapeIndex );
                        else lid.PreWeight = 0;

                        float value = Mathf.Lerp( lid.Open * AdditionalEyelidsMotion, lid.Closed * AdditionalEyelidsMotion, lid.AdditionalFactor );
                        value += lid.PreWeight;

                        BlendShapesMesh.SetBlendShapeWeight( lid.ShapeIndex, value );

                    }
            }
        }

        private void AnimateBlinkingBlendshapes( int i, int varInd )
        {
            if( BlendShapes[i].Animated ) BlendShapes[i].PreWeight = BlendShapesMesh.GetBlendShapeWeight( BlendShapes[i].ShapeIndex );
            else BlendShapes[i].PreWeight = 0;

            float value = Mathf.Lerp( BlendShapes[i].Open, BlendShapes[i].Closed, blinking[varInd].progress * BlinkingBlend );

            value += BlendShapes[i].PreWeight;

            if( BlendShapes[i].MaxToClose ) if( value > BlendShapes[i].Closed ) value = BlendShapes[i].Closed;

            if( BlendShapes[i].UseOtherShape )
                BlendShapes[i].UseOtherShape.SetBlendShapeWeight( BlendShapes[i].ShapeIndex, value );
            else
                BlendShapesMesh.SetBlendShapeWeight( BlendShapes[i].ShapeIndex, value );
        }



        private class BlinkTimers
        {
            public float timer = 0f;
            public float power = 0f;
            public float keepCloseTime = 0f;
            public float progress = 0f;
        }



        [System.Serializable]
        public class EyesAnimator_BlenshapesInfo
        {
            public EyesAnimator_BlenshapesInfo( int initIndex )
            {
                ShapeIndex = initIndex;
            }

            public int ShapeIndex;
            public float Open = 0f;
            public float Closed = 100f;
            public float PreWeight = 0f;
            [Tooltip( "If this blendshape is animated and you want use blinking as additive motion, toggle this" )]
            public bool Animated = false;
            [Tooltip( "If maximum value for blendshape should be close value" )]
            public bool MaxToClose = false;

            // For additional eyelids motion
            public float InitVal = 0f;
            public float AdditionalFactor = 0f;

            [Tooltip( "If your character is using multiple meshes for animating blinking" )]
            public SkinnedMeshRenderer UseOtherShape = null;
        }
    }
}