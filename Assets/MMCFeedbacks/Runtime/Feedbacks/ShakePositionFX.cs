using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable] public class ShakePositionFX : Feedback
    {
        public override int Order => 9;
        public override string MenuString => "Transform/Shake Position";
        public override Color TagColor => FeedbackStyling.TransformFXColor;
        [Space(10)]
        [SerializeField] private GameObject target;
        [SerializeField] private bool isRelative = true;
        [Header("Shake Position")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private float duration=1;
        [Space(10)]
        [SerializeField] private float strength=1;
        [SerializeField] private int vibrato = 10;
        [SerializeField] private float randomness = 90f;
        [SerializeField] private bool snapping;
        [SerializeField] private bool isFadeOut = true;
        
        private Vector3 _initialPosition;
        private Tween _tween;
        protected override void OnReset()
        {
            _tween?.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _initialPosition = target.transform.position;
            _tween = target.transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, isFadeOut)
                .SetUpdate(_ignoreTimeScale)
                .SetRelative(isRelative)
                .OnKill(()=> target.transform.position = _initialPosition)
                .OnComplete(() =>
                {
                    target.transform.position = _initialPosition;
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