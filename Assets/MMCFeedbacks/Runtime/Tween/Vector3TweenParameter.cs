using System;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class Vector3TweenParameter: TweenParameter
    {
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private Vector3 zero;
        [SerializeField] private Vector3 one;
        [SerializeField] private float duration=1;
        
        public Tween DoTween(bool ignoreTimeScale,DOGetter<Vector3> getter,DOSetter<Vector3> setter)
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
        public Vector3TweenParameter(bool showActiveBox=false) => ShowActiveBox = showActiveBox;

        public Vector3TweenParameter(bool isActive,bool showActiveBox=false)
        {
            IsActive = isActive;
            ShowActiveBox = showActiveBox;
        }
        public Vector3TweenParameter(Vector3TweenParameter parameter)
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