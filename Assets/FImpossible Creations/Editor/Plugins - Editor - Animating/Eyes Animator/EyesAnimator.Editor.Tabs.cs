using FIMSpace.FEditor;
using System;
using UnityEditor;
using UnityEngine;

public partial class FEyesAnimator_Editor : UnityEditor.Editor
{
    //bool wrongLimit = false;
    bool drawAdditionalModules = false;

    void Tab_AdditionalModules()
    {
        GUI.color = c;
        FGUI_Inspector.VSpace(-2, -4);
        GUILayout.BeginVertical(FGUI_Resources.ViewBoxStyle);
        GUILayout.Space(2);

        EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxBlankStyle);
        if (GUILayout.Button(new GUIContent("  " + FGUI_Resources.GetFoldSimbol(showClampingAndOthers, 10, "►") + "  Clamping Look Angles", FGUI_Resources.Tex_Knob), FGUI_Resources.FoldStyle, GUILayout.Height(22))) showClampingAndOthers = !showClampingAndOthers;

        if (showClampingAndOthers)
        {
            El_DrawClamping();
        }

        EditorGUILayout.EndVertical();


        FGUI_Inspector.FoldSwitchableHeaderStart(ref Get.UseBlinking, ref drawBlinking, "Blinking Module", FGUI_Resources.BGInBoxStyle, _TexBlinkIcon);

        if (drawBlinking && Get.UseBlinking)
        {
            DrawBlikningModule();
        }

        GUILayout.Space(2);
        GUILayout.EndVertical();
        GUILayout.EndVertical();
        GUILayout.Space(-4);
    }


    static bool drawTweakingAnimationSettings = true;
    //static bool drawConditions = true;
    static bool drawFollowSettings = true;
    void Tab_Tweaking()
    {
        GUI.color = c;
        FGUI_Inspector.VSpace(-2, -4);
        GUILayout.BeginVertical(FGUI_Resources.ViewBoxStyle);
        GUILayout.Space(2);

        EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxBlankStyle);
        EditorGUIUtility.labelWidth = 160; EditorGUILayout.PropertyField(sp_EyesBlend); EditorGUIUtility.labelWidth = 140;
        GUILayout.Space(3);
        EditorGUILayout.EndVertical();

        //EditorGUILayout.EndVertical();

        FGUI_Inspector.FoldHeaderStart(ref drawFollowSettings, "Eyes Move and Follow Settings", FGUI_Resources.BGInBoxStyle, _TexEyeFollowIcon);

        if (drawFollowSettings)
        {
            GUILayout.Space(6);

            EditorGUILayout.PropertyField(sp_FollowAmount);

            if (drawEyesTarget)
            {
                EditorGUILayout.PropertyField(sp_EyesTarget, new GUIContent(" " + sp_EyesTarget.displayName, FGUI_Resources.TexTargetingIcon, sp_EyesTarget.tooltip), true);
                GUILayout.Space(2f);
            }

            GUILayout.Space(3f);
            EditorGUILayout.PropertyField(sp_EyesSpeed);
            EditorGUILayout.PropertyField(sp_SquintPreventer);
            EditorGUIUtility.labelWidth = 0;

            GUILayout.Space(3);
            El_DrawMaxDist();

            GUILayout.Space(3);
        }

        EditorGUILayout.EndVertical();
        //EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxBlankStyle);





        FGUI_Inspector.FoldHeaderStart(ref drawRandomSettings, "Eyes Random Movement Settings", FGUI_Resources.BGInBoxLightStyle, _TexEyeRandomIcon);

        if (drawRandomSettings)
        {
            GUILayout.Space(3);

            if (Get.EyesRandomMovement > 0f)
            {
                EditorGUIUtility.labelWidth = 100;
                EditorGUILayout.PropertyField(sp_EyesRandomMovement, new GUIContent("Amount", "Random movement for eyes in addition to current direction - you can crank it up for example when there is no target for eyes, or when character is talking with someone"));
                EditorGUILayout.PropertyField(sp_RandomMovementAxisScale, new GUIContent("Scale"));
                EditorGUILayout.PropertyField(sp_RandomMovementPreset, new GUIContent("Preset"));
                EditorGUILayout.PropertyField(sp_RandomizingSpeed, new GUIContent("Frequency"));
                EditorGUILayout.PropertyField(sp_EyesRandomMovementIndividual, new GUIContent("Individual", "Option for monsters, each eye will have individual random rotation direction"));
                EditorGUIUtility.labelWidth = 0;
            }
            else
            {
                GUILayout.BeginHorizontal();

                GUI.color = c;
                EditorGUIUtility.labelWidth = 100;
                EditorGUILayout.PropertyField(sp_EyesRandomMovement, new GUIContent("Amount", "Random movement for eyes in addition to current direction - you can crank it up for example when there is no target for eyes, or when character is talking with someone"));
                EditorGUILayout.LabelField("(Not Used)", new GUILayoutOption[] { GUILayout.Width(70f) });
                EditorGUIUtility.labelWidth = 0;
                GUI.color = c;

                GUILayout.EndHorizontal();

            }

            GUILayout.Space(3);
        }

        EditorGUILayout.EndVertical();


        FGUI_Inspector.FoldHeaderStart(ref drawLagSettings, "Eyes Lag Effect Settings", FGUI_Resources.BGInBoxStyle, FGUI_Resources.TexMotionIcon);

        if (drawLagSettings)
        {

            if (Get.EyesLagAmount > 0)
            {
                EditorGUIUtility.labelWidth = 110;
                EditorGUILayout.PropertyField(sp_EyesLagAmount, new GUIContent("Lag Amount", "When we rotate eyes, they're reaching target with kinda jumpy movement depending of point of interest, but for more toon/not real effect you can left this value at 0"));
                EditorGUILayout.PropertyField(sp_LagStiffness);
                EditorGUILayout.PropertyField(sp_IndividualLags, new GUIContent("Individual", "Option for monsters, each eye will have individual random delay for movement"));
                EditorGUIUtility.labelWidth = 0;
            }
            else
            {
                GUILayout.BeginHorizontal();

                GUI.color = c;
                EditorGUIUtility.labelWidth = 110;
                EditorGUILayout.PropertyField(sp_EyesLagAmount, new GUIContent("Lag Amount", "When we rotate eyes, they're reaching target with kinda jumpy movement depending of point of interest, but for more toon/not real effect you can left this value at 0"));
                EditorGUILayout.LabelField("(Not Used)", new GUILayoutOption[] { GUILayout.Width(70f) });
                EditorGUIUtility.labelWidth = 0;
                GUI.color = c;

                GUILayout.EndHorizontal();
            }

            GUILayout.Space(3);
        }

        EditorGUILayout.EndVertical();



        //FGUI_Inspector.FoldHeaderStart(ref drawConditions, "Look Conditions", FGUI_Resources.BGInBoxLightStyle, FGUI_Resources.TexBehaviourIcon);

        //EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxLightStyle);
        ////if (drawConditions)
        //{
        //    GUILayout.Space(4f);


        //    GUILayout.Space(3);
        //}
        //EditorGUILayout.EndVertical();
        GUILayout.Space(-4f);


        //EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();
    }


    static bool drawSetup = true;
    void Tab_Setup()
    {
        EditorGUI.BeginChangeCheck();

        FGUI_Inspector.VSpace(-2, -4);
        GUILayout.BeginVertical(FGUI_Resources.ViewBoxStyle);
        EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxBlankStyle);

        GUILayout.Space(2);

        if (Get.BaseTransform == Get.transform)
        {
            GUILayout.BeginHorizontal();
            GUI.color = new Color(1f, 1f, 1f, 0.65f);
            EditorGUILayout.PropertyField(sp_Base);
            EditorGUILayout.LabelField("(Optional)", GUILayout.Width(60));
            GUI.color = c;
            GUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.PropertyField(sp_Base);
        }
        GUILayout.Space(4);

        GUILayout.BeginHorizontal();

        if (!Get.HeadReference)
            GUILayout.BeginHorizontal(FGUI_Inspector.Style(new Color(0.8f, 0.2f, 0.2f, 0.25f)));
        else
            GUILayout.BeginHorizontal();

        EditorGUILayout.PropertyField(sp_HeadReference, true);

        if (GUILayout.Button(new GUIContent("Auto Find", "By pressing this button, algorithm will go trough hierarchy and try to find object which name includes 'head' or 'neck', be aware, this bone can not be correct but sure it will help you find right one quicker"), new GUILayoutOption[2] { GUILayout.MaxWidth(80), GUILayout.MaxHeight(18) }))
        {
            FindHeadBone(Get);
            EditorUtility.SetDirty(target);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndHorizontal();

        GUILayout.Space(3f);
        EditorGUILayout.PropertyField(sp_StartLookOffset);
        GUILayout.Space(3f);
        El_DrawOptimizeWithMesh();
        GUILayout.Space(3f);


        GUILayout.Space(2f);
        EditorGUILayout.EndVertical();
        GUILayout.BeginVertical(FGUI_Resources.BGInBoxStyle);
        GUILayout.BeginHorizontal();

        if (Get.Eyes == null) Get.Eyes = new System.Collections.Generic.List<Transform>();
        FGUI_Inspector.FoldHeaderStart(ref showEyes, "Eye Game Objects (" + Get.Eyes.Count + ")", null, _TexEyeIcon);

        if (showEyes)
        {
            if (ActiveEditorTracker.sharedTracker.isLocked) GUI.color = new Color(0.44f, 0.44f, 0.44f, 0.8f); else GUI.color = c;
            if (GUILayout.Button(new GUIContent("Lock Inspector", "Locking Inspector Window to help Drag & Drop operations"), new GUILayoutOption[2] { GUILayout.Width(106), GUILayout.Height(18) })) ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
            GUI.color = c;

            if (GUILayout.Button("+", new GUILayoutOption[2] { GUILayout.MaxWidth(28), GUILayout.MaxHeight(18) }))
            {
                Get.Eyes.Add(null);
                Get.UpdateLists();
                EditorUtility.SetDirty(target);
            }
        }

        GUILayout.EndHorizontal();

        GUI.color = c;

        if (showEyes)
        {
            GUILayout.Space(4f);
            GUI.color = new Color(0.5f, 1f, 0.5f, 0.9f);

            var drop = GUILayoutUtility.GetRect(0f, 34f, new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
            GUI.Box(drop, "Drag & Drop your eye GameObjects here", new GUIStyle(EditorStyles.helpBox) { alignment = TextAnchor.MiddleCenter, fixedHeight = 34 });
            var dropEvent = Event.current;

            switch (dropEvent.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!drop.Contains(dropEvent.mousePosition)) break;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (dropEvent.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (var dragged in DragAndDrop.objectReferences)
                        {
                            GameObject draggedObject = dragged as GameObject;

                            if (draggedObject)
                            {
                                if (!Get.Eyes.Contains(draggedObject.transform)) Get.Eyes.Add(draggedObject.transform);
                                EditorUtility.SetDirty(target);
                            }
                        }

                        Get.UpdateLists();
                    }

                    Event.current.Use();
                    break;
            }

            GUI.color = c;
            GUILayout.Space(4f);

            for (int i = 0; i < Get.Eyes.Count; i++)
            {
                GUILayout.BeginHorizontal();
                FIMSpace.FEyes.FEyesAnimator.EyeSetup eyeSetup = null;
                if (i < Get.EyeSetups.Count) eyeSetup = Get.EyeSetups[i];

                if (eyeSetup != null && eyeSetup.ControlType == FIMSpace.FEyes.FEyesAnimator.EyeSetup.EEyeControlType.Blendshape)
                {
                    if (GUILayout.Button(FGUI_Resources.GetFoldSimbolTex(eyeSetup._BlendFoldout, true), EditorStyles.label, GUILayout.Width(21), GUILayout.Height(16))) { eyeSetup._BlendFoldout = !eyeSetup._BlendFoldout; }
                }

                Get.Eyes[i] = (Transform)EditorGUILayout.ObjectField("", Get.Eyes[i], typeof(Transform), true);

                if (eyeSetup != null)
                {
                    eyeSetup.ControlType = (FIMSpace.FEyes.FEyesAnimator.EyeSetup.EEyeControlType)EditorGUILayout.EnumPopup(eyeSetup.ControlType, GUILayout.MaxWidth(96));
                }


                GUI.backgroundColor = new Color(1f, 0.5f, 0.5f, 1f);
                if (GUILayout.Button(FGUI_Resources.GUIC_Remove, FGUI_Resources.ButtonStyle, new GUILayoutOption[2] { GUILayout.Width(22), GUILayout.Height(18) }))
                {
                    Get.Eyes.RemoveAt(i);
                    Get.UpdateLists();
                    EditorUtility.SetDirty(target);
                }
                GUI.backgroundColor = Color.white;

                GUI.color = c;

                GUILayout.EndHorizontal();

                if (eyeSetup != null && eyeSetup.ControlType == FIMSpace.FEyes.FEyesAnimator.EyeSetup.EEyeControlType.Blendshape && eyeSetup._BlendFoldout)
                {
                    if (eyeSetup.BlendshapeMesh == null)
                    {
                        eyeSetup.BlendshapeMesh = Get.Eyes[i].GetComponent<SkinnedMeshRenderer>();
                    }
                    else
                    {
                        if (eyeSetup.BlendshapeMesh.transform != Get.Eyes[i])
                        {
                            eyeSetup.BlendshapeMesh = Get.Eyes[i].GetComponent<SkinnedMeshRenderer>();
                        }
                    }

                    if (eyeSetup.BlendshapeMesh == null)
                    {
                        EditorGUILayout.HelpBox("Could not detect blendshape mesh! Change eye object to be transform of blendshape mesh!", MessageType.None);
                        //eyeSetup.BlendshapeMesh = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("Blendshape Mesh:", eyeSetup.BlendshapeMesh, typeof(SkinnedMeshRenderer), true);
                    }

                    if (eyeSetup.BlendshapeMesh)
                    {
                        //int wdth = 70;

                        EditorGUIUtility.labelWidth = 190; eyeSetup.MinMaxValue= EditorGUILayout.Vector2Field("Blendshapes Min-Max:", eyeSetup.MinMaxValue); EditorGUIUtility.labelWidth = 0;
                        DisplayBlendshapeField("Eye-Left:", ref eyeSetup.EyeLeftShape, eyeSetup.BlendshapeMesh, (int v) => { eyeSetup.EyeLeftShape = v; });
                        DisplayBlendshapeField("Eye-Right:", ref eyeSetup.EyeRightShape, eyeSetup.BlendshapeMesh, (int v) => { eyeSetup.EyeRightShape = v; });
                        DisplayBlendshapeField("Eye-Up:", ref eyeSetup.EyeUpShape, eyeSetup.BlendshapeMesh, (int v) => { eyeSetup.EyeUpShape = v; });
                        DisplayBlendshapeField("Eye-Down:", ref eyeSetup.EyeDownShape, eyeSetup.BlendshapeMesh, (int v) => { eyeSetup.EyeDownShape = v; });
                    }

                }

            }

            GUILayout.Space(3);
        }


        EditorGUILayout.EndVertical();
        GUILayout.Space(-4);
        EditorGUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(Get);
    }


    public static void DisplayBlendshapeField(string label, ref int val, SkinnedMeshRenderer skin, Action<int> apply)
    {
        if (skin == null)
        {
            EditorGUILayout.LabelField("No Mesh to read blenshapes!");
        }
        else
        {
            string valueName = "None";
            if (val > -1) if (val < skin.sharedMesh.blendShapeCount)
                {
                    valueName = skin.sharedMesh.GetBlendShapeName(val);
                }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(label);

            if (GUILayout.Button(valueName, EditorStyles.layerMaskField))
            {
                GenericMenu menu = new GenericMenu();

                int targetVal = -1;

                menu.AddItem(new GUIContent("None"), val == -1, () => { apply.Invoke(-1); });
                for (int i = skin.sharedMesh.blendShapeCount - 1; i >= 0; i--)
                {
                    int shapeVal = i;
                    menu.AddItem(new GUIContent(skin.sharedMesh.GetBlendShapeName(i)), val == i, () => { apply.Invoke(shapeVal); });
                }

                menu.ShowAsContext();

                val = targetVal;
            }

            EditorGUILayout.EndHorizontal();
        }
    }


    static bool drawCorrections = false;
    void Tab_Corrections()
    {
        FGUI_Inspector.VSpace(-2, -4);
        GUILayout.BeginVertical(FGUI_Resources.ViewBoxStyle);
        EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxBlankStyle);

        GUI.enabled = false;
        EditorGUILayout.LabelField("Eyes Rotation Corrections", FGUI_Resources.HeaderStyle);
        GUI.enabled = true;
        GUILayout.Space(3);

        for (int i = 0; i < Get.CorrectionOffsets.Count; i++)
        {
            if (i > Get.Eyes.Count - 1) break;
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false; EditorGUILayout.ObjectField(Get.Eyes[i], typeof(Transform), true, GUILayout.Width(110)); GUI.enabled = true;
            EditorGUILayout.PropertyField(sp_CorrectionOffsets.GetArrayElementAtIndex(i), new GUIContent(""), true);
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(6);
        var sp = serializedObject.FindProperty( "WorldUpIsBaseTransformUp" );
        EditorGUILayout.PropertyField(sp);
        sp.Next( false );
        EditorGUILayout.PropertyField( sp );

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();
    }
}
