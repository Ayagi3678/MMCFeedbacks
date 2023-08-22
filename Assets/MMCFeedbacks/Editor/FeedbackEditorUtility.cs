using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MMCFeedbacks.Editor
{
    public static class FeedbackEditorUtility
    {
        internal static bool Foldout(Rect rect,string label, Color tagColor,bool isActive,ref bool isMenu ,ref bool isExpanded)
        {
            var labelRect = rect;
            labelRect.xMin += 32f;
            labelRect.xMax -= 20f;
            labelRect.y += 4f;
            labelRect.height = 13f;

            var foldoutRect = rect;
            foldoutRect.y += 1f;
            foldoutRect.height = 18f;

            var toggleRect = rect;
            toggleRect.x += 16f;
            toggleRect.y += 4f;
            toggleRect.width = 20f;
            toggleRect.height = 20f;

            var menuRect = rect;
            menuRect.xMin += rect.width-50f;
            menuRect.height = 20f;
            menuRect.width = 50f;

            var backgroundRect = rect;
            backgroundRect.xMin -= 27f;
            backgroundRect.xMax += 20f;
            backgroundRect.height = 20;

            var tagRect = rect;
            tagRect.xMin -= 27f;
            tagRect.width = 2f;
            tagRect.height = 20f;

            var tagBackgroundRect = rect;
            tagBackgroundRect.xMin -= 27f;
            tagBackgroundRect.width = 3f;

            var topLineRect = rect;
            topLineRect.xMin -= 27f;
            topLineRect.xMax += 20f;
            topLineRect.height = 1f;

            var bottomLineRect = rect;
            bottomLineRect.xMin -= 27f;
            bottomLineRect.xMax += 20f;
            bottomLineRect.y += rect.height;
            bottomLineRect.height = 1f;
            
            // Background
            EditorGUI.DrawRect(backgroundRect, EditorStyling.HeaderBackground);
            /*EditorGUI.DrawRect(backgroundRect, tagColor*.3f);*/
            EditorGUI.DrawRect(topLineRect,EditorStyling.HeaderBackgroundLine);
            EditorGUI.DrawRect(bottomLineRect,EditorStyling.HeaderBackgroundLine);
            EditorGUI.DrawRect(tagBackgroundRect,tagColor*.5f);
            EditorGUI.DrawRect(tagRect,tagColor);
            // Title
            EditorGUI.LabelField(labelRect, label, EditorStyles.boldLabel);
            
            // Active checkbox
            isActive = GUI.Toggle(toggleRect, isActive, GUIContent.none, EditorStyling.smallTickbox);

            // menu
            isMenu = GUI.Button(menuRect, "[menu]",new GUIStyle("Label"));
            // foldout
            isExpanded = GUI.Toggle(foldoutRect, isExpanded, GUIContent.none, EditorStyles.foldout);

            return isActive;
        }
        
    }
}