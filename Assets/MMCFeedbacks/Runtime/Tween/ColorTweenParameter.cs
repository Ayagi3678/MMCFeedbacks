using System;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class ColorTweenParameter : TweenParameter
    {
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)] 
        [NormalizedAnimationCurve(false)]private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private Color zero=Color.black;
        [SerializeField] private Color one=Color.white;
        [SerializeField] private float duration=1;
        
        public Tween DoTween(bool ignoreTimeScale,TweenCallback<Color> callback)
        {
            if (!IsActive) return null;
            var tween = DOVirtual.Color(zero, one, duration, callback)
                .SetUpdate(ignoreTimeScale);
            if (mode == EaseMode.Ease) 
                tween.SetEase(ease);
            else 
                tween.SetEase(curve);

            return tween;
        }
        public ColorTweenParameter(bool showActiveBox=false) => ShowActiveBox = showActiveBox;

        public ColorTweenParameter(bool isActive,bool showActiveBox=false)
        {
            IsActive = isActive;
            ShowActiveBox = showActiveBox;
        }
        public ColorTweenParameter(ColorTweenParameter parameter)
        {
            IsActive = parameter.IsActive;
            mode = parameter.mode;
            ease = parameter.ease;
            curve = parameter.curve;
            zero = parameter.zero;
            one = parameter.one;
            duration = parameter.duration;
        }
        
    }
}