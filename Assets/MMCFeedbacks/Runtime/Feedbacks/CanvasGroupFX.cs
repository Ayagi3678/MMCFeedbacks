using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable] public class CanvasGroupFX : Feedback
    {
        public override int Order => 8;
        public override string MenuString => "UI/Canvas Group";
        public override Color TagColor => FeedbackStyling.UIFXColor;
        [Space(10)]
        [SerializeField] private CanvasGroup target;
        [SerializeField] private FloatTweenParameter Alpha=new();

        private TweenCallback _onCompleteCache;
        private DOGetter<float> _getterCache;
        private DOSetter<float> _setterCache;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onCompleteCache = Complete;
            _getterCache = () => target.alpha;
            _setterCache = x => target.alpha = x;
        }
        protected override void OnReset()
        {
            _tween?.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _tween = Alpha.ExecuteTween(_ignoreTimeScale,_getterCache,_setterCache)
                .OnComplete(_onCompleteCache);
        }

        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}