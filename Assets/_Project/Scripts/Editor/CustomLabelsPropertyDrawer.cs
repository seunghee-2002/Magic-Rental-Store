// CustomLabelsPropertyDrawer.cs
using UnityEngine;
using UnityEditor;
using MagicRentalShop.Utils;

[CustomPropertyDrawer(typeof(CustomLabelsAttribute))]
[CustomPropertyDrawer(typeof(GradeLabelsAttribute))]
public class CustomLabelsPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        try
        {
            var customLabels = attribute as CustomLabelsAttribute;
            
            // 배열 인덱스 추출
            string propertyPath = property.propertyPath;
            string customName = GetCustomName(propertyPath, customLabels.labels);
            
            if (!string.IsNullOrEmpty(customName))
            {
                EditorGUI.PropertyField(position, property, new GUIContent(customName, label.tooltip), true);
                return;
            }
            
            // 기본 처리
            EditorGUI.PropertyField(position, property, label, true);
        }
        catch
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
    
    private string GetCustomName(string propertyPath, string[] labels)
    {
        // 배열 인덱스 추출
        if (propertyPath.Contains("Array.data["))
        {
            int startIndex = propertyPath.LastIndexOf("[") + 1;
            int endIndex = propertyPath.LastIndexOf("]");
            
            if (startIndex > 0 && endIndex > startIndex)
            {
                string indexStr = propertyPath.Substring(startIndex, endIndex - startIndex);
                if (int.TryParse(indexStr, out int index) && index < labels.Length)
                {
                    return labels[index];
                }
            }
        }
        
        // 중첩 배열 처리
        var matches = System.Text.RegularExpressions.Regex.Matches(propertyPath, @"\[(\d+)\]");
        if (matches.Count > 0)
        {
            var lastMatch = matches[matches.Count - 1];
            if (int.TryParse(lastMatch.Groups[1].Value, out int index) && index < labels.Length)
            {
                return labels[index];
            }
        }
        
        return null;
    }
}