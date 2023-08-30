using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable] public class CameraFOVFX : Feedback
    {
        public override int Order => 10;
        public override string MenuString => "Camera/Camera FOV";
        public override Color TagColor => FeedbackStyling.CameraFXColor;
        [Space(10)]
        [SerializeField] private Camera target;
        [SerializeField] private bool isReturn;
        [SerializeField] private FloatTweenParameter Fov = new();

        private float _initialFOV;
        private Tween _tween;
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay()
        {
            _initialFOV = target.fieldOfView;
            _tween=Fov.DoTween(_ignoreTimeScale, value => target.fieldOfView = value)
                .OnKill(() =>
                {
                    if (isReturn) target.fieldOfView = _initialFOV;
                })
                .OnComplete(() =>
                {
                    if (isReturn) target.fieldOfView = _initialFOV;
                    Complete();
                });
        }
        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}