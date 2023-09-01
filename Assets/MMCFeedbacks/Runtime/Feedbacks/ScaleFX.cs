using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
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
        
        private TweenCallback _onKillCache;
        private TweenCallback _onCompleteCache;
        private DOGetter<Vector3> _getterCache;
        private DOSetter<Vector3> _setterCache;
        
        private Vector3 _initialScale;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onKillCache = () => { if (isReturn) target.transform.localScale = _initialScale; };
            _onCompleteCache = () => { if (isReturn) target.transform.localScale = _initialScale; Complete(); };
            _getterCache = () => target.transform.localScale;
            _setterCache = x => target.transform.localScale = x;
        }
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay(CancellationToken token)
        {
            _initialScale = target.transform.localScale;
            _tween = DOTween.To(_getterCache,_setterCache,one,duration)
                .From(zero, true, isRelative)
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