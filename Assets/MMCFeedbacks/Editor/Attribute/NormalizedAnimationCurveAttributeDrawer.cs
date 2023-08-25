using MMCFeedbacks.Core;

namespace MMCFeedbacks.Editor
{using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(NormalizedAnimationCurveAttribute))]
public class NormalizedAnimationCurveAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = (NormalizedAnimationCurveAttribute) attribute;

        if (property.propertyType != SerializedPropertyType.AnimationCurve)
        {
            // AnimationCurve以外のフィールドにアトリビュートが付けられていた場合のエラー表示
            position = EditorGUI.PrefixLabel(position, label);
            var preIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.LabelField(position, "Use NormalizedAnimationCurveAttribute with AnimationCurve.");
            EditorGUI.indentLevel = preIndent;
            return;
        }

        using (var ccs = new EditorGUI.ChangeCheckScope())
        {
            EditorGUI.PropertyField(position, property, label, true);
            if (ccs.changed)
            {
                if (attr.NormalizeValue)
                {
                    property.animationCurveValue = NormalizeValue(property.animationCurveValue);
                }

                if (attr.NormalizeTime)
                {
                    property.animationCurveValue = NormalizeTime(property.animationCurveValue);
                }
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property);
    }

    // アニメーションカーブの値（縦軸）を正規化する
    private static AnimationCurve NormalizeValue(AnimationCurve curve)
    {
        var keys = curve.keys;
        if (keys.Length <= 0)
        {
            return curve;
        }

        var minVal = keys[0].value;
        var maxVal = minVal;
        foreach (var t in keys)
        {
            minVal = Mathf.Min(minVal, t.value);
            maxVal = Mathf.Max(maxVal, t.value);
        }

        var range = maxVal - minVal;
        var valScale = range < 1 ? 1 : 1 / range;
        var valOffset = 0f;
        if (range < 1)
        {
            if (minVal > 0 && minVal + range <= 1)
            {
                valOffset = minVal;
            }
            else
            {
                valOffset = 1 - range;
            }
        }

        for (var i = 0; i < keys.Length; ++i)
        {
            keys[i].value = (keys[i].value - minVal) * valScale + valOffset;
        }

        curve.keys = keys;
        return curve;
    }

    // アニメーションカーブの時間（横軸）を正規化する
    private static AnimationCurve NormalizeTime(AnimationCurve curve)
    {
        var keys = curve.keys;
        if (keys.Length <= 0)
        {
            return curve;
        }

        var minTime = keys[0].time;
        var maxTime = minTime;
        foreach (var t in keys)
        {
            minTime = Mathf.Min(minTime, t.time);
            maxTime = Mathf.Max(maxTime, t.time);
        }

        var range = maxTime - minTime;
        var timeScale = range < 0.0001f ? 1 : 1 / range;
        for (var i = 0; i < keys.Length; ++i)
        {
            keys[i].time = (keys[i].time - minTime) * timeScale;
        }

        curve.keys = keys;
        return curve;
    }
}
#endif
}