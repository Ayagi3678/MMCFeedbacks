using System;
using MMCFeedbacks.Core;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

namespace MMCFeedbacks.Editor
{
    [CustomPropertyDrawer(typeof(FeedbackList))]
    public class FeedbackListDrawer : PropertyDrawer
    {
        private ReorderableList _reorderableList;
        private FeedbackList _feedbackList;
        private bool _isProSkin;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(_reorderableList==null) Initialize(property);

            if (fieldInfo.GetValue(property.serializedObject.targetObject) is FeedbackList feedbackList) 
                _feedbackList = feedbackList;
            var listRect = position;
            listRect.xMin = 7f + property.depth;
            EditorGUI.BeginChangeCheck();
            _reorderableList?.DoList(listRect);
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
            var addFeedbackRect = position;
            addFeedbackRect.xMin = (EditorGUIUtility.currentViewWidth*.2f)*.5f;
            addFeedbackRect.height = 21;
            addFeedbackRect.width = EditorGUIUtility.currentViewWidth*.6f;
            addFeedbackRect.y += _reorderableList.GetHeight();
            if (GUI.Button(addFeedbackRect,"AddFeedback", new GUIStyle("MiniPullDown")))
            {
                var dropdown = new FeedbackDropDown(new AdvancedDropdownState());
                dropdown.OnSelect += item => _feedbackList.AddFeedback(item.name);
                dropdown.Show(addFeedbackRect);
            }

            var copyAllRect = addFeedbackRect;
            copyAllRect.xMin+=EditorGUIUtility.currentViewWidth*.6f;
            copyAllRect.width = EditorGUIUtility.currentViewWidth*.2f;
            if (GUI.Button(copyAllRect, "Clear", new GUIStyle("minibutton")))
            {
                _feedbackList.List.Clear();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if(_reorderableList==null) Initialize(property);
            return _reorderableList.GetHeight()+25f;
        }

        private void Initialize(SerializedProperty property)
        {
            var listProperty = property.FindPropertyRelative("List");
            _reorderableList = new ReorderableList(property.serializedObject, listProperty, true, true,
                false, false)
            {
                drawElementCallback = DrawElement,
                drawElementBackgroundCallback = (rect, index, active, focused) => {},
                elementHeightCallback = GetElementHeight,
                headerHeight = 30f,
                footerHeight = 5f,
                showDefaultBackground = false,
                onChangedCallback = list => { property.serializedObject.ApplyModifiedProperties(); },
                drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y + 5, rect.width, rect.height), " Feedbacks :",new GUIStyle("BoldLabel"));
                    EditorGUI.DrawRect(new Rect(rect.x, rect.y + rect.height, rect.width, 1), Color.gray);
                },
                drawNoneElementCallback = rect =>
                {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, rect.height),"");
                }
            };
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            if (element.managedReferenceValue is IFeedback referenceValue)
            {
                var tagColor = referenceValue.TagColor;
                var label =referenceValue.Label;
                var isMenu = false;

                var elementIsExpanded = element.isExpanded;
                referenceValue.IsActive = FeedbackEditorUtility.Foldout(rect,label,tagColor,referenceValue.IsActive,ref isMenu ,ref elementIsExpanded);
                element.isExpanded = elementIsExpanded;

                if (isMenu)
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Delete"), false, () => _feedbackList.List.RemoveAt(index));
                    menu.AddItem(new GUIContent("Duplicate"), false, () =>
                    {
                        _feedbackList.List.Add(referenceValue.CopyFeedback());
                    });
                    menu.AddItem(new GUIContent("Reset"), false, () =>
                    {
                        _feedbackList.List.RemoveAt(index);
                        _feedbackList.List.Insert(index, Activator.CreateInstance(referenceValue.GetType())as IFeedback);
                    });
                    menu.ShowAsContext();
                }

                var propertyRect = rect;
                propertyRect.y += 25;
                propertyRect.height = EditorGUIUtility.singleLineHeight;

                if (elementIsExpanded)
                {
                    var depth = -1;
                    EditorGUI.BeginDisabledGroup(!referenceValue.IsActive);
                    while(element.NextVisible(true) || depth == -1){
                        if (depth != -1 && element.depth < depth) break;
                        if (depth != -1 && element.depth > depth) continue;
                        
                        depth = element.depth;
                        propertyRect.height = EditorGUI.GetPropertyHeight(element);
                        EditorGUI.PropertyField(propertyRect,element,true);
                        propertyRect.y += EditorGUI.GetPropertyHeight(element)+2;
                    }
                    EditorGUI.EndDisabledGroup();
                    
                    propertyRect.y += 10;
                    propertyRect.height = EditorGUIUtility.singleLineHeight;
                    var playButtonRect = propertyRect;
                    playButtonRect.width = rect.width*.5f;
                    var stopButtonRect = playButtonRect;
                    stopButtonRect.x += rect.width*.5f;
                    EditorGUI.BeginDisabledGroup(!EditorApplication.isPlaying);
                    if (GUI.Button(playButtonRect, "Play", new GUIStyle("minibuttonmid")))
                    {
                        referenceValue.Play();
                    }
                    if (GUI.Button(stopButtonRect,"Stop",new GUIStyle("minibuttonmid")))
                    {
                        referenceValue.Stop();
                    }
                    EditorGUI.EndDisabledGroup();
                }
            }
        }

        private float GetElementHeight(int index)
        {
            var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            if (element.isExpanded)
                return EditorGUI.GetPropertyHeight(element) + 38;
            return EditorGUI.GetPropertyHeight(element);
        }
    }
}