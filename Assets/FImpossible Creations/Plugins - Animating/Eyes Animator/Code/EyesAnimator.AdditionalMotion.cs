using UnityEngine;

namespace FIMSpace.FEyes
{
    public partial class FEyesAnimator
    {
        private bool changeFlag = true;


        /// <summary>
        /// Handling eye lag simulation
        /// </summary>
        private void CalculateLagTimer(int i)
        {
            eyesData[i].lagTimer -= Time.deltaTime / LagStiffness;

            if (IsClamping)
            {
                eyesData[i].SetLagStartRotation(BaseTransform, eyesData[i].lerpRotation);
            }
            else
            {
                if (eyesData[i].lagProgress > 0)
                {
                    if (eyesData[i].lagTimer < 0f)
                    {
                        eyesData[i].lagProgress -= Random.Range(0.4f, 0.85f) * 0.7f * LagStiffness;
                    }
                }
                else
                {
                    if (eyesData[i].lagProgress <= 0)
                    {
                        if (eyesData[i].lagTimer < 0f)
                        {
                            eyesData[i].lagProgress = 1f;
                            eyesData[i].SetLagStartRotation(BaseTransform, eyesData[i].lerpRotation);
                            changeFlag = true;
                        }
                    }
                }
            }

            if (eyesData[i].lagTimer < 0f) eyesData[i].lagTimer = Random.Range(0.15f, 0.34f);
        }



        private void CalculateLagTimerNonIndividualEvent(int i)
        {
            if (changeFlag) eyesData[i].SetLagStartRotation(BaseTransform, eyesData[i].lerpRotation);
        }




        /// <summary>
        /// Handling random eye movement simulation
        /// </summary>
        private void CalculateRandomTimer(int i)
        {
            eyesData[i].randomTimer -= Time.deltaTime * RandomizingSpeed;

            if (eyesData[i].randomTimer < 0f)
            {
                // If random rotation is directed away right now, we want go it back to center a bit
                if (eyesData[i].randomDir.magnitude > (EyesClampHorizontal.magnitude + EyesClampVertical.magnitude) / 8)
                {
                    float range = 5f;
                    eyesData[i].randomDir = new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0f);
                    eyesData[i].randomDir = Vector2.Scale(eyesData[i].randomDir, RandomMovementAxisScale);

                    switch (RandomMovementPreset)
                    {
                        case FERandomMovementType.Default:
                        case FERandomMovementType.Listening:
                        case FERandomMovementType.Calm:
                        case FERandomMovementType.Focused:
                            eyesData[i].randomTimer = Random.Range(0.9f, 2.4f);
                            break;
                        case FERandomMovementType.Nervous:
                            eyesData[i].randomTimer = Random.Range(0.2f, 0.6f);
                            break;
                        case FERandomMovementType.AccessingImaginedVisual:
                        case FERandomMovementType.AccessingImaginedAuditory:
                        case FERandomMovementType.AccessingFeelings:
                        case FERandomMovementType.AccessingVisualMemory:
                        case FERandomMovementType.AccessingAuditoryMemory:
                        case FERandomMovementType.AccessingInternalSelfTalk:
                            eyesData[i].randomTimer = Random.Range(0.6f, 0.9f);
                            break;
                    }
                }
                else
                {
                    switch (RandomMovementPreset)
                    {
                        case FERandomMovementType.Default:
                            eyesData[i].randomTimer = Random.Range(0.4f, 1.24f);
                            eyesData[i].randomDir = new Vector3(Random.Range(-28, 28), Random.Range(-28, 28), 0f);
                            break;
                        case FERandomMovementType.Listening:
                            eyesData[i].randomTimer = Random.Range(0.4f, 1.24f);
                            eyesData[i].randomDir = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0f);
                            break;
                        case FERandomMovementType.Calm:
                            eyesData[i].randomTimer = Random.Range(1.11f, 2.2f);
                            eyesData[i].randomDir = new Vector3(Random.Range(-25, 25), Random.Range(-25, 25), 0f);
                            break;
                        case FERandomMovementType.Focused:
                            eyesData[i].randomTimer = Random.Range(1.3f, 3.2f);
                            eyesData[i].randomDir = new Vector3(Random.Range(-30, 30), Random.Range(-30, 30), 0f);
                            break;
                        case FERandomMovementType.Nervous:
                            eyesData[i].randomTimer = Random.Range(0.165f, 0.34f);
                            eyesData[i].randomDir = new Vector3(Random.Range(-24, 24), Random.Range(-24, 24), 0f);
                            break;

                        case FERandomMovementType.AccessingImaginedVisual:
                            eyesData[i].randomTimer = Random.Range(0.45f, 1.7f);
                            eyesData[i].randomDir = new Vector3(Random.Range(-50, -35), Random.Range(40, 60), 0f);
                            break;
                        case FERandomMovementType.AccessingImaginedAuditory:
                            eyesData[i].randomTimer = Random.Range(0.45f, 1.7f);
                            eyesData[i].randomDir = new Vector3(Random.Range(-3, 3), Random.Range(40, 60), 0f);
                            break;
                        case FERandomMovementType.AccessingFeelings:
                            eyesData[i].randomTimer = Random.Range(0.45f, 1.7f);
                            eyesData[i].randomDir = new Vector3(Random.Range(30, 40), Random.Range(40, 60), 0f);
                            break;

                        case FERandomMovementType.AccessingVisualMemory:
                            eyesData[i].randomTimer = Random.Range(0.45f, 1.7f);
                            eyesData[i].randomDir = new Vector3(Random.Range(-60, -40), Random.Range(-60, -40), 0f);
                            break;
                        case FERandomMovementType.AccessingAuditoryMemory:
                            eyesData[i].randomTimer = Random.Range(0.45f, 1.7f);
                            eyesData[i].randomDir = new Vector3(Random.Range(-3, 3), Random.Range(-60, -40), 0f);
                            break;
                        case FERandomMovementType.AccessingInternalSelfTalk:
                            eyesData[i].randomTimer = Random.Range(0.45f, 1.7f);
                            eyesData[i].randomDir = new Vector3(Random.Range(40, 60), Random.Range(-60, -40), 0f);
                            break;
                    }

                    eyesData[i].randomDir = Vector2.Scale(eyesData[i].randomDir, RandomMovementAxisScale);
                }

                // Smoothing a little speed for eye when new rotation is choosed
                float mul = Mathf.Lerp(0.4f, 1.3f, EyesLagAmount);
                eyesData[i].changeSmoother = Random.Range(0.5f, 0.85f) * mul;
            }
        }


        public enum FERandomMovementType
        {
            ///<summary> Random but not too quick movement for eyes </summary>
            Default = 0,
            ///<summary> Small calm movements </summary>
            Listening = 1,
            ///<summary> Rare long random moves for eyes </summary>
            Calm = 2,
            ///<summary> Rare medium random moves for eyes </summary>
            Focused = 3,
            ///<summary> Quick and short random movement for eyes </summary>
            Nervous = 4,
            ///<summary> (Right Up) When someone is imagining something visual, he can be lying right now </summary>
            AccessingImaginedVisual = 5,
            ///<summary> (Right) When someone is imagining something related to audio, he can be lying right now </summary>
            AccessingImaginedAuditory = 6,
            ///<summary> (Right Down) When someone is recalling / imagining emotion </summary>
            AccessingFeelings = 7,
            ///<summary> (Left Up) When someone is remembering image or scene </summary>
            AccessingVisualMemory = 8,
            ///<summary> (Left) When someone is remembering something heard before </summary>
            AccessingAuditoryMemory = 9,
            ///<summary> (Left Down) When someone is talking to himself inside </summary>
            AccessingInternalSelfTalk = 10
        }

    }
}