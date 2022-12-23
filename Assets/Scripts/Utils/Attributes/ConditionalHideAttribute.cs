using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideAttribute : PropertyAttribute
{
    public string ConditionalSourceField = "";
    public bool HideInInspector = false;
    public bool UseInverseOfConditionalSource = false;

    public ConditionalHideAttribute(string conditionalSourceField)
    {
        ConditionalSourceField = conditionalSourceField;
        HideInInspector = false;
        UseInverseOfConditionalSource = false;
    }

    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
    {
        ConditionalSourceField = conditionalSourceField;
        HideInInspector = hideInInspector;
        UseInverseOfConditionalSource = false;
    }

    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, bool useInverseOfConditionalSource)
    {
        ConditionalSourceField = conditionalSourceField;
        HideInInspector = hideInInspector;
        UseInverseOfConditionalSource = useInverseOfConditionalSource;
    }
}
