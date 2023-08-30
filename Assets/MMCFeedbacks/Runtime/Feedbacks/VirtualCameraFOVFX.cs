using System;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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

        private float _initialFOV;
        private Tween _tween;
        protected override void OnReset()
        {
            _tween?.Kill();
        }

        protected override void OnPlay()
        {
            _initialFOV = target.m_Lens.FieldOfView;
            _tween= Fov.DoTween(_ignoreTimeScale, value => target.m_Lens.FieldOfView = value)
                .OnComplete(() =>
                {
                    if (isReturn) target.m_Lens.FieldOfView = _initialFOV;
                    Complete();
                });
        }

        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}