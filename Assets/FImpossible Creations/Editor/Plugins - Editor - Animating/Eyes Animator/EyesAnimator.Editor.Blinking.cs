using FIMSpace.FEditor;
using FIMSpace.FEyes;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class FEyesAnimator_Editor : UnityEditor.Editor
{

    static bool focusCloseRotations = false;
    static List<Quaternion> openRotations;
    static List<Vector3> openPositions;
    static List<Vector3> openScales;

    public static bool drawBlinking = true;
    public static bool showEyelids = true;
    public static bool showDownB = false;
    public static bool showUpB = false;

    protected SerializedProperty sp_EyeLids;
    protected SerializedProperty sp_EyeLidsCloseRotations;
    protected SerializedProperty sp_EyeLidsClosePos;
    protected SerializedProperty sp_EyeLidsCloseScale;
    protected SerializedProperty sp_SyncWithRandomPreset;
    protected SerializedProperty sp_BlinkFrequency;
    protected SerializedProperty sp_OpenCloseSpeed;
    protected SerializedProperty sp_IndividualBlinking;
    protected SerializedProperty sp_MinOpenValue;

    protected SerializedProperty sp_UpDownEyelidsFactor;
    protected SerializedProperty sp_UpEyelids;
    protected SerializedProperty sp_DownEyelids;

    protected SerializedProperty sp_BlinkingMode;
    protected SerializedProperty sp_BlensShapesMesh;
    protected SerializedProperty sp_BlinkingBlend;
    protected SerializedProperty sp_KeepClose;


    void InitBlinking()
    {

        sp_EyeLids = serializedObject.FindProperty( "EyeLids" );
        sp_EyeLidsCloseRotations = serializedObject.FindProperty( "EyeLidsCloseRotations" );
        sp_EyeLidsClosePos = serializedObject.FindProperty( "EyeLidsClosePositions" );
        sp_EyeLidsCloseScale = serializedObject.FindProperty( "EyeLidsCloseScales" );
        sp_SyncWithRandomPreset = serializedObject.FindProperty( "SyncWithRandomPreset" );
        sp_BlinkFrequency = serializedObject.FindProperty( "BlinkFrequency" );
        sp_OpenCloseSpeed = serializedObject.FindProperty( "OpenCloseSpeed" );
        sp_IndividualBlinking = serializedObject.FindProperty( "IndividualBlinking" );
        sp_MinOpenValue = serializedObject.FindProperty( "MinOpenValue" );

        sp_UpDownEyelidsFactor = serializedObject.FindProperty( "AdditionalEyelidsMotion" );
        sp_UpEyelids = serializedObject.FindProperty( "UpEyelids" );
        sp_DownEyelids = serializedObject.FindProperty( "DownEyelids" );

        sp_BlinkingMode = serializedObject.FindProperty( "BlinkingMode" );
        sp_BlensShapesMesh = serializedObject.FindProperty( "BlendShapesMesh" );
        sp_BlinkingBlend = serializedObject.FindProperty( "BlinkingBlend" );
        sp_KeepClose = serializedObject.FindProperty( "HoldClosedTime" );

    }


    bool drawBlinkingSetup = true;
    bool drawBlinkingTweak = true;
    bool drawUpDownEyelids = false;
    void DrawBlikningModule()
    {
        Color preCol = GUI.color;

        if( Get.EyeLids == null ) Get.EyeLids = new List<Transform>();

        //EditorGUILayout.EndVertical(); ///

        FGUI_Inspector.HeaderBox( ref drawBlinkingSetup, "Blinking Setup", true, FGUI_Resources.Tex_GearSetup );

        if( drawBlinkingSetup )
        {
            GUILayout.Space( 3f );
            EditorGUILayout.PropertyField( sp_BlinkingMode );
            GUILayout.Space( 3f );

            if( Get.BlinkingMode != FEyesAnimator.FE_EyesBlinkingMode.Blendshapes )
            {
                GUILayout.Space( 4f );


                GUILayout.BeginVertical( FGUI_Resources.BGInBoxStyle );
                GUILayout.BeginHorizontal();
                FGUI_Inspector.FoldHeaderStart( ref showEyelids, "Eyelids Game Objects", null, _TexEyeLidUpIcon );

                if( showEyelids )
                {
                    if( !Application.isPlaying )
                    {
                        if( ActiveEditorTracker.sharedTracker.isLocked ) GUI.color = new Color( 0.44f, 0.44f, 0.44f, 0.8f ); else GUI.color = preCol;
                        if( GUILayout.Button( new GUIContent( "Lock Inspector", "Locking Inspector Window to help Drag & Drop operations" ), new GUILayoutOption[2] { GUILayout.Width( 106 ), GUILayout.Height( 16 ) } ) ) ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;

                        GUI.color = preCol;

                        if( GUILayout.Button( "+", new GUILayoutOption[2] { GUILayout.MaxWidth( 28 ), GUILayout.MaxHeight( 14 ) } ) )
                        {
                            Get.EyeLids.Add( null );
                            Get.UpdateLists();
                            EditorUtility.SetDirty( target );
                        }
                    }
                }

                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                GUI.color = preCol;

                if( showEyelids )
                {
                    GUI.color = new Color( 0.5f, 1f, 0.5f, 0.9f );

                    if( !Application.isPlaying )
                    {
                        EditorGUILayout.BeginVertical( FGUI_Resources.BGInBoxBlankStyle );
                        var drop = GUILayoutUtility.GetRect( 0f, 22f, new GUILayoutOption[1] { GUILayout.ExpandWidth( true ) } );
                        GUI.Box( drop, "Drag & Drop your eyelids GameObjects here", new GUIStyle( EditorStyles.helpBox ) { alignment = TextAnchor.MiddleCenter, fixedHeight = 22 } );
                        var dropEvent = Event.current;

                        switch( dropEvent.type )
                        {
                            case EventType.DragUpdated:
                            case EventType.DragPerform:
                                if( !drop.Contains( dropEvent.mousePosition ) ) break;

                                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                                if( dropEvent.type == EventType.DragPerform )
                                {
                                    DragAndDrop.AcceptDrag();

                                    foreach( var dragged in DragAndDrop.objectReferences )
                                    {
                                        GameObject draggedObject = dragged as GameObject;

                                        if( draggedObject )
                                        {
                                            if( !Get.EyeLids.Contains( draggedObject.transform ) ) Get.EyeLids.Add( draggedObject.transform );
                                            EditorUtility.SetDirty( target );
                                        }
                                    }

                                    Get.UpdateLists();
                                }

                                Event.current.Use();
                                break;
                        }
                        EditorGUILayout.EndVertical();
                    }

                    GUILayout.Space( 5 );

                    if( Get.EyeLids.Count > 0 )
                    {
                        EditorGUILayout.HelpBox( "Set closed eyes pose with 'Focus' button enabled", MessageType.None );
                    }

                    EditorGUI.BeginChangeCheck();
                    GUI.color = c;
                    for( int i = 0; i < Get.EyeLids.Count; i++ )
                    {
                        GUILayout.BeginHorizontal();

                        Get.EyeLids[i] = (Transform)EditorGUILayout.ObjectField( "", Get.EyeLids[i], typeof( Transform ), true );

                        if( GUILayout.Button( "X", new GUILayoutOption[2] { GUILayout.Width( 20 ), GUILayout.Height( 14 ) } ) )
                        {
                            if( !Application.isPlaying )
                            {
                                if( Get.UpEyelids.Contains( Get.EyeLids[i] ) ) Get.UpEyelids.Remove( Get.EyeLids[i] );
                                else
                                    if( Get.DownEyelids.Contains( Get.EyeLids[i] ) ) Get.DownEyelids.Remove( Get.EyeLids[i] );

                                if( Get.EyeLids.Count > i ) Get.EyeLids.RemoveAt( i );
                                if( Get.EyeLidsCloseRotations.Count > i ) Get.EyeLidsCloseRotations.RemoveAt( i );
                                if( Get.EyeLidsClosePositions.Count > i ) Get.EyeLidsClosePositions.RemoveAt( i );
                                if( Get.EyeLidsCloseScales.Count > i ) Get.EyeLidsCloseScales.RemoveAt( i );
                                Get.UpdateLists();
                                EditorUtility.SetDirty( target );
                            }

                            GUILayout.EndHorizontal();
                        }
                        else
                        {
                            GUILayout.EndHorizontal();

                            if( Get.BlinkingMode == FEyesAnimator.FE_EyesBlinkingMode.Bones )
                            {
                                if( i < Get.EyeLidsCloseRotations.Count )
                                {
                                    Vector3 preEye = Get.EyeLidsCloseRotations[i];
                                    Get.EyeLidsCloseRotations[i] = EditorGUILayout.Vector3Field( "Close Rotation", Get.EyeLidsCloseRotations[i] );

                                    if( Get.EyeLidsCloseRotations[i] != preEye )
                                    {
                                        EditorUtility.SetDirty( target );
                                    }
                                }
                            }
                            else if( Get.BlinkingMode == FEyesAnimator.FE_EyesBlinkingMode.Bones_Position )
                            {
                                if( i < Get.EyeLidsClosePositions.Count )
                                {
                                    Vector3 preEye = Get.EyeLidsClosePositions[i];
                                    Get.EyeLidsClosePositions[i] = EditorGUILayout.Vector3Field( "Close Position Offset", Get.EyeLidsClosePositions[i] );
                                    if( Get.EyeLidsClosePositions[i] != preEye ) EditorUtility.SetDirty( target );
                                }
                            }
                            else if( Get.BlinkingMode == FEyesAnimator.FE_EyesBlinkingMode.Bones_Scale )
                            {
                                if( i < Get.EyeLidsCloseScales.Count )
                                {
                                    Vector3 preEye = Get.EyeLidsCloseScales[i];
                                    Get.EyeLidsCloseScales[i] = EditorGUILayout.Vector3Field( "Close Eye Scale", Get.EyeLidsCloseScales[i] );
                                    if( Get.EyeLidsCloseScales[i] != preEye ) EditorUtility.SetDirty( target );
                                }
                            }
                        }

                        GUI.color = preCol;
                    }
                    if( EditorGUI.EndChangeCheck() ) { serializedObject.Update(); EditorUtility.SetDirty( Get ); Get.UpdateLists(); }

                    GUILayout.Space( 5 );

                    if( focusCloseRotations ) GUI.color = new Color( 0.14f, 0.9f, 0.05f, 0.9f ); else GUI.color = c;

                    string title = Get.BlinkingMode == FEyesAnimator.FE_EyesBlinkingMode.Bones ? "Focus On Close Rotations" : "Focus On Close Positions";
                    if( GUILayout.Button( new GUIContent( title, "Changing close rotations for eyes in editor mode, so you can easily adjust them" ), EditorStyles.miniButton ) )
                    {
                        focusCloseRotations = !focusCloseRotations;

                        if( Get.BlinkingMode == FEyesAnimator.FE_EyesBlinkingMode.Bones )
                        {
                            if( focusCloseRotations )
                            {
                                if( openRotations == null ) openRotations = new List<Quaternion>(); else openRotations.Clear();

                                for( int i = 0; i < Get.EyeLids.Count; i++ )
                                    if( Get.EyeLids[i] != null )
                                    {
                                        openRotations.Add( Get.EyeLids[i].localRotation );
                                        Get.EyeLids[i].localRotation = Quaternion.Euler( Get.EyeLidsCloseRotations[i] );
                                    }
                            }
                            else
                            {
                                for( int i = 0; i < Get.EyeLids.Count; i++ )
                                    if( Get.EyeLids[i] != null ) if( i < openRotations.Count ) Get.EyeLids[i].localRotation = openRotations[i];
                            }
                        }
                        else if( Get.BlinkingMode == FEyesAnimator.FE_EyesBlinkingMode.Bones_Position )
                        {
                            if( focusCloseRotations )
                            {
                                if( openPositions == null ) openPositions = new List<Vector3>(); else openPositions.Clear();

                                for( int i = 0; i < Get.EyeLids.Count; i++ )
                                    if( Get.EyeLids[i] != null )
                                    {
                                        openPositions.Add( Get.EyeLids[i].localPosition );
                                        Get.EyeLids[i].localPosition = ( openPositions[i] + Get.EyeLidsClosePositions[i] );
                                    }
                            }
                            else
                            {
                                for( int i = 0; i < Get.EyeLids.Count; i++ )
                                    if( Get.EyeLids[i] != null ) if( i < openPositions.Count ) Get.EyeLids[i].localPosition = openPositions[i];
                            }
                        }
                        else if( Get.BlinkingMode == FEyesAnimator.FE_EyesBlinkingMode.Bones_Scale )
                        {
                            if( focusCloseRotations )
                            {
                                if( openScales == null ) openScales = new List<Vector3>(); else openScales.Clear();

                                for( int i = 0; i < Get.EyeLids.Count; i++ )
                                    if( Get.EyeLids[i] != null )
                                    {
                                        openScales.Add( Get.EyeLids[i].localScale );
                                        Get.EyeLids[i].localScale = ( Get.EyeLidsCloseScales[i] );
                                    }
                            }
                            else
                            {
                                for( int i = 0; i < Get.EyeLids.Count; i++ )
                                    if( Get.EyeLids[i] != null ) if( i < openScales.Count ) Get.EyeLids[i].localScale = openScales[i];
                            }
                        }

                    }

                    GUI.color = preCol;

                    if( focusCloseRotations )
                        if( SceneView.lastActiveSceneView )
                        {
#if UNITY_2019_4_OR_NEWER
                            if( SceneView.lastActiveSceneView.drawGizmos == false )
                            {
                                GUILayout.Space( 3 );
                                EditorGUILayout.HelpBox( "GIZMOS are disabled on the scene view and eye focus is not applied!", MessageType.Info );
                                if( GUILayout.Button( "Enable Gizmos on Scene View" ) ) SceneView.lastActiveSceneView.drawGizmos = true;
                                GUILayout.Space( 3 );
                            }
#endif
                        }

                    GUILayout.Space( 3 );

                    EditorGUILayout.PropertyField( serializedObject.FindProperty( "BlinkAnimatedEyelids" ) );

                    GUILayout.Space( 3 );
                }

                if( Get.BlinkingMode == FEyesAnimator.FE_EyesBlinkingMode.Bones )
                {
                    FGUI_Inspector.FoldHeaderStart( ref drawUpDownEyelids, "Up and Down Eyelids Motion", FGUI_Resources.BGInBoxLightStyle, _TexEyeLidDownIcon );

                    if( drawUpDownEyelids )
                    {
                        GUILayout.Space( 4f );
                        EditorGUILayout.PropertyField( sp_UpDownEyelidsFactor );
                        var sp = sp_UpDownEyelidsFactor.Copy();
                        sp.Next( false );
                        Vector2 val = sp.vector2Value;
                        EditorGUILayout.MinMaxSlider( sp.displayName, ref val.x, ref val.y, -1.9f, 1.9f );
                        sp.vector2Value = val;

                        if( Get.UpEyelids != null && Get.DownEyelids != null )
                            if( Get.UpEyelids.Count == 0 && Get.DownEyelids.Count == 0 )
                            {
                                EditorGUILayout.HelpBox( "Tell the component which GameObjects are upper and which are lower eyelids", MessageType.None );

                                if( Get.EyeLids.Count > 0 )
                                {
                                    if( GUILayout.Button( "Try detect in 'Eyelids Game Objects'" ) )
                                        for( int i = 0; i < Get.EyeLids.Count; i++ )
                                        {
                                            if( Get.EyeLids[i] == null ) continue;

                                            if( Get.EyeLids[i].name.Contains( "Up" ) ) Get.UpEyelids.Add( Get.EyeLids[i] );

                                            if( Get.EyeLids[i].name.Contains( "Down" ) ) Get.DownEyelids.Add( Get.EyeLids[i] );
                                            else
                                            if( Get.EyeLids[i].name.Contains( "Lower" ) ) Get.DownEyelids.Add( Get.EyeLids[i] );
                                            EditorUtility.SetDirty( target );
                                        }

                                    if( Get.EyeLids.Count > 0 ) if( Get.AdditionalEyelidsMotion <= 0f )
                                        {
                                            Get.AdditionalEyelidsMotion = 1f;
                                            EditorUtility.SetDirty( target );
                                        }
                                }
                            }

                        GUILayout.Space( 4f );
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField( sp_UpEyelids, true );
                        GUILayout.Space( 2f );
                        EditorGUILayout.PropertyField( sp_DownEyelids, true );
                        EditorGUI.indentLevel--;
                    }

                    GUILayout.EndVertical();
                }

                GUILayout.Space( 4f );
            }
            else // Blenshapes settings
            {

                GUILayout.BeginVertical( FGUI_Resources.BGInBoxStyle );
                GUILayout.BeginHorizontal();
                //FGUI_Inspector.FoldHeaderStart(ref showEyelids, "Prepare Blendshapes", null, FGUI_Resources.Tex_MiniGear);

                GUILayout.EndHorizontal();
                GUI.color = preCol;

                if( Get.BlendShapesMesh == null ) GUI.color = new Color( 1f, 0.7f, 0.7f, 0.9f );
                EditorGUILayout.PropertyField( sp_BlensShapesMesh );
                GUI.color = preCol;

                if( Get.BlendShapesMesh == null )
                {
                    GUILayout.Space( 6f );
                    GUIStyle centerBold = new GUIStyle( EditorStyles.boldLabel ); centerBold.alignment = TextAnchor.MiddleCenter;
                    GUI.color = new Color( 0f, 0f, 0f, 0.5f );
                    EditorGUILayout.LabelField( "Waiting for Blend Shapes Mesh...", centerBold );
                    GUI.color = preCol;
                    EditorGUILayout.EndVertical();
                }
                else
                {
                    string[] blendHapes = new string[0];

                    EditorGUI.indentLevel++;

                    blendHapes = new string[Get.BlendShapesMesh.sharedMesh.blendShapeCount];
                    for( int i = 0; i < blendHapes.Length; i++ ) blendHapes[i] = Get.BlendShapesMesh.sharedMesh.GetBlendShapeName( i );

                    if( Get.BlendShapesMesh.sharedMesh.blendShapeCount == 0 )
                    {
                        GUILayout.Space( 3f );
                        GUIStyle centerBold = new GUIStyle( EditorStyles.boldLabel ); centerBold.alignment = TextAnchor.MiddleCenter;
                        GUI.color = new Color( 0f, 0f, 0f, 0.5f );
                        EditorGUILayout.LabelField( "! No blendshapes inside mesh !", centerBold );
                        GUI.color = preCol;
                    }
                    else // Drawing panel to add new bledshapes to animation list
                    {


                        GUILayout.Space( 3f );
                        GUILayout.BeginHorizontal();
                        showEyelids = EditorGUILayout.Foldout( showEyelids, "Blendshapes to animate (" + Get.BlendShapes.Count + ")", true );

                        if( showEyelids )
                        {
                            if( !Application.isPlaying )
                            {
                                if( GUILayout.Button( "+", new GUILayoutOption[2] { GUILayout.MaxWidth( 28 ), GUILayout.MaxHeight( 14 ) } ) )
                                {
                                    int targetIndex = Get.BlendShapes.Count + 1;
                                    if( targetIndex >= Get.BlendShapesMesh.sharedMesh.blendShapeCount ) targetIndex = Get.BlendShapesMesh.sharedMesh.blendShapeCount - 1;
                                    Get.BlendShapes.Add( new FEyesAnimator.EyesAnimator_BlenshapesInfo( targetIndex ) );
                                    EditorUtility.SetDirty( target );
                                    return;
                                }
                            }
                        }

                        GUILayout.EndHorizontal();
                        GUI.color = preCol;



                        if( showEyelids )
                        {
                            EditorGUI.indentLevel--;
                            for( int i = 0; i < Get.BlendShapes.Count; i++ )
                            {
                                GUILayout.BeginHorizontal();

                                if( Get.BlendShapes[i].UseOtherShape == null )
                                    Get.BlendShapes[i].ShapeIndex = EditorGUILayout.Popup( Get.BlendShapes[i].ShapeIndex, blendHapes );
                                else
                                {
                                    var blendSh = Get.BlendShapes[i];

                                    if ( blendSh.UseOtherShape.sharedMesh != null && blendSh.UseOtherShape.sharedMesh.blendShapeCount > 0)
                                    {
                                        string shapeName = "Out Of Range";
                                        if( blendSh.ShapeIndex < blendSh.UseOtherShape.sharedMesh.blendShapeCount )
                                        {
                                            shapeName = blendSh.UseOtherShape.sharedMesh.GetBlendShapeName( blendSh.ShapeIndex );
                                        }

                                        if( GUILayout.Button( shapeName ) )
                                        {
                                            GenericMenu menu = new GenericMenu();

                                            for( int b = 0; b < blendSh.UseOtherShape.sharedMesh.blendShapeCount; b++ )
                                            {
                                                int shapeId = b;
                                                menu.AddItem( new GUIContent( blendSh.UseOtherShape.sharedMesh.GetBlendShapeName( b ) ), false, () =>
                                                {
                                                    blendSh.ShapeIndex = shapeId;
                                                    EditorUtility.SetDirty( Get );
                                                } );
                                            }

                                            menu.ShowAsContext();
                                        }
                                    }
                                    else
                                    {
                                        EditorGUILayout.HelpBox( "No blendshapes in " + blendSh.UseOtherShape.name, MessageType.None );
                                    }
                                    
                                }

                                EditorGUIUtility.fieldWidth = 28;
                                EditorGUIUtility.labelWidth = 4;
                                EditorGUILayout.PropertyField( serializedObject.FindProperty( "BlendShapes" ).GetArrayElementAtIndex( i ).FindPropertyRelative( "UseOtherShape" ), new GUIContent( " ", "Use if your character is using multiple meshes for animating blinking" ), GUILayout.MaxWidth( 50 ) );
                                EditorGUIUtility.labelWidth = 46;
                                EditorGUILayout.PropertyField( serializedObject.FindProperty( "BlendShapes" ).GetArrayElementAtIndex( i ).FindPropertyRelative( "Open" ) );
                                EditorGUIUtility.labelWidth = 51;
                                EditorGUILayout.PropertyField( serializedObject.FindProperty( "BlendShapes" ).GetArrayElementAtIndex( i ).FindPropertyRelative( "Closed" ) );
                                EditorGUIUtility.labelWidth = 14;
                                EditorGUIUtility.fieldWidth = 12;

                                EditorGUILayout.PropertyField( serializedObject.FindProperty( "BlendShapes" ).GetArrayElementAtIndex( i ).FindPropertyRelative( "Animated" ), new GUIContent( "A", "If this blendshape is animated and you want use blinking as additive motion, toggle t" ) );
                                if( Get.BlendShapes[i].Animated )
                                    EditorGUILayout.PropertyField( serializedObject.FindProperty( "BlendShapes" ).GetArrayElementAtIndex( i ).FindPropertyRelative( "MaxToClose" ), new GUIContent( "L", "If maximum value for blendshape should be 'closed' value" ) );

                                EditorGUIUtility.labelWidth = 0;
                                EditorGUIUtility.fieldWidth = 0;

                                if( GUILayout.Button( "X", new GUILayoutOption[2] { GUILayout.Width( 20 ), GUILayout.Height( 14 ) } ) )
                                {
                                    if( !Application.isPlaying )
                                    {
                                        Get.BlendShapes.RemoveAt( i );
                                        EditorUtility.SetDirty( target );
                                        break;
                                    }
                                }

                                GUILayout.EndHorizontal();
                            }
                            EditorGUI.indentLevel++;
                        }




                        for( int i = 0; i < Get.EyeLids.Count; i++ )
                        {
                            GUILayout.BeginHorizontal();

                            string name;
                            if( !Get.EyeLids[i] )
                            {
                                name = "Assign Object";
                                GUI.color = new Color( 0.9f, 0.4f, 0.4f, 0.9f );
                            }
                            else
                            {
                                name = Get.EyeLids[i].name;
                                if( name.Length > 16 ) name = Get.EyeLids[i].name.Substring( 0, 12 ) + "...";
                            }

                            Get.EyeLids[i] = (Transform)EditorGUILayout.ObjectField( "[" + i + "] " + name, Get.EyeLids[i], typeof( Transform ), true );

                            if( GUILayout.Button( "X", new GUILayoutOption[2] { GUILayout.Width( 20 ), GUILayout.Height( 14 ) } ) )
                            {
                                if( !Application.isPlaying )
                                {
                                    if( Get.UpEyelids.Contains( Get.EyeLids[i] ) ) Get.UpEyelids.Remove( Get.EyeLids[i] );
                                    else
                                        if( Get.DownEyelids.Contains( Get.EyeLids[i] ) ) Get.DownEyelids.Remove( Get.EyeLids[i] );

                                    Get.EyeLids.RemoveAt( i );
                                    Get.EyeLidsCloseRotations.RemoveAt( i );
                                    Get.UpdateLists();
                                    EditorUtility.SetDirty( target );
                                }

                                GUILayout.EndHorizontal();
                            }
                            else
                            {
                                GUILayout.EndHorizontal();

                                if( i < Get.EyeLidsCloseRotations.Count )
                                {
                                    Vector3 preEye = Get.EyeLidsCloseRotations[i];
                                    Get.EyeLidsCloseRotations[i] = EditorGUILayout.Vector3Field( "Close Rotation", Get.EyeLidsCloseRotations[i] );

                                    if( Get.EyeLidsCloseRotations[i] != preEye )
                                    {
                                        EditorUtility.SetDirty( target );
                                    }
                                }
                            }

                            GUI.color = preCol;
                        }
                    }

                    EditorGUILayout.EndVertical();

                    if( Get.BlendShapesMesh.sharedMesh.blendShapeCount > 0 )
                    {
                        GUILayout.Space( 4f );
                        EditorGUILayout.BeginVertical( FGUI_Inspector.Style( new Color( 0.6f, 0.4f, 0.8f, 0.07f ) ) );

                        EditorGUI.indentLevel--;
                        EditorGUILayout.PropertyField( sp_UpDownEyelidsFactor );
                        EditorGUI.indentLevel++;
                        GUILayout.Space( 4f );

                        if( Get.UpEyelids != null && Get.DownEyelids != null )
                            if( Get.UpEyelidsBlendShapes.Count == 0 && Get.DownEyelidsBlendShapes.Count == 0 )
                            {
                                EditorGUILayout.HelpBox( "Tell the component which blendshapes should be animated to enchance animations of hardly looking up or down", MessageType.None );
                            }






                        GUILayout.BeginHorizontal();
                        showUpB = EditorGUILayout.Foldout( showUpB, "Blendshapes Looking Up (" + Get.UpEyelidsBlendShapes.Count + ")", true );

                        if( showUpB )
                        {
                            if( !Application.isPlaying )
                            {
                                if( GUILayout.Button( "+", new GUILayoutOption[2] { GUILayout.MaxWidth( 28 ), GUILayout.MaxHeight( 14 ) } ) )
                                {
                                    int targetIndex = Get.UpEyelidsBlendShapes.Count + 1;
                                    if( targetIndex >= Get.BlendShapesMesh.sharedMesh.blendShapeCount ) targetIndex = Get.BlendShapesMesh.sharedMesh.blendShapeCount - 1;
                                    Get.UpEyelidsBlendShapes.Add( new FEyesAnimator.EyesAnimator_BlenshapesInfo( targetIndex ) );
                                    EditorUtility.SetDirty( target );
                                    return;
                                }
                            }
                        }

                        GUILayout.EndHorizontal();

                        if( showUpB )
                        {
                            for( int i = 0; i < Get.UpEyelidsBlendShapes.Count; i++ )
                            {
                                GUILayout.BeginHorizontal();

                                Get.UpEyelidsBlendShapes[i].ShapeIndex = EditorGUILayout.Popup( Get.UpEyelidsBlendShapes[i].ShapeIndex, blendHapes );
                                EditorGUIUtility.fieldWidth = 28;
                                EditorGUIUtility.labelWidth = 34;
                                EditorGUI.indentLevel--;
                                EditorGUILayout.PropertyField( serializedObject.FindProperty( "UpEyelidsBlendShapes" ).GetArrayElementAtIndex( i ).FindPropertyRelative( "Open" ), new GUIContent( "Up" ) );
                                EditorGUIUtility.labelWidth = 46;
                                EditorGUILayout.PropertyField( serializedObject.FindProperty( "UpEyelidsBlendShapes" ).GetArrayElementAtIndex( i ).FindPropertyRelative( "Closed" ), new GUIContent( "Down" ) );
                                EditorGUIUtility.labelWidth = 14;
                                EditorGUIUtility.fieldWidth = 12;
                                EditorGUILayout.PropertyField( serializedObject.FindProperty( "UpEyelidsBlendShapes" ).GetArrayElementAtIndex( i ).FindPropertyRelative( "Animated" ), new GUIContent( "A", "If this blendshape is animated and you want use blinking as additive motion, toggle t" ) );
                                EditorGUI.indentLevel++;

                                EditorGUIUtility.labelWidth = 0;
                                EditorGUIUtility.fieldWidth = 0;

                                if( GUILayout.Button( "X", new GUILayoutOption[2] { GUILayout.Width( 20 ), GUILayout.Height( 14 ) } ) )
                                {
                                    if( !Application.isPlaying )
                                    {
                                        Get.UpEyelidsBlendShapes.RemoveAt( i );
                                        EditorUtility.SetDirty( target );
                                        break;
                                    }
                                }

                                GUILayout.EndHorizontal();
                            }
                        }






                        GUILayout.BeginHorizontal();
                        showDownB = EditorGUILayout.Foldout( showDownB, "Blendshapes Looking Down (" + Get.DownEyelidsBlendShapes.Count + ")", true );

                        if( showDownB )
                        {
                            if( !Application.isPlaying )
                            {
                                if( GUILayout.Button( "+", new GUILayoutOption[2] { GUILayout.MaxWidth( 28 ), GUILayout.MaxHeight( 14 ) } ) )
                                {
                                    int targetIndex = Get.DownEyelidsBlendShapes.Count + 1;
                                    if( targetIndex >= Get.BlendShapesMesh.sharedMesh.blendShapeCount ) targetIndex = Get.BlendShapesMesh.sharedMesh.blendShapeCount - 1;
                                    Get.DownEyelidsBlendShapes.Add( new FEyesAnimator.EyesAnimator_BlenshapesInfo( targetIndex ) );
                                    EditorUtility.SetDirty( target );
                                    return;
                                }
                            }
                        }

                        GUILayout.EndHorizontal();

                        if( showDownB )
                        {
                            for( int i = 0; i < Get.DownEyelidsBlendShapes.Count; i++ )
                            {
                                GUILayout.BeginHorizontal();

                                Get.DownEyelidsBlendShapes[i].ShapeIndex = EditorGUILayout.Popup( Get.DownEyelidsBlendShapes[i].ShapeIndex, blendHapes );
                                EditorGUIUtility.fieldWidth = 28;
                                EditorGUIUtility.labelWidth = 46;
                                EditorGUI.indentLevel--;
                                EditorGUILayout.PropertyField( serializedObject.FindProperty( "DownEyelidsBlendShapes" ).GetArrayElementAtIndex( i ).FindPropertyRelative( "Open" ), new GUIContent( "Down" ) );
                                EditorGUIUtility.labelWidth = 34;
                                EditorGUILayout.PropertyField( serializedObject.FindProperty( "DownEyelidsBlendShapes" ).GetArrayElementAtIndex( i ).FindPropertyRelative( "Closed" ), new GUIContent( "Up" ) );
                                EditorGUIUtility.labelWidth = 14;
                                EditorGUIUtility.fieldWidth = 12;
                                EditorGUILayout.PropertyField( serializedObject.FindProperty( "DownEyelidsBlendShapes" ).GetArrayElementAtIndex( i ).FindPropertyRelative( "Animated" ), new GUIContent( "A", "If this blendshape is animated and you want use blinking as additive motion, toggle t" ) );
                                EditorGUI.indentLevel++;

                                EditorGUIUtility.labelWidth = 0;
                                EditorGUIUtility.fieldWidth = 0;

                                if( GUILayout.Button( "X", new GUILayoutOption[2] { GUILayout.Width( 20 ), GUILayout.Height( 14 ) } ) )
                                {
                                    if( !Application.isPlaying )
                                    {
                                        Get.DownEyelidsBlendShapes.RemoveAt( i );
                                        EditorUtility.SetDirty( target );
                                        break;
                                    }
                                }

                                GUILayout.EndHorizontal();
                            }
                        }


                        EditorGUILayout.EndVertical();
                    }

                    GUILayout.Space( 4f );
                    EditorGUI.indentLevel--;


                    //if (false)
                    //{
                    //    EditorGUI.indentLevel--;
                    //    for (int i = 0; i < Get.BlendShapes.Count; i++)
                    //    {
                    //        GUILayout.BeginHorizontal();

                    //        Get.BlendShapes[i].ShapeIndex = EditorGUILayout.Popup(Get.BlendShapes[i].ShapeIndex, blendHapes);
                    //        EditorGUIUtility.fieldWidth = 28;
                    //        EditorGUIUtility.labelWidth = 46;
                    //        EditorGUILayout.PropertyField(serializedObject.FindProperty("BlendShapes").GetArrayElementAtIndex(i).FindPropertyRelative("Open"));
                    //        EditorGUIUtility.labelWidth = 51;
                    //        EditorGUILayout.PropertyField(serializedObject.FindProperty("BlendShapes").GetArrayElementAtIndex(i).FindPropertyRelative("Closed"));
                    //        EditorGUIUtility.labelWidth = 14;
                    //        EditorGUIUtility.fieldWidth = 12;

                    //        EditorGUILayout.PropertyField(serializedObject.FindProperty("BlendShapes").GetArrayElementAtIndex(i).FindPropertyRelative("Animated"), new GUIContent("A", "If this blendshape is animated and you want use blinking as additive motion, toggle t"));
                    //        if (Get.BlendShapes[i].Animated)
                    //            EditorGUILayout.PropertyField(serializedObject.FindProperty("BlendShapes").GetArrayElementAtIndex(i).FindPropertyRelative("MaxToClose"), new GUIContent("L", "If maximum value for blendshape should be 'closed' value"));

                    //        EditorGUIUtility.labelWidth = 0;
                    //        EditorGUIUtility.fieldWidth = 0;

                    //        if (GUILayout.Button("X", new GUILayoutOption[2] { GUILayout.Width(20), GUILayout.Height(14) }))
                    //        {
                    //            if (!Application.isPlaying)
                    //            {
                    //                Get.BlendShapes.RemoveAt(i);
                    //                EditorUtility.SetDirty(target);
                    //                break;
                    //            }
                    //        }

                    //        GUILayout.EndHorizontal();
                    //    }
                    //    EditorGUI.indentLevel++;
                    //}


                }

            }

        }


        GUI.color = new Color( 0.95f, 0.95f, 1f, 0.92f );
        FGUI_Inspector.HeaderBox( ref drawBlinkingTweak, "Tweak Blinking", true, FGUI_Resources.Tex_Sliders );

        if( drawBlinkingTweak )
        {
            GUILayout.Space( 2f );
            EditorGUILayout.PropertyField( sp_BlinkingBlend );
            GUILayout.Space( 5f );

            EditorGUILayout.PropertyField( sp_OpenCloseSpeed );
            EditorGUILayout.PropertyField( sp_MinOpenValue );
            EditorGUILayout.PropertyField( sp_KeepClose );
            EditorGUILayout.PropertyField( sp_IndividualBlinking );
            GUILayout.Space( 5f );

            EditorGUILayout.PropertyField( sp_SyncWithRandomPreset );
            if( !Get.SyncWithRandomPreset ) EditorGUILayout.PropertyField( sp_BlinkFrequency );
        }


        GUI.color = preCol;
        GUILayout.Space( 6f );
    }


    void HandleDrawingBlinkingCoords( List<Vector3> coords, string title = "Rotation" )
    {

    }

}
