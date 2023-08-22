using System;
using MMCFeedbacks.Core;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MMCFeedbacks.Editor
{
    [CustomEditor(typeof(FeedbackPlayer))]
    public class FeedbackPlayerEditor : UnityEditor.Editor
    {
        private FeedbackPlayer _feedbackPlayer;

        private void OnEnable()
        {
            _feedbackPlayer = (FeedbackPlayer)target;
            
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("mode"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("loopCount"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("playAwake"));
            GUILayout.Space(-5);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("list"));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            var splitterRect = EditorGUILayout.GetControlRect(false, GUILayout.Width(EditorGUIUtility.currentViewWidth-25),GUILayout.Height(1));
            splitterRect.xMin -= 5;
            splitterRect.xMax -= 5;
            GUILayout.Space(5);
            EditorGUI.DrawRect(splitterRect, Color.gray);
            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(!EditorApplication.isPlaying);
            if (GUILayout.Button("Play",new GUIStyle("ButtonMid"))) _feedbackPlayer.PlayFeedbacks();
            if (GUILayout.Button("Stop",new GUIStyle("ButtonMid"))) _feedbackPlayer.StopFeedbacks();
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button("Copy all", new GUIStyle("ButtonMid")))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Copy"), false, () => ComponentUtility.CopyComponent(_feedbackPlayer));
                menu.AddItem(new GUIContent("Paste Value"), false, () => ComponentUtility.PasteComponentValues(_feedbackPlayer));
                menu.AddItem(new GUIContent("Paste as New"), false, () =>ComponentUtility.PasteComponentAsNew(_feedbackPlayer.gameObject));
                menu.ShowAsContext();
            }
            GUILayout.EndHorizontal();
        }
    }
}