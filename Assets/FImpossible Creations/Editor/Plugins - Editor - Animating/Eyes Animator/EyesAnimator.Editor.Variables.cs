using FIMSpace.FEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class FEyesAnimator_Editor : UnityEditor.Editor
{

    protected static bool drawDefaultInspector = false;

    protected bool drawEyesTarget = true;
    protected bool drawRanges = true;

    static bool showEyes = true;
    static bool showClampingAndOthers = false;

    float bothX = 70f;
    float lastBothX = 70f;
    float bothY = 60f;
    float lastBothY = 60f;

    protected SerializedProperty sp_Base;
    protected SerializedProperty sp_HeadReference;
    protected SerializedProperty sp_StartLookOffset;
    protected SerializedProperty sp_EyesTarget;
    protected SerializedProperty sp_Eyes;
    protected SerializedProperty sp_EyesSpeed;
    protected SerializedProperty sp_FollowAmount;
    protected SerializedProperty sp_EyesBlend;
    protected SerializedProperty sp_EyesRandomMovement;
    protected SerializedProperty sp_RandomMovementAxisScale;
    protected SerializedProperty sp_RandomMovementPreset;
    protected SerializedProperty sp_RandomizingSpeed;
    protected SerializedProperty sp_EyesRandomMovementIndividual;
    protected SerializedProperty sp_EyesLagAmount;
    protected SerializedProperty sp_IndividualLags;
    protected SerializedProperty sp_StopLookAbove;
    protected SerializedProperty sp_BlendTransitionSpeed;
    protected SerializedProperty sp_EyesClampHorizontal;
    protected SerializedProperty sp_EyesClampVertical;
    protected SerializedProperty sp_SquintPreventer;
    protected SerializedProperty sp_CorrectionOffsets;
    protected SerializedProperty sp_EyesMaxDistance;
    protected SerializedProperty sp_DetectionFactor;
    protected SerializedProperty sp_LagStiffness;
    protected SerializedProperty sp_OptimizeWithMesh;
    protected SerializedProperty sp_IndClamp;

    protected virtual void OnEnable()
    {
        sp_Base = serializedObject.FindProperty("_baseTransform");
        sp_HeadReference = serializedObject.FindProperty("HeadReference");
        sp_StartLookOffset = serializedObject.FindProperty("StartLookOffset");
        sp_EyesTarget = serializedObject.FindProperty("EyesTarget");
        sp_Eyes = serializedObject.FindProperty("Eyes");
        sp_EyesSpeed = serializedObject.FindProperty("EyesSpeed");
        sp_FollowAmount = serializedObject.FindProperty("FollowTargetAmount");
        sp_EyesBlend = serializedObject.FindProperty("EyesAnimatorAmount");
        sp_EyesRandomMovement = serializedObject.FindProperty("EyesRandomMovement");
        sp_RandomMovementAxisScale = serializedObject.FindProperty("RandomMovementAxisScale");
        sp_RandomMovementPreset = serializedObject.FindProperty("RandomMovementPreset");
        sp_RandomizingSpeed = serializedObject.FindProperty("RandomizingSpeed");
        sp_EyesRandomMovementIndividual = serializedObject.FindProperty("EyesRandomMovementIndividual");
        sp_EyesLagAmount = serializedObject.FindProperty("EyesLagAmount");
        sp_IndividualLags = serializedObject.FindProperty("IndividualLags");
        sp_StopLookAbove = serializedObject.FindProperty("StopLookAbove");
        sp_BlendTransitionSpeed = serializedObject.FindProperty("BlendTransitionSpeed");
        sp_EyesClampHorizontal = serializedObject.FindProperty("EyesClampHorizontal");
        sp_EyesClampVertical = serializedObject.FindProperty("EyesClampVertical");
        sp_SquintPreventer = serializedObject.FindProperty("SquintPreventer");
        sp_EyesClampVertical = serializedObject.FindProperty("EyesClampVertical");
        sp_CorrectionOffsets = serializedObject.FindProperty("CorrectionOffsets");
        sp_EyesMaxDistance = serializedObject.FindProperty("MaxTargetDistance");
        sp_DetectionFactor = serializedObject.FindProperty("GoOutFactor");
        sp_LagStiffness = serializedObject.FindProperty("LagStiffness");
        sp_OptimizeWithMesh = serializedObject.FindProperty("OptimizeWithMesh");
        sp_IndClamp = serializedObject.FindProperty("IndividualClamping");

        InitBlinking();

        FindComponents();

        drawSetup = false;
        showEyes = false;

        if (Get.Eyes == null || Get.Eyes.Count == 0)
        {
            showEyes = true;
            drawSetup = true;
        }
        else if (!Get.EyesTarget)
        {
            drawSetup = true;
        }

        showEyelids = false;
        drawBlinkingSetup = false;
        if (Get.BlinkingMode == FIMSpace.FEyes.FEyesAnimator.FE_EyesBlinkingMode.Bones)
        {
            if (Get.EyeLids == null || Get.EyeLids.Count == 0)
            {
                showEyelids = true;
                drawBlinkingSetup = true;
            }
        }
        else
        {
            if (Get.BlendShapes == null || Get.BlendShapes.Count == 0)
            {
                showEyelids = true;
                drawBlinkingSetup = true;
            }
        }

        // For Look Animator Implementation
        InitSyncVariables();
    }



    private void OnDisable()
    {
        if (focusCloseRotations)
        {
            if (openRotations != null)
            {
                for (int i = 0; i < Get.EyeLids.Count; i++)
                    if (Get.EyeLids[i] != null) Get.EyeLids[i].localRotation = openRotations[i];
            }

            if (openPositions != null)
            {
                for (int i = 0; i < Get.EyeLids.Count; i++)
                    if (Get.EyeLids[i] != null) Get.EyeLids[i].localPosition = openPositions[i];
            }
        }

        focusCloseRotations = false;
        if (openRotations != null) openRotations.Clear();
    }

    private void OnSceneGUI()
    {
        if (focusCloseRotations)
        {
            if (openRotations != null)
            {
                if (Get.EyeLids != null)
                    for (int i = 0; i < Get.EyeLids.Count; i++)
                        if (Get.EyeLids[i] != null) Get.EyeLids[i].localRotation = Quaternion.Euler(Get.EyeLidsCloseRotations[i]);
            }

            if (openPositions != null)
            {
                if (Get.EyeLids != null)
                    for (int i = 0; i < Get.EyeLids.Count; i++)
                        if (Get.EyeLids[i] != null) Get.EyeLids[i].localPosition = (openPositions[i] + Get.EyeLidsClosePositions[i]);
            }

            if (openScales != null)
            {
                if (Get.EyeLids != null)
                    for (int i = 0; i < Get.EyeLids.Count; i++)
                        if (Get.EyeLids[i] != null) Get.EyeLids[i].localScale = (Get.EyeLidsCloseScales[i] );
            }
        }
    }
}
