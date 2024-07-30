using FIMSpace.FEditor;
using UnityEditor;
using UnityEngine;

public partial class FEyesAnimator_Editor : UnityEditor.Editor
{

    void DrawNew()
    {
        Undo.RecordObject(target, "Eyes Animator Inspector");

        EditorGUI.BeginChangeCheck();

        serializedObject.Update();

        string title = drawDefaultInspector ? " Default Inspector" : " Eyes Animator 2";
        HeaderBoxMain(title, ref Get.DrawGizmos, ref drawDefaultInspector, _TexEyesAnimIcon, Get, 27);

        DrawNewGUI();

        if( EditorGUI.EndChangeCheck() ) { EditorUtility.SetDirty( Get ); }

        serializedObject.ApplyModifiedProperties();
    }


    private void HeaderBoxMain(string title, ref bool drawGizmos, ref bool defaultInspector, Texture2D scrIcon, MonoBehaviour target, int height = 22)
    {
        EditorGUILayout.BeginVertical(FGUI_Resources.HeaderBoxStyle);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button(new GUIContent(scrIcon), EditorStyles.label, new GUILayoutOption[2] { GUILayout.Width(height - 2), GUILayout.Height(height - 2) }))
        {
            MonoScript script = MonoScript.FromMonoBehaviour(target);
            if (script) EditorGUIUtility.PingObject(script);
        }

        if (GUILayout.Button(title, FGUI_Resources.GetTextStyle(14, true, TextAnchor.MiddleLeft), GUILayout.Height(height)))
        {
            MonoScript script = MonoScript.FromMonoBehaviour(target);
            if (script) EditorGUIUtility.PingObject(script);
        }


        if (EditorGUIUtility.currentViewWidth > 326)
            // Youtube channel button
            if (GUILayout.Button(new GUIContent(FGUI_Resources.Tex_Tutorials, "Open FImpossible Creations Channel with tutorial videos in your web browser"), FGUI_Resources.ButtonStyle, new GUILayoutOption[2] { GUILayout.Width(height), GUILayout.Height(height) }))
            {
                Application.OpenURL("https://www.youtube.com/c/FImpossibleCreations");
            }

        if (EditorGUIUtility.currentViewWidth > 292)
            // Store site button
            if (GUILayout.Button(new GUIContent(FGUI_Resources.Tex_Website, "Open FImpossible Creations Asset Store Page inside your web browser"), FGUI_Resources.ButtonStyle, new GUILayoutOption[2] { GUILayout.Width(height), GUILayout.Height(height) }))
            {
                Application.OpenURL("https://assetstore.unity.com/publishers/37262");
            }

        // Manual file button
        if (_manualFile == null) _manualFile = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(target))) + "/Spine Animator User Manual.pdf");
        if (_manualFile)
            if (GUILayout.Button(new GUIContent(FGUI_Resources.Tex_Manual, "Open .PDF user manual file for Spine Animator"), FGUI_Resources.ButtonStyle, new GUILayoutOption[2] { GUILayout.Width(height), GUILayout.Height(height) }))
            {
                EditorGUIUtility.PingObject(_manualFile);
                Application.OpenURL(Application.dataPath + "/" + AssetDatabase.GetAssetPath(_manualFile).Replace("Assets/", ""));
            }

        FGUI_Inspector.DrawSwitchButton(ref drawGizmos, FGUI_Resources.Tex_GizmosOff, FGUI_Resources.Tex_Gizmos, "Toggle drawing gizmos on character in scene window", height, height, true);


        if (EditorGUIUtility.currentViewWidth > 346)
        {
            bool hierSwitchOn = PlayerPrefs.GetInt("EyesH", 1) == 1;
            FGUI_Inspector.DrawSwitchButton(ref hierSwitchOn, FGUI_Resources.Tex_HierSwitch, null, "Switch drawing small icons in hierarchy", height, height, true);
            PlayerPrefs.SetInt("EyesH", hierSwitchOn ? 1 : 0);
        }


        FGUI_Inspector.DrawSwitchButton(ref defaultInspector, FGUI_Resources.Tex_Default, null, "Toggle inspector view to default inspector.\n\nIf you ever edit source code of Look Animator and add custom variables, you can see them by entering this mode, also sometimes there can be additional/experimental variables to play with.", height, height);


        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }


    void DrawNewGUI()
    {
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


            GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(1f, .4f, .4f, bgAlpha ), Vector4.one * 3, 3));
            FGUI_Inspector.HeaderBox(ref drawCorrections, "Corrections", true, FGUI_Resources.Tex_Tweaks, 22, 21, false);

            if (drawCorrections) Tab_Corrections();

            GUILayout.EndVertical();

        }
    }

}
