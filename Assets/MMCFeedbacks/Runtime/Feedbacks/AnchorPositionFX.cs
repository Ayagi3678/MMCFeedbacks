using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using MMCFeedbacks.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable] public class AnchorPositionFX : Feedback
    {
        public override int Order => 8;
        public override string MenuString => "RectTransform/Anchor Position";
        public override Color TagColor => FeedbackStyling.RectTransformFXColor;
        [Space(10)]
        [SerializeField] private RectTransform target;
        [SerializeField] private bool isRelative = true;
        [SerializeField] private bool isReturn;
        [Header("Anchor Position")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),0)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),1)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private Vector3 zero;
        [SerializeField] private Vector3 one;
        [SerializeField] private float duration=1;
        
        private DOGetter<Vector3> _getterCache;
        private DOSetter<Vector3> _setterCache;
        private TweenCallback _onKillCache;
        private TweenCallback _onCompleteCache;
        
        private Vector3 _initialPosition;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _getterCache = () => target.anchoredPosition3D;
            _setterCache = x => target.anchoredPosition3D = x;
            _onKillCache = () => { if (isReturn) target.anchoredPosition3D = _initialPosition; };
            _onCompleteCache = () => { if (isReturn) target.anchoredPosition3D = _initialPosition; Complete(); };
        }
        protected override void OnReset()
        {
            _tween?.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
             _initialPosition = target.anchoredPosition3D;
            _tween = DOTween.To(_getterCache,_setterCache,one,duration)
                .From(zero,true,isRelative)
                .SetRelative(isRelative)
                .SetUpdate(_ignoreTimeScale)
                .OnKill(_onKillCache)
                .OnComplete(_onCompleteCache);
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