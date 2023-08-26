using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class CanvasGroupFX : IFeedback
    {
        public int Order => 8;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "UI/Canvas Group";
        public Color TagColor => FeedbackStyling.UIFXColor;

        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
        [Space(10)]
        [SerializeField] private CanvasGroup target;

        [SerializeField] private FloatTweenParameter Alpha=new();

        private Tween _tween;
        private CancellationTokenSource _cancellationTokenSource;
        public void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }
        public void Play()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new();
            _tween?.Kill();
            State = FeedbackState.Pending;
            PlayAsync().Forget();
        }

        public void Stop()
        {
            _tween.Pause();
        }
        private async UniTaskVoid PlayAsync()
        {
            
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),cancellationToken:_cancellationTokenSource.Token);
            State = FeedbackState.Running;

            _tween = Alpha.DoTween(ignoreTimeScale, value => target.alpha = value)
                .OnComplete(() => State=FeedbackState.Completed);

        }
    }
}