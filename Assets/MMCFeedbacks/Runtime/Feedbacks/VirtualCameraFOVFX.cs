using System;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class VirtualCameraFOVFX : IFeedback
    {
         public int Order => 10;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Camera/Virtual Camera FOV";
        public Color TagColor => FeedbackStyling.CameraFXColor;

        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
        [Space(10)]
        [SerializeField] private CinemachineVirtualCamera target;
        [SerializeField] private bool isReturn;
        [SerializeField] private FloatTweenParameter Fov = new();

        private CancellationTokenSource _cancellationTokenSource;
        public void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }
        public void Play()
        {
            _cancellationTokenSource = new();
            State = FeedbackState.Pending;
            PlayAsync().Forget();
        }
        public void Stop()
        {
        }
        
        private async UniTaskVoid PlayAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),cancellationToken:_cancellationTokenSource.Token);
            State = FeedbackState.Running;
            var initialValue = target.m_Lens.FieldOfView;
            Fov.DoTween(ignoreTimeScale, value => target.m_Lens.FieldOfView = value)
                .OnComplete(() =>
                {
                    if (isReturn) target.m_Lens.FieldOfView = initialValue;
                    State = FeedbackState.Completed;
                });
        }
    }
}