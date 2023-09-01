using System;
using DG.Tweening;
using DG.Tweening.Core;
using MMCFeedbacks.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class IntTweenParameter : TweenParameter
    {
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private int zero=1;
        [SerializeField] private int one;
        [SerializeField] private float duration=1;
        
        public Tween DoTween(bool ignoreTimeScale,DOGetter<int> getter,DOSetter<int> setter)
        {
            if (!IsActive) return null;
            var tween = DOTween.To(getter,setter,one,duration)
                .From(zero)
                .SetUpdate(ignoreTimeScale);
            if (mode == EaseMode.Ease) 
                tween.SetEase(ease);
            else 
                tween.SetEase(curve);
            
            return tween;
        }
        public IntTweenParameter(bool showActiveBox=false) => ShowActiveBox = showActiveBox;

        public IntTweenParameter(bool isActive,bool showActiveBox=false)
        {
            IsActive = isActive;
            ShowActiveBox = showActiveBox;
        }
        public IntTweenParameter(IntTweenParameter parameter)
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