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
    public class ChromaticAberrationFX : IFeedback
    {
        public int Order => 7;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Volume/Chromatic Aberration";
        public Color TagColor => FeedbackStyling.VolumeFXColor;
        
        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
        [SerializeField] private FloatTweenParameter Intensity=new();

        [Header("Chromatic Aberration")] 
        [SerializeField][DisplayIf(nameof(Intensity),typeof(TweenParameter))][Range(0,1)]
        private float intensity;
        private Tween _tween;
        private CancellationTokenSource _cancellationTokenSource;
        public void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }
        public void Play()
        {
            var chromaticAberration = VolumeSingleton.Instance.TryGetVolumeComponent<ChromaticAberration>();
            VolumeSingleton.Instance.EnableVolumeComponent(chromaticAberration);
            if (!Intensity.IsActive) chromaticAberration.intensity.value = intensity;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new();
            _tween?.Kill();
            State = FeedbackState.Pending;
            PlayAsync(chromaticAberration).Forget();
        }

        public void Stop()
        {
            _tween.Pause();
        }

        private async UniTaskVoid
            PlayAsync(ChromaticAberration vignette)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),cancellationToken:_cancellationTokenSource.Token);
            State = FeedbackState.Running;

            _tween = Intensity.DoTween(ignoreTimeScale, value => vignette.intensity.value = value)
                .OnComplete(() =>
                {
                    VolumeSingleton.Instance.DisableVolumeComponent(vignette);
                    State = FeedbackState.Completed;
                });

        }
    }
}