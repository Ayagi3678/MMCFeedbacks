using System;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using Random = System.Random;

namespace MMCFeedbacks.Core
{
    [Serializable] public class VirtualCameraFOVFX : Feedback
    {
        public override int Order => 10;
        public override string MenuString => "Camera/Virtual Camera FOV";
        public override Color TagColor => FeedbackStyling.CameraFXColor;
        [Space(10)]
        [SerializeField] private CinemachineVirtualCamera target;
        [SerializeField] private bool isReturn;
        [SerializeField] private FloatTweenParameter Fov = new();

        private TweenCallback _onCompleteCache;
        private DOGetter<float> _getterCache;
        private DOSetter<float> _setterCache;
        private float _initialFOV;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onCompleteCache = () => { if (isReturn) target.m_Lens.FieldOfView = _initialFOV; Complete(); };
            _getterCache = () => target.m_Lens.FieldOfView;
            _setterCache = x => target.m_Lens.FieldOfView = x;
        }
        protected override void OnReset()
        {
            _tween?.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _initialFOV = target.m_Lens.FieldOfView;
            _tween= Fov.ExecuteTween(_ignoreTimeScale, _getterCache,_setterCache)
                .OnComplete(_onCompleteCache);
        }

        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}