using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MMCFeedbacks.Core
{
    //TODO:MagicTweenが出たら対応
    /*[Serializable] public class LegacyTextFX : Feedback
    {
        public override int Order => 8;
        public override string MenuString => "UI/Legacy Text";
        public override Color TagColor => FeedbackStyling.UIFXColor;
        [Space(10)]
        [SerializeField] private Text target;
        [Header("Text")]
        [SerializeField] private ScrambleMode scrambleMode;
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private float duration=1;

        [SerializeField, TextArea] private string zero;
        [SerializeField, TextArea] private string one;

        private Tween _tween;
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay()
        {
            _tween = target.DOText(one, duration, true, scrambleMode)
                .From(zero)
                .SetUpdate(_ignoreTimeScale)
                .OnComplete(Complete());
            if (mode == EaseMode.Ease) 
                _tween.SetEase(ease);
            else 
                _tween.SetEase(curve);
        }
        protected override void OnStop()
        {
            _tween.Pause();
        }
    }*/
}