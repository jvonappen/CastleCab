#if EYES_LOOKANIMATOR_IMPORTED
using FIMSpace.FEditor;
using FIMSpace.FLook;
using UnityEditor;
using UnityEngine;

public partial class FEyesAnimator_Editor : UnityEditor.Editor
{
    protected SerializedProperty sp_LookAnimator;
    protected SerializedProperty sp_SyncTarget;
    protected SerializedProperty sp_SyncClamping;
    protected SerializedProperty sp_SyncRanges;
    protected SerializedProperty sp_SyncAmount;

    public static Texture2D _TexLookAnimIcon { get { if (__texLookAnimIcon != null) return __texLookAnimIcon; __texLookAnimIcon = Resources.Load<Texture2D>("Look Animator/LookAnimator_SmallIcon"); return __texLookAnimIcon; } }
    private static Texture2D __texLookAnimIcon = null;

    void InitSyncVariables()
    {
        sp_LookAnimator = serializedObject.FindProperty("LookAnimator");
        sp_SyncTarget = serializedObject.FindProperty("SyncTarget");
        sp_SyncClamping = serializedObject.FindProperty("SyncClamping");
        sp_SyncRanges = serializedObject.FindProperty("SyncRanges");
        sp_SyncAmount = serializedObject.FindProperty("SyncUseAmount");
    }

    bool drawLookAnimatorSync = false;
    void DrawLookAnimatorSync()
    {
        RectOffset zeroOff = new RectOffset(0, 0, 0, 0);
        GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(.8f, .8f, .8f, .2f), Vector4.one * 3, 3));
        FGUI_Inspector.HeaderBox(ref drawLookAnimatorSync, "Look Animator Sync Settings", true, _TexLookAnimIcon, 22, 21, false);

        FLookAnimator look = Get.GetComponentInChildren<FLookAnimator>();
        if (look)
        {
            look._editor_hideEyes = true;
            look.UseEyes = false;
        }

        if (drawLookAnimatorSync)
        {
            GUILayout.BeginVertical(FGUI_Resources.BGInBoxBlankStyle);
            EditorGUILayout.PropertyField(sp_LookAnimator);
            EditorGUILayout.PropertyField(sp_SyncTarget);
            EditorGUILayout.PropertyField(sp_SyncClamping);
            EditorGUILayout.PropertyField(sp_SyncRanges);
            EditorGUILayout.PropertyField(sp_SyncAmount);
            GUILayout.Space(4);
            GUILayout.EndVertical();
        }

        GUILayout.EndVertical();
    }


}

#endif
