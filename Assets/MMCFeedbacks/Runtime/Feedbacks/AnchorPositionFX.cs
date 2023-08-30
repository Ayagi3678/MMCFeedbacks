using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        [SerializeField] private bool isReturn;
        [SerializeField] private Vector3TweenParameter AnchorPosition=new();
        
        private Vector3 _initialPosition;
        private Tween _tween;
        protected override void OnReset()
        {
            _tween?.Kill();
        }

        protected override void OnPlay()
        {
             _initialPosition = target.anchoredPosition3D;
            _tween = AnchorPosition.DoTween(_ignoreTimeScale,value=>target.anchoredPosition3D=value)
                .OnKill(() =>
                {
                    if (isReturn) target.anchoredPosition3D = _initialPosition;
                })
                .OnComplete(() =>
                {
                    if (isReturn) target.anchoredPosition3D = _initialPosition;
                    Complete();
                });
        }

        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}