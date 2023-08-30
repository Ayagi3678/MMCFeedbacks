using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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

        private Tween _tween;
        protected override void OnReset()
        {
            _tween?.Kill();
        }

        protected override void OnPlay()
        {
            _tween = Alpha.DoTween(_ignoreTimeScale, value => target.alpha = value)
                .OnComplete(Complete);
        }

        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}