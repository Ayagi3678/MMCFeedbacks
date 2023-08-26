using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class AudioPitchFX : IFeedback
    {
        public int Order => 11;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Audio/Audio Pitch";
        public Color TagColor => FeedbackStyling.AudioFXColor;
        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
        [Space(10)] 
        [SerializeField] private AudioSource target;
        [SerializeField] private bool isReturn;

        [SerializeField] private FloatTweenParameter Pitch = new();
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
            State = FeedbackState.Pending;
            _tween?.Kill();
            PlayAsync().Forget();
        }

        public void Stop()
        {
            target.Stop();
        }

        private async UniTaskVoid PlayAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),
                cancellationToken: _cancellationTokenSource.Token);
            State = FeedbackState.Running;
            var initialPitch = target.pitch;
            _tween=Pitch.DoTween(ignoreTimeScale, value => target.pitch = value)
                .OnComplete(() =>
                {
                    if (isReturn) target.pitch = initialPitch;
                    State = FeedbackState.Completed;
                });
        }
    }
}