using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute hideAttribute = (ConditionalHideAttribute)attribute;

        // Returns the current selected bool value
        bool propertyEnabled = GetConditionResult(hideAttribute, property);

        // Handles hiding/displaying the property
        bool wasEnabledGUI = GUI.enabled;
        GUI.enabled = propertyEnabled;
        if (!hideAttribute.HideInInspector || propertyEnabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }

        GUI.enabled = wasEnabledGUI;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute hideAttribute = (ConditionalHideAttribute)attribute;

        // Returns the current selected bool value
        bool propertyEnabled = GetConditionResult(hideAttribute, property);

        // Handles hiding/disabling the property
        if (!hideAttribute.HideInInspector || propertyEnabled)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    private bool GetConditionResult(ConditionalHideAttribute hideAttribute, SerializedProperty property)
    {
        bool propertyEnabled = true;

        // Returns the serialized property by using the path to the condition
        string propertyPath = property.propertyPath;
        string conditionPath = propertyPath.Replace(property.name, hideAttribute.Condition);
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        if (sourcePropertyValue != null)
            propertyEnabled = sourcePropertyValue.boolValue;
        else
            Debug.LogWarning("ConditionalHideAttribute has found no matching property in object: " + hideAttribute.Condition);

        if (!hideAttribute.Inverse) return propertyEnabled;
        else return !propertyEnabled;
    }
}