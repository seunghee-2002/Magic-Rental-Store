using UnityEngine;
using UnityEditor;

using MagicRentalShop.Utils;

[CustomPropertyDrawer(typeof(ArrayElementTitle))]
public class ArrayElementTitleDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        try
        {
            var config = attribute as ArrayElementTitle;
            SerializedProperty titleNameProp = property.FindPropertyRelative(config.varName);
            string fullPathName = titleNameProp.displayName;
            string name = property.displayName;
            
            if (titleNameProp != null && !string.IsNullOrEmpty(titleNameProp.stringValue))
            {
                name = titleNameProp.stringValue;
            }
            else if (titleNameProp != null && titleNameProp.propertyType == SerializedPropertyType.Enum)
            {
                name = titleNameProp.enumDisplayNames[titleNameProp.enumValueIndex];
            }
            
            EditorGUI.PropertyField(position, property, new GUIContent(name, label.tooltip), true);
        }
        catch
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}