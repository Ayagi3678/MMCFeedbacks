using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable] public class ScaleFX : Feedback
    {
        public override int Order => 9;
        public override string MenuString => "Transform/Scale";
        public override Color TagColor => FeedbackStyling.TransformFXColor;
        [Space(10)] 
        
        [SerializeField] private GameObject target;
        [SerializeField] private bool isRelative = true;
        [SerializeField] private bool isReturn;
        [Header("Scale")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private Vector3 zero;
        [SerializeField] private Vector3 one;
        [SerializeField] private float duration=1;
        
        private Vector3 _initialScale;
        private Tween _tween;
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay()
        {
            _initialScale = target.transform.localScale;
            _tween = target.transform.DOScale(one, duration)
                .From(zero, true, isRelative)
                .SetRelative(isRelative)
                .SetUpdate(_ignoreTimeScale)
                .OnKill(() =>
                {
                    if (isReturn) target.transform.localScale = _initialScale;
                })
                .OnComplete(() =>
                {
                    if (isReturn) target.transform.localScale = _initialScale;
                    Complete();
                });

            
            if (mode == EaseMode.Ease) 
                _tween.SetEase(ease);
            else 
                _tween.SetEase(curve);
        }
        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}