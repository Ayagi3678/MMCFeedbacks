using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class AudioVolumeFX : IFeedback
    {
         public int Order => 11;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Audio/Audio Volume";
        public Color TagColor => FeedbackStyling.AudioFXColor;
        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
        [Space(10)] 
        [SerializeField] private AudioSource target;
        [SerializeField] private bool isReturn;

        [SerializeField] private FloatTweenParameter Volume = new();
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
            var initialVolume = target.volume;
            _tween=Volume.DoTween(ignoreTimeScale, value => target.volume = value)
                .OnComplete(() =>
                {
                    if (isReturn) target.volume = initialVolume;
                    State = FeedbackState.Completed;
                });
        }
    }
}