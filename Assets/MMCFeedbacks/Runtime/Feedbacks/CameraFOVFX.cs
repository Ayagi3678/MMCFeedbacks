using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class CameraFOVFX : IFeedback
    {
        public int Order => 10;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Camera/Camera FOV";
        public Color TagColor => FeedbackStyling.CameraFXColor;

        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
        [Space(10)]
        [SerializeField] private Camera target;
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
            var initialValue = target.fieldOfView;
            Fov.DoTween(ignoreTimeScale, value => target.fieldOfView = value)
                .OnComplete(() =>
                {
                    if (isReturn) target.fieldOfView = initialValue;
                    State = FeedbackState.Completed;
                });
        }
    }
}