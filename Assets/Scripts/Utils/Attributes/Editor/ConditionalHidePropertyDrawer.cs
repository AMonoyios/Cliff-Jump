using UnityEngine;
using UnityEditor;

// - Attribute that helps readability and development in the editor. Has the ability to show and hide properties without the need to
// create a new custom editor script for each class.
[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;

        bool enabled = true;

        // Returns the property path of the property we want to apply the attribute to
        string propertyPath = property.propertyPath;
        // Changes the path to the conditionalsource property path
        string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField);
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        // Checking if the property has a valid Type
        if (sourcePropertyValue != null)
        {
            if (sourcePropertyValue.propertyType == SerializedPropertyType.Enum)
            {
                bool isNegativeEnum = sourcePropertyValue.enumValueIndex > 0;
                enabled = condHAtt.UseInverseOfConditionalSource ? !isNegativeEnum : isNegativeEnum;
            }
            else if (sourcePropertyValue.propertyType == SerializedPropertyType.Boolean)
            {
                enabled = condHAtt.UseInverseOfConditionalSource ? !sourcePropertyValue.boolValue : sourcePropertyValue.boolValue;
            }
            else
            {
                Debug.LogWarning("Conditional hide does not support " + sourcePropertyValue.propertyType.ToString());
                enabled = false;
            }
        }
        else
        {
            Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
        }

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
        if (!condHAtt.HideInInspector || enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }

        GUI.enabled = wasEnabled;
    }
}
