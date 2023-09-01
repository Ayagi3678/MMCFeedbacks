using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
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

        private TweenCallback _onKillCache;
        private TweenCallback _onCompleteCache;
        private DOGetter<float> _getterCache;
        private DOSetter<float> _setterCache;
        private float _initialFOV;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onKillCache = () => { if (isReturn) target.fieldOfView = _initialFOV; };
            _onCompleteCache = () => { if (isReturn) target.fieldOfView = _initialFOV; Complete(); };
            _getterCache = () => target.fieldOfView;
            _setterCache = x => target.fieldOfView = x;
        }
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay(CancellationToken token)
        {
            _initialFOV = target.fieldOfView;
            _tween=Fov.ExecuteTween(_ignoreTimeScale, _getterCache,_setterCache)
                .OnKill(_onKillCache)
                .OnComplete(_onCompleteCache);
        }
        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}