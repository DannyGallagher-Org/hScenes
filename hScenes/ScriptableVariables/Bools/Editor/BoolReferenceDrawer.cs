using UnityEditor;
using UnityEngine;

namespace ScriptableVariables.Bools.Editor
{
    [CustomPropertyDrawer(typeof(BoolReference))]
    public class BoolReferenceDrawer : PropertyDrawer 
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            var prefixLabel = new GUIContent(property.name);

            var style = GUI.skin.label;
            style.alignment = TextAnchor.MiddleLeft;
            style.stretchWidth = true;
            style.wordWrap = false;
            style.padding = new RectOffset(2,10,2,2);

            // Compute how large the button needs to be.
            var size = style.CalcSize(prefixLabel);
            
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent(prefixLabel), style);
            
            // Calculate rects
            var useConstantRect = new Rect(position.x + size.x, position.y, 70, position.height);
            var definitionRect = new Rect(position.x + size.x + 80, position.y, 160, position.height);
            
            var boolProp = property.FindPropertyRelative("UseConstant");
            
            var newValue = EditorGUI.Popup(useConstantRect, boolProp.boolValue?0:1, new[]{"Constant", "Dyanmic"});
            
            if (newValue == 0 != boolProp.boolValue)
            {
                boolProp.boolValue = newValue == 0;
                boolProp.serializedObject.ApplyModifiedProperties();
            }

            if (boolProp.boolValue)
            {
                var constValProp = property.FindPropertyRelative("ConstantValue");
                var constantValue = EditorGUI.Toggle(definitionRect, constValProp.boolValue);
                
                constValProp.boolValue = constantValue;
                constValProp.serializedObject.ApplyModifiedProperties();
            }
            else
            {
                var dynValProp = property.FindPropertyRelative("Variable");
                var dynamicValue = EditorGUI.ObjectField(definitionRect, dynValProp.objectReferenceValue, typeof(BoolVariable), false);

                dynValProp.objectReferenceValue = dynamicValue;
                dynValProp.serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.EndProperty();
        }
    }
}