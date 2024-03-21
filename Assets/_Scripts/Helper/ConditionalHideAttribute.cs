using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideAttribute : PropertyAttribute
{
    // Bool name that will determine field visibility
    public string Condition = "";

    // Determines if the field will be hidden, or just disabled
    public bool HideInInspector = true;

    // Determines whether or not to invert the bool before determining visibility
    public bool Inverse = false;

    public ConditionalHideAttribute(string boolCondition)
    {
        Condition = boolCondition;
    }

    public ConditionalHideAttribute(string boolCondition, bool hideInInspector)
    {
        Condition = boolCondition;
        HideInInspector = hideInInspector;
    }
}