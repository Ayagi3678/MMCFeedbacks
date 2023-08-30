using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MMCFeedbacks.Core
{
    [Serializable] public class ImageFillAmountFX : Feedback
    {
        public override int Order => 8;
        public override string MenuString => "UI/Image FillAmount";
        public override Color TagColor => FeedbackStyling.UIFXColor;
        [Space(10)]
        [SerializeField] private Image target;

        [SerializeField] private FloatTweenParameter ImageFillAmount=new();

        private Tween _tween;
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay()
        {
            _tween = ImageFillAmount.DoTween(_ignoreTimeScale, value => target.fillAmount = value)
                .OnComplete(Complete);
        }
        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}