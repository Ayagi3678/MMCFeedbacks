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
    public class LensDistortionFX : IFeedback
    {
        public int Order => 7;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set;}
        public string MenuString => "Volume/Lens Distortion";
        public Color TagColor => FeedbackStyling.VolumeFXColor;
        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
        [SerializeField] private FloatTweenParameter Intensity=new();

        [Header("Lens Distortion")] 
        [DisplayIf(nameof(Intensity),typeof(TweenParameter))]
        [SerializeField] private float intensity;
        [SerializeField,Range(0,1)] private float xMultiplier=1;
        [SerializeField,Range(0,1)] private float yMultiplier=1;
        [SerializeField] private Vector2 center=new (.5f,.5f);
        [SerializeField, Range(0.01f, 5)] private float scale=1;

        private Tween _tween;
        private CancellationTokenSource _cancellationTokenSource;
        public void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }
        public void Play()
        {
            var lensDistortion = VolumeSingleton.Instance.TryGetVolumeComponent<LensDistortion>();
            VolumeSingleton.Instance.EnableVolumeComponent(lensDistortion);
            if (!Intensity.IsActive) lensDistortion.intensity.value = intensity;
            lensDistortion.xMultiplier.value = xMultiplier;
            lensDistortion.yMultiplier.value = yMultiplier;
            lensDistortion.center.value = center;
            lensDistortion.scale.value = scale;
            
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new();
            _tween?.Kill();
            State = FeedbackState.Pending;
            PlayAsync(lensDistortion).Forget();
        }

        public void Stop()
        {
            _tween?.Pause();
        }

        private async UniTaskVoid PlayAsync(LensDistortion lensDistortion)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),cancellationToken:_cancellationTokenSource.Token);
            State = FeedbackState.Running;
            
            _tween = Intensity.DoTween(ignoreTimeScale, value=>lensDistortion.intensity.value=value)
                .OnComplete(() =>
                {
                    VolumeSingleton.Instance.DisableVolumeComponent(lensDistortion);
                    State = FeedbackState.Completed;
                });
        }
    }
}