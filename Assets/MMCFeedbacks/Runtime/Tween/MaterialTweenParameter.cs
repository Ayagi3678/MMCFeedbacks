using System;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class MaterialTweenParameter : TweenParameter
    {
        [SerializeField] private Material target;
        [SerializeField] private string propertyName;
        [SerializeField] private ParameterType type;
        [SerializeField,DisplayIf(nameof(type),2)] private ColorTweenParameter Color = new();
        [SerializeField,DisplayIf(nameof(type),0)] private FloatTweenParameter Float = new();
        [SerializeField,DisplayIf(nameof(type),1)] private IntTweenParameter Int = new();
        [SerializeField, DisplayIf(nameof(type), 1)] private Vector3TweenParameter Vector3 = new();

        public Tween DoTween(bool ignoreTimeScale)
        {
            if (!IsActive) return null;
            Tween tween = type switch
            {
                ParameterType.Float => Float.DoTween(ignoreTimeScale, value => target.SetFloat(propertyName, value)),
                ParameterType.Int => Int.DoTween(ignoreTimeScale, value => target.SetInt(propertyName, value)),
                ParameterType.Color => Color.DoTween(ignoreTimeScale, value => target.SetColor(propertyName, value)),
                ParameterType.Vector3 => Vector3.DoTween(ignoreTimeScale,value=> target.SetVector(propertyName,value)),
                _ => throw new ArgumentOutOfRangeException()
            };
            return tween;
        }

        public MaterialTweenParameter(bool showActiveBox=false) => ShowActiveBox = showActiveBox;

        public MaterialTweenParameter(bool isActive,bool showActiveBox=false)
        {
            IsActive = isActive;
            ShowActiveBox = showActiveBox;
        }
        public MaterialTweenParameter(MaterialTweenParameter parameter)
        {
            IsActive = parameter.IsActive;
            ShowActiveBox = parameter.ShowActiveBox;
            propertyName = parameter.propertyName;
            target = parameter.target;
            type = parameter.type;
            Float = new FloatTweenParameter(parameter.Float);
            Int = new IntTweenParameter(parameter.Int);
            Color = new ColorTweenParameter(parameter.Color);
        }
    }

    public enum ParameterType
    {
        Color,
        Float,
        Int,
        Vector3
    }
}