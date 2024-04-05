using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalEnumHideAttribute : PropertyAttribute
{
    // Enum conditions that determine field visibility
    public string Condition1 = "";
    public string Condition2 = "";

    // List of acceptable values for each enum condition
    public int Enum1Value1 = int.MaxValue;
    public int Enum1Value2 = int.MaxValue;

    public int Enum2Value1 = int.MaxValue;
    public int Enum2Value2 = int.MaxValue;

    // Determines if each condition will be inversed or not
    public bool Enum1Inverse = false;
    public bool Enum2Inverse = false;

    // Determines if the field will be hidden, or just disabled
    public bool HideInInspector = true;

    // If true both conditions will have to be true, if not only one conndition has to be true
    public bool RequireBothConditions;

    public ConditionalEnumHideAttribute(string condition1, int enum1Value1)
    {
        Condition1 = condition1;
        Enum1Value1 = enum1Value1;
    }
}