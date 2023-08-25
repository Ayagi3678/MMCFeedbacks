using System;
using System.Collections.Generic;
using MMCFeedbacks.Core;
using UnityEditor;
using UnityEngine;
using GetCondFunc =
    System.Func<UnityEditor.SerializedProperty, MMCFeedbacks.Core.DisplayIfAttribute, bool>;

namespace MMCFeedbacks.Editor
{
    [CustomPropertyDrawer(typeof(DisplayIfAttribute))]
    public class DisplayIfDrawer : PropertyDrawer
    {
        private readonly Dictionary<Type, GetCondFunc> _disableCondFuncMap = new()
        {
            { typeof(bool), (prop, attr) => attr.TrueThenShow ? !prop.boolValue : prop.boolValue },
            {
                typeof(string),
                (prop, attr) => attr.TrueThenShow
                    ? prop.stringValue == attr.ComparedStr
                    : prop.stringValue != attr.ComparedStr
            },
            {
                typeof(int),
                (prop, attr) => attr.TrueThenShow
                    ? prop.intValue != attr.ComparedInt
                    : prop.intValue == attr.ComparedInt
            },
            {
                typeof(float),
                (prop, attr) => attr.TrueThenShow
                    ? prop.floatValue <= attr.ComparedFloat
                    : prop.floatValue > attr.ComparedFloat
            },
            {
                typeof(TweenParameter),
                (prop, attr) => attr.TrueThenShow
                    ? prop.FindPropertyRelative("IsActive").boolValue
                    : !prop.FindPropertyRelative("IsActive").boolValue
            }
        };
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var drawer = PropertyDrawerDatabase.GetDrawer(fieldInfo.FieldType);
            if (attribute is DisplayIfAttribute attr)
            {
                var prop = FindPathProperty(property, attr);
                if (prop == null)
                {
                    Debug.LogError($"Not found '{attr.VariableName}' property");
                    EditorGUI.PropertyField(position, property, label, true);
                    return;
                }
                if (IsDisable(attr, prop)) return;
            }
            if (drawer != null)
            {
                drawer.OnGUI(position,property,label);
                return;
            }
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var drawer = PropertyDrawerDatabase.GetDrawer(fieldInfo.FieldType);
            if (attribute is DisplayIfAttribute attr)
            {
                var prop = FindPathProperty(property, attr);
                if(prop==null)  return EditorGUI.GetPropertyHeight(property, true);
                if (IsDisable(attr, prop))
                {

                    return -EditorGUIUtility.standardVerticalSpacing;
                    
                }
            }

            if (drawer != null)return drawer.GetPropertyHeight(property,new GUIContent());
            return EditorGUI.GetPropertyHeight(property, true);
        }

        private SerializedProperty FindPathProperty(SerializedProperty property, DisplayIfAttribute attr)
        {
            var path=property.propertyPath.Split('[', ']');
            if (path.Length == 1) return property.serializedObject.FindProperty(attr.VariableName);
            
            string targetPath = attr.VariableName;
            string rootPath = property.propertyPath;
            int splitIndex  = rootPath.LastIndexOf(".", StringComparison.Ordinal);
            if (splitIndex >= 0)
                targetPath = $"{rootPath[..splitIndex]}.{targetPath}";
            /*if (!targetPath.EndsWith("]"))
            {
                int nameIndex = targetPath.LastIndexOf(attr.VariableName);
                targetPath    = targetPath.Substring(0, nameIndex);
                // 変数に親子関係になっているときの"."消し
                int parentIndex = targetPath.LastIndexOf(".");
                if (parentIndex >= 0)
                    targetPath = targetPath.Substring(0, parentIndex);
            }*/
            var targetProperty = property.serializedObject.FindProperty(targetPath);
            return targetProperty;
        }
        private bool IsDisable(DisplayIfAttribute attr, SerializedProperty prop)
        {
            if (!_disableCondFuncMap.TryGetValue(attr.VariableType, out var disableCondFunc))
            {
                Debug.LogError($"{attr.VariableType} type is not supported");
                return false;
            }

            //Debug.Log(prop);
            return disableCondFunc(prop, attr);
        }
    }
}