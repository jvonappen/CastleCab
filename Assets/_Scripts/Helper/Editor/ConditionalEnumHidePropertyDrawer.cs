using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ConditionalEnumHideAttribute))]
public class ConditionalEnumHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalEnumHideAttribute hideAttribute = (ConditionalEnumHideAttribute)attribute;

        // Returns true if the conditions are met
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
        ConditionalEnumHideAttribute hideAttribute = (ConditionalEnumHideAttribute)attribute;

        // Returns true if the conditions are met
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

    private bool GetConditionResult(ConditionalEnumHideAttribute hideAttribute, SerializedProperty property)
    {
        // Returns the current selected enum values
        int enum1Value = GetSelectedEnumValue(property, hideAttribute.Condition1);

        int enum2Value = int.MaxValue;
        if (hideAttribute.Condition2 != "")
            enum2Value = GetSelectedEnumValue(property, hideAttribute.Condition2);

        // Returns true if the conditions are met
        return CheckEnums(hideAttribute, enum1Value, enum2Value);
    }

    private int GetSelectedEnumValue(SerializedProperty property, string condition)
    {
        int enumValue = 0;

        SerializedProperty sourcePropertyValue;

        // Finds the fields full relative property path for nested hiding
        if (!property.isArray)
        {
            // Returns the serialized property by using the path to the condition
            string propertyPath = property.propertyPath;
            string conditionPath = propertyPath.Replace(property.name, condition);
            sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

            // If the sourcePropertyValue isnt found, use alternate way (This doesn't work with nested serialized objects)
            sourcePropertyValue ??= property.serializedObject.FindProperty(condition);
        }
        else
        {
            // This doesn't work with nested serialized objects
            sourcePropertyValue = property.serializedObject.FindProperty(condition);
        }


        if (sourcePropertyValue != null)
            enumValue = sourcePropertyValue.enumValueIndex;
        else
            Debug.LogWarning("ConditionalEnumHideAttribute has found no matching property in object: " + condition);

        return enumValue;
    }

    bool CheckEnums(ConditionalEnumHideAttribute hideAttribute, int selectedEnum1Value, int selectedEnum2Value)
    {
        // Only check seconds enum parameter if one is set, if not just check against the first enum parameter
        if (selectedEnum2Value == int.MaxValue)
        {
            if (CheckEnum(hideAttribute, selectedEnum1Value, false))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            // Checks if conditions per enum are met
            bool cond1Met = CheckEnum(hideAttribute, selectedEnum1Value, false);
            bool cond2Met = CheckEnum(hideAttribute, selectedEnum2Value, true);

            // If require both conditions is true, the property will only display when both are met, otherwise only one condition needs to be met
            if (hideAttribute.RequireBothConditions)
            {
                if (cond1Met && cond2Met) return true;
            }
            else
            {
                if (cond1Met || cond2Met) return true;
            }

            return false;
        }

    }

    bool CheckEnum(ConditionalEnumHideAttribute hideAttribute, int selectedEnumValue, bool isSecondEnum)
    {
        if (isSecondEnum)
        {
            if (hideAttribute.Enum2Value1 == selectedEnumValue || hideAttribute.Enum2Value2 == selectedEnumValue)
            {
                return !hideAttribute.Enum2Inverse;
            }
            else
            {
                return hideAttribute.Enum2Inverse;
            }
        }
        else
        {
            if (hideAttribute.Enum1Value1 == selectedEnumValue || hideAttribute.Enum1Value2 == selectedEnumValue)
            {
                return !hideAttribute.Enum1Inverse;
            }
            else
            {
                return hideAttribute.Enum1Inverse;
            }
        }

    }
}