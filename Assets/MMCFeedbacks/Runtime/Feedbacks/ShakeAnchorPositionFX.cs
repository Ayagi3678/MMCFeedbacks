using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    //TODO:MagicTweenに対応
    /*[Serializable] public class ShakeAnchorPositionFX : Feedback
    {
        public override int Order => 8;
        public override string MenuString => "RectTransform/Shake Anchor Position";
        public override Color TagColor => FeedbackStyling.RectTransformFXColor;
        [Space(10)]
        [SerializeField] private RectTransform target;
        [SerializeField] private bool isRelative = true;
        [Header("Shake Anchor Position")]
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
        protected override void OnPlay()
        {
            _initialPosition = target.anchoredPosition3D;
            _tween = target.DOShakeAnchorPos(duration,strength,vibrato,randomness,snapping,isFadeOut)
                .SetRelative(isRelative)
                .SetUpdate(_ignoreTimeScale)
                .OnKill(() =>
                {
                    target.anchoredPosition3D = _initialPosition;
                })
                .OnComplete(() =>
                {
                    target.anchoredPosition3D = _initialPosition;
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
    }*/
}