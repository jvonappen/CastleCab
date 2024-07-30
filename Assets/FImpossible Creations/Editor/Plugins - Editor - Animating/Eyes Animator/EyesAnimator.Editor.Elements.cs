using FIMSpace.FEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class FEyesAnimator_Editor : UnityEditor.Editor
{

    void El_DrawOptimizeWithMesh()
    {
        // Drawing box informing if spine animator is working by mesh visibility factor
        if (Get.OptimizeWithMesh)
        {
            if (Application.isPlaying)
            {
                GUI.color = new Color(1f, 1f, 1f, .5f);
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

                if (Get.OptimizeWithMesh.isVisible)
                    EditorGUILayout.LabelField("Spine Animator Is Active", FGUI_Resources.HeaderStyle);
                else
                {
                    GUI.enabled = false;
                    EditorGUILayout.LabelField("Spine Animator Is Inactive", FGUI_Resources.HeaderStyle);
                    GUI.enabled = true;
                }

                EditorGUILayout.EndHorizontal();
                GUI.color = c;
            }
        }


        EditorGUILayout.BeginHorizontal();

        EditorGUIUtility.labelWidth = 144;
        EditorGUILayout.PropertyField(sp_OptimizeWithMesh);
        EditorGUIUtility.labelWidth = 0;

        if (Get.OptimizeWithMesh == null)
        {
            if (GUILayout.Button("Find", new GUILayoutOption[1] { GUILayout.Width(44) }))
            {
                if (Get.OptimizeWithMesh == null)
                {
                    Get.OptimizeWithMesh = Get.transform.GetComponent<Renderer>();
                    if (!Get.OptimizeWithMesh) Get.OptimizeWithMesh = Get.transform.GetComponentInChildren<Renderer>();
                    if (!Get.OptimizeWithMesh) if (Get.transform.parent != null) Get.OptimizeWithMesh = Get.transform.parent.GetComponentInChildren<Renderer>();
                    if (!Get.OptimizeWithMesh) if (Get.transform.parent != null) if (Get.transform.parent.parent != null) Get.OptimizeWithMesh = Get.transform.parent.parent.GetComponentInChildren<Renderer>();
                    if (!Get.OptimizeWithMesh) if (Get.transform.parent != null) if (Get.transform.parent.parent != null) if (Get.transform.parent.parent.parent != null) Get.OptimizeWithMesh = Get.transform.parent.parent.parent.GetComponentInChildren<Renderer>();
                }
            }
        }


        EditorGUILayout.EndHorizontal();
    }


    void El_DrawMaxDist()
    {
        if (Get.MaxTargetDistance > 0f)
        {
            GUILayout.BeginHorizontal();

            EditorGUIUtility.labelWidth = 134;
            EditorGUILayout.PropertyField(sp_EyesMaxDistance);
            EditorGUIUtility.labelWidth = 176;

            if (!Get._gizmosDrawMaxDist || !Get.DrawGizmos)
            {
                if (GUILayout.Button(new GUIContent(FGUI_Resources.Tex_GizmosOff, "Check scene view for sphere gizmos showing max distance and other ranges in world space"), FGUI_Resources.ButtonStyle, new GUILayoutOption[] { GUILayout.Width(20f), GUILayout.Height(16f) })) { Get._gizmosDrawMaxDist = !Get._gizmosDrawMaxDist; }
            }
            else
                if (GUILayout.Button(new GUIContent(FGUI_Resources.Tex_Gizmos, "Check scene view for sphere gizmos showing max distance and other ranges in world space"), FGUI_Resources.ButtonStyle, new GUILayoutOption[] { GUILayout.Width(20f), GUILayout.Height(16f) })) { Get._gizmosDrawMaxDist = !Get._gizmosDrawMaxDist; }

            GUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(sp_DetectionFactor);
        }
        else
        {
            GUILayout.BeginHorizontal();

            GUI.color = c;
            EditorGUIUtility.labelWidth = 134;
            EditorGUILayout.PropertyField(sp_EyesMaxDistance);
            EditorGUILayout.LabelField("(Not Used)", new GUILayoutOption[] { GUILayout.Width(70f) });
            EditorGUIUtility.labelWidth = 0;
            GUI.color = c;

            GUILayout.EndHorizontal();
        }

        if (Get.MaxTargetDistance < 0f)
        {
            Get.MaxTargetDistance = 0f;
            EditorUtility.SetDirty(target);
        }

        GUI.color = c;
    }


    private Color grayStyle = new Color(0.4f, 0.4f, 0.4f, 0.2f);
    void El_DrawClamping()
    {
        bool wrongLimit = false;
        if (Mathf.Abs(Get.EyesClampHorizontal.x) > Get.StopLookAbove) wrongLimit = true;
        if (Mathf.Abs(Get.EyesClampHorizontal.y) > Get.StopLookAbove) wrongLimit = true;

        if (!wrongLimit)
            GUI.color = c;
        else
            GUI.color = new Color(0.9f, 0.55f, 0.55f, 0.8f);

        GUILayout.Space(8);

        GUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 120;
        EditorGUILayout.PropertyField(sp_StopLookAbove);
        EditorGUIUtility.labelWidth = 0;

        GUILayout.BeginHorizontal(GUILayout.Width(30));
        FEditor_CustomInspectorHelpers.DrawMinMaxSphere(-Get.StopLookAbove, Get.StopLookAbove, 14); GUILayout.EndHorizontal();

        GUILayout.EndHorizontal();
        GUI.color = c;

        EditorGUILayout.PropertyField(sp_IndClamp);
        GUILayout.Space(4f);


        if (!sp_IndClamp.boolValue)
        {

            #region Clamping angles

            if (!wrongLimit) GUI.color = new Color(0.55f, 0.9f, 0.75f, 0.8f); else GUI.color = new Color(0.9f, 0.55f, 0.55f, 0.8f);
            GUILayout.EndVertical(); ///

            GUILayout.BeginVertical(FGUI_Resources.BGInBoxLightStyle); ////
            GUILayout.Space(3f);
            GUILayout.BeginHorizontal();
            GUILayout.Label("  Clamp Angle Horizontal (X)", GUILayout.MaxWidth(170f));
            GUILayout.FlexibleSpace();
            GUILayout.Label(Mathf.Round(Get.EyesClampHorizontal.x) + "°", FGUI_Inspector.Style(grayStyle), GUILayout.MaxWidth(40f));
            FEditor_CustomInspectorHelpers.DrawMinMaxSphere(Get.EyesClampHorizontal.x, Get.EyesClampHorizontal.y, 14, 0f);
            GUILayout.Label(Mathf.Round(Get.EyesClampHorizontal.y) + "°", FGUI_Inspector.Style(grayStyle), GUILayout.MaxWidth(40f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUILayout.MinMaxSlider(ref Get.EyesClampHorizontal.x, ref Get.EyesClampHorizontal.y, -180f, 180f);
            bothX = EditorGUILayout.Slider("Adjust symmetrical", bothX, 1f, 180f);

            if (lastBothX != bothX)
            {
                Get.EyesClampHorizontal.x = -bothX;
                Get.EyesClampHorizontal.y = bothX;
                lastBothX = bothX;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }

            GUI.color = c;
            GUILayout.Space(3f);
            GUILayout.EndVertical(); ///


            GUI.color = new Color(0.6f, 0.75f, 0.9f, 0.8f);

            GUILayout.BeginVertical(FGUI_Resources.BGInBoxStyle); ////
            GUILayout.Space(3f);
            GUILayout.BeginHorizontal();
            GUILayout.Label("  Clamp Angle Vertical (Y)", GUILayout.MaxWidth(170f));
            GUILayout.FlexibleSpace();
            GUILayout.Label(Mathf.Round(Get.EyesClampVertical.x) + "°", FGUI_Inspector.Style(grayStyle), GUILayout.MaxWidth(40f));
            FEditor_CustomInspectorHelpers.DrawMinMaxVertSphere(-Get.EyesClampVertical.y, -Get.EyesClampVertical.x, 14, 0f);
            GUILayout.Label(Mathf.Round(Get.EyesClampVertical.y) + "°", FGUI_Inspector.Style(grayStyle), GUILayout.MaxWidth(40f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUILayout.MinMaxSlider(ref Get.EyesClampVertical.x, ref Get.EyesClampVertical.y, -90f, 90f);
            bothY = EditorGUILayout.Slider("Adjust symmetrical", bothY, 1f, 90f);

            if (lastBothY != bothY)
            {
                Get.EyesClampVertical.x = -bothY;
                Get.EyesClampVertical.y = bothY;
                lastBothY = bothY;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }

            GUI.color = c;

            #endregion


        }
        else
        {
            string selEye = "No Eye Selected";
            if (_indivClampEye > -1) selEye = "Eye [" + _indivClampEye + "]";
            if (GUILayout.Button(selEye, EditorStyles.layerMaskField))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("None"), _indivClampEye == -1, () => { _indivClampEye = -1; });
                for (int i = 0; i < Get.EyeSetups.Count; i++)
                {
                    int eIndex = i;
                    menu.AddItem(new GUIContent("Eye [" + i + "]"), _indivClampEye == eIndex, () => { _indivClampEye = eIndex; });
                }
                menu.ShowAsContext();
            }

            if ( _indivClampEye > -1)
            if ( _indivClampEye < Get.EyeSetups.Count)
                {
                    DrawIndividualClampingForEye(Get.EyeSetups[_indivClampEye]);
                }
            else
                {
                    _indivClampEye = -1;
                }

        }

        GUILayout.Space(4f);
    }

    int _indivClampEye = -1;
    void DrawIndividualClampingForEye(FIMSpace.FEyes.FEyesAnimator.EyeSetup eye)
    {
        EditorGUI.BeginChangeCheck();

        GUILayout.BeginVertical(FGUI_Resources.BGInBoxLightStyle); ////
        GUILayout.Space(3f);
        GUILayout.BeginHorizontal();
        GUILayout.Label("  Clamp Angle Horizontal (X)", GUILayout.MaxWidth(170f));
        GUILayout.FlexibleSpace();
        GUILayout.Label(Mathf.Round(eye.IndividualClampingHorizontal.x) + "°", FGUI_Inspector.Style(grayStyle), GUILayout.MaxWidth(40f));
        FEditor_CustomInspectorHelpers.DrawMinMaxSphere(eye.IndividualClampingHorizontal.x, eye.IndividualClampingHorizontal.y, 14, 0f);
        GUILayout.Label(Mathf.Round(eye.IndividualClampingHorizontal.y) + "°", FGUI_Inspector.Style(grayStyle), GUILayout.MaxWidth(40f));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical(); ///
        EditorGUILayout.MinMaxSlider(ref eye.IndividualClampingHorizontal.x, ref eye.IndividualClampingHorizontal.y, -180f, 180f);


        GUI.color = new Color(0.6f, 0.75f, 0.9f, 0.8f);

        GUILayout.BeginVertical(FGUI_Resources.BGInBoxStyle); ////
        GUILayout.Space(3f);
        GUILayout.BeginHorizontal();
        GUILayout.Label("  Clamp Angle Vertical (Y)", GUILayout.MaxWidth(170f));
        GUILayout.FlexibleSpace();
        GUILayout.Label(Mathf.Round(eye.IndividualClampingVertical.x) + "°", FGUI_Inspector.Style(grayStyle), GUILayout.MaxWidth(40f));
        FEditor_CustomInspectorHelpers.DrawMinMaxVertSphere(-eye.IndividualClampingVertical.y, -eye.IndividualClampingVertical.x, 14, 0f);
        GUILayout.Label(Mathf.Round(eye.IndividualClampingVertical.y) + "°", FGUI_Inspector.Style(grayStyle), GUILayout.MaxWidth(40f));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical(); ///
        EditorGUILayout.MinMaxSlider(ref eye.IndividualClampingVertical.x, ref eye.IndividualClampingVertical.y, -180f, 180f);

        GUI.color = c;

        if ( EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(Get);
        }
    }

}
