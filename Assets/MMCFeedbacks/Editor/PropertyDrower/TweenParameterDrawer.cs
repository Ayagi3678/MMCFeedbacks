using MMCFeedbacks.Core;
using UnityEditor;
using UnityEngine;

namespace MMCFeedbacks.Editor
{
    [CustomPropertyDrawer(typeof(TweenParameter),true)]
    public class TweenParameterDrawer : PropertyDrawer
    {
        private float _propertyHeight;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var depth = -1;
            var height = 0f;
            position.y += 10;
            height += 10;
            var toggleRect = position;
            toggleRect.height = 15;
            toggleRect.width = EditorGUIUtility.labelWidth;
            toggleRect.y += 3;
            toggleRect.x -= 15;
            
            var showActiveBox = property.FindPropertyRelative(nameof(TweenParameter.ShowActiveBox));
            var active = property.FindPropertyRelative(nameof(TweenParameter.IsActive));
            position.height = EditorGUI.GetPropertyHeight(active);
            var headerString = StringConversionUtility.SeparateCamelCase(property.name);
            EditorGUI.LabelField(position,headerString,EditorStyling.headerLabel);
            if (showActiveBox.boolValue)active.boolValue=EditorGUI.Toggle(toggleRect,active.boolValue,EditorStyling.smallTickbox);
            position.y += EditorGUI.GetPropertyHeight(active)+2;
            height += EditorGUI.GetPropertyHeight(active) + 2;
            EditorGUI.BeginDisabledGroup(!active.boolValue);
            while(property.NextVisible(true) || depth == -1){
                if (depth != -1 && property.depth < depth) break;
                if (depth != -1 && property.depth > depth) continue;
                depth = property.depth;
                EditorGUI.PropertyField(position,property,true);
                position.y += EditorGUI.GetPropertyHeight(property)+2;
                height += EditorGUI.GetPropertyHeight(property) + 2;
            }
            EditorGUI.EndDisabledGroup();
            _propertyHeight = height;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _propertyHeight-EditorGUIUtility.standardVerticalSpacing;
        }
    }
}