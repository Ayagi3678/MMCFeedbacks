using System;
using System.Collections.Generic;
using MMCFeedbacks.Core;
using MMCFeedbacks.etc;
using UnityEditor;
using UnityEngine;
using GetCondFunc =
    System.Func<UnityEditor.SerializedProperty, MMCFeedbacks.etc.DisplayIfAttribute, bool>;

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
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (attribute is DisplayIfAttribute attr)
            {
                var prop = FindPathProperty(property, attr);
                if(prop==null)  return EditorGUI.GetPropertyHeight(property, true);
                if (IsDisable(attr, prop))
                    return -EditorGUIUtility.standardVerticalSpacing;
            }

            return EditorGUI.GetPropertyHeight(property, true);
        }

        private SerializedProperty FindPathProperty(SerializedProperty property, DisplayIfAttribute attr)
        {
            var path=property.propertyPath.Split('[', ']');
            if (path.Length == 1) return property.serializedObject.FindProperty(attr.VariableName);
            var iterator = property.serializedObject.GetIterator();
            while (iterator.Next(true))
            {
                if (iterator.name != attr.VariableName) continue;
                var iteratorPath = iterator.propertyPath.Split('[', ']');
                if(iteratorPath.Length==1) continue;
                if(path[1] != iteratorPath[1]) continue;
                return iterator;
            }
            return null;
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