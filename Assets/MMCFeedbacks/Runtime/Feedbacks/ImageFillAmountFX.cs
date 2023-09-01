using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
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

        private TweenCallback _onCompleteCache;
        private DOGetter<float> _getterCache;
        private DOSetter<float> _setterCache;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onCompleteCache = Complete;
            _getterCache = () => target.fillAmount;
            _setterCache = x => target.fillAmount = x;
        }
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay(CancellationToken token)
        {
            _tween = ImageFillAmount.ExecuteTween(_ignoreTimeScale, _getterCache,_setterCache)
                .OnComplete(_onCompleteCache);
        }
        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}