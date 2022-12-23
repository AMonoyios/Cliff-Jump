using UnityEditor;
using UnityEngine;
using Utils;

[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
public class MinToDrawer : PropertyDrawer
{
    private const float precision = 0.01f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;
        var att = (MinMaxSliderAttribute)attribute;
        var type = property.propertyType;

        string minName = att.minName.Replace("$", property.name);
        int lastDot = property.propertyPath.LastIndexOf('.');
        if (lastDot > -1)
            minName = property.propertyPath[..lastDot] + '.' + minName;

        if (type == SerializedPropertyType.Float)
        {
            label.text = " ";
        }

        var ctrlRect = EditorGUI.PrefixLabel(position, label);
        Rect[] r = SplitRectIn3(ctrlRect, 55); // 36
        if (type == SerializedPropertyType.Vector2)
        {
            EditorGUI.BeginChangeCheck();
            var vec = property.vector2Value;
            float min = vec.x.RoundToDecimals(precision);
            float to = vec.y.RoundToDecimals(precision);
            min = EditorGUI.FloatField(r[0], min);
            to = EditorGUI.FloatField(r[2], to);
            EditorGUI.MinMaxSlider(r[1], ref min, ref to, att.min, att.max ?? to);
            vec = new Vector2(min < to ? min : to, to);
            if (EditorGUI.EndChangeCheck())
                property.vector2Value = vec;
        }
        else if (type == SerializedPropertyType.Vector2Int)
        {
            EditorGUI.BeginChangeCheck();
            var vec = property.vector2IntValue;
            float min = vec.x;
            float to = vec.y;
            min = EditorGUI.IntField(r[0], (int)min);
            to = EditorGUI.IntField(r[2], (int)to);
            EditorGUI.MinMaxSlider(r[1], ref min, ref to, att.min, att.max ?? to);
            vec = new Vector2Int(Mathf.RoundToInt(min < to ? min : to), Mathf.RoundToInt(to));
            if (EditorGUI.EndChangeCheck())
                property.vector2IntValue = vec;
        }
        else if (type == SerializedPropertyType.Float)
        {
            EditorGUI.BeginChangeCheck();
            // Line setup
            var line2 = position;
            line2.y += EditorGUIUtility.singleLineHeight;

            // First we draw the float below/above as normal
            EditorGUI.PropertyField(line2, property);

            // Then the slider
            var minProperty = property.serializedObject.FindProperty(minName);
            if (minProperty?.propertyType != SerializedPropertyType.Float)
            {
                EditorGUI.HelpBox(ctrlRect, "Min float not found!!", MessageType.Info);
                return;
            }
            float minVal = minProperty.floatValue.RoundToDecimals(precision);
            float maxVal = property.floatValue.RoundToDecimals(precision);

            EditorGUI.MinMaxSlider(r[1], ref minVal, ref maxVal, att.min, att.max ?? maxVal);
            EditorGUI.LabelField(r[0], att.min.ToString());

            if (att.max.HasValue && maxVal > att.max.Value)
            {
                // Shows that the max value overflowed the slider
                // So if you just wanna try infinite range and stuff you just put 999
                // and it shows clearly that it is a big test value with the color
                // This is only if you specify a max value in the attribute
                Color c = GUI.contentColor;
                GUI.contentColor = ColorExtra.Orange;
                EditorGUI.LabelField(r[2], maxVal.ToString());
                GUI.contentColor = c;
            }
            else
            {
                EditorGUI.LabelField(r[2], (att.max ?? maxVal).ToString());
            }

            // Proofcheck
            maxVal = Mathf.Max(att.min, maxVal);
            minVal = Mathf.Clamp(minVal, att.min, maxVal);

            // And finally update the variables
            if (EditorGUI.EndChangeCheck())
            {
                minProperty.floatValue = minVal;
                property.floatValue = maxVal;
            }
        }
        else
        {
            EditorGUI.HelpBox(ctrlRect, "MinTo is for Vector2 or float!!", MessageType.Error);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int lines = 1;
        if (property.propertyType == SerializedPropertyType.Float)
            lines = 2;

        return lines * EditorGUIUtility.singleLineHeight;
    }

    public static Rect[] SplitRectIn3(Rect rect, int bordersSize, int space = 0)
    {
        Rect[] r = SplitRect(rect, 3);
        int pad = (int)r[0].width - bordersSize;
        int ps = pad + space;
        r[0].width = r[2].width -= ps;
        r[1].width += pad * 2;
        r[1].x -= pad;
        r[2].x += ps;
        return r;
    }
    public static Rect[] SplitRect(Rect a, int n)
    {
        Rect[] r = new Rect[n];
        for (int i = 0; i < n; ++i)
            r[i] = new Rect(a.x + (a.width / n * i), a.y, a.width / n, a.height);
        return r;
    }
}
