using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class VignetteFX : IFeedback
    {
        public int Order => 7;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Volume/Vignette";
        public Color TagColor => FeedbackStyling.VolumeFXColor;
        
        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
        [SerializeField] private FloatTweenParameter Intensity=new(true);
        [SerializeField] private ColorTweenParameter Color = new(true);

        [Header("Vignette")] 
        [SerializeField][DisplayIf(nameof(Intensity),typeof(TweenParameter))][Range(0,1)]
        private float intensity;
        [SerializeField][DisplayIf(nameof(Color),typeof(TweenParameter))]
        private Color color;
        [SerializeField] private Vector2 center=new(.5f,.5f);
        [SerializeField,Range(.01f,1f)] private float smoothness=.2f;
        [SerializeField] private bool rounded;
        
        private Sequence _tweenSequence;
        private CancellationTokenSource _cancellationTokenSource;
        public void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }
        public void Play()
        {
            var vignette = VolumeSingleton.Instance.TryGetVolumeComponent<Vignette>();
            VolumeSingleton.Instance.EnableVolumeComponent(vignette);
            if (!Intensity.IsActive) vignette.intensity.value = intensity;
            if (!Color.IsActive) vignette.color.value = color;
            vignette.center.value = center;
            vignette.smoothness.value = smoothness;
            vignette.rounded.value = rounded;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new();
            _tweenSequence?.Kill();
            State = FeedbackState.Pending;
            PlayAsync(vignette).Forget();
        }

        public void Stop()
        {
            _tweenSequence.Pause();
        }
        private async UniTaskVoid PlayAsync(Vignette vignette)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),cancellationToken:_cancellationTokenSource.Token);
            State = FeedbackState.Running;

            _tweenSequence = DOTween.Sequence();
            _tweenSequence
                .Join(Intensity.DoTween(ignoreTimeScale, value => vignette.intensity.value = value))
                .Join(Color.DoTween(ignoreTimeScale, value => vignette.color.value = value));
            _tweenSequence.OnComplete(() =>
            {
                VolumeSingleton.Instance.DisableVolumeComponent(vignette);
                State = FeedbackState.Completed;
            });

        }
    }
}