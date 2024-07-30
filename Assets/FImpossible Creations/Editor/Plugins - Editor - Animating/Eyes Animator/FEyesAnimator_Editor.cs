using FIMSpace.FEditor;
using FIMSpace.FEyes;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;





/// <summary>
/// FM: Editor class component to enchance controll over component from inspector window
/// </summary>
[CanEditMultipleObjects]
[UnityEditor.CustomEditor(typeof(FEyesAnimator))]
public partial class FEyesAnimator_Editor : UnityEditor.Editor
{

    public override void OnInspectorGUI()
    {
        Undo.RecordObject(target, "Eyes Animator Inspector");

        serializedObject.Update();

        string title = drawDefaultInspector ? " Default Inspector" : " Eyes Animator 2";
        HeaderBoxMain(title, ref Get.DrawGizmos, ref drawDefaultInspector, _TexEyesAnimIcon, Get, 27);


        #region Preparations for unity versions and skin

        c = Color.Lerp(GUI.color * new Color(0.8f, 0.8f, 0.8f, 0.7f), GUI.color, Mathf.InverseLerp(0f, 0.15f, Get.EyesAnimatorAmount));

        RectOffset zeroOff = new RectOffset(0, 0, 0, 0);
        float bgAlpha = 0.05f; if (EditorGUIUtility.isProSkin) bgAlpha = 0.1f;

        #endregion


        if (drawDefaultInspector)
        {
            DrawDefaultInspector();
        }
        else
        {

            GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(.7f, .7f, .7f, bgAlpha), Vector4.one * 3, 3));
            FGUI_Inspector.HeaderBox(ref drawSetup, "References Setup", true, FGUI_Resources.Tex_GearSetup, 22, 21, false);

            if (drawSetup) Tab_Setup();

            GUILayout.EndVertical();



            GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(.3f, .4f, 1f, bgAlpha), Vector4.one * 3, 3));
            FGUI_Inspector.HeaderBox(ref drawTweakingAnimationSettings, "Tweaking Animation", true, FGUI_Resources.Tex_Sliders, 22, 21, false);

            if (drawTweakingAnimationSettings) Tab_Tweaking();

            GUILayout.EndVertical();


            GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(.3f, 1f, .7f, bgAlpha), Vector4.one * 3, 3));
            FGUI_Inspector.HeaderBox(ref drawAdditionalModules, "Additional Modules", true, FGUI_Resources.Tex_Module, 22, 21, false);

            if (drawAdditionalModules) Tab_AdditionalModules();

            GUILayout.EndVertical();


            GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(1f, .4f, .4f, bgAlpha), Vector4.one * 3, 3));
            FGUI_Inspector.HeaderBox(ref drawCorrections, "Corrections", true, FGUI_Resources.Tex_Tweaks, 22, 21, false);

            if (drawCorrections) Tab_Corrections();

            GUILayout.EndVertical();

        }

#if EYES_LOOKANIMATOR_IMPORTED
        DrawLookAnimatorSync();
        serializedObject.ApplyModifiedProperties();
    }


#else
        serializedObject.ApplyModifiedProperties();
    }

        void DrawLookAnimatorSync()
        {}

        void InitSyncVariables()
        {}

#endif

}


