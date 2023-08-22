using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.etc;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class LensDistortionFX : IFeedback
    {
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set;}
        public string MenuString => "Volume/LensDistortion";
        public Color TagColor => FeedbackStyling.volumeFeedbackColor;
        [SerializeField] private Timing timing = new();
        [SerializeField] private bool ignoreTimeScale;
        [Header("Intensity")]
        [SerializeField] private FloatTweenParameter intensity=new();
        [Header("Lens Distortion")]
        [SerializeField,Range(0,1)] private float xMultiplier=1;
        [SerializeField,Range(0,1)] private float yMultiplier=1;
        [SerializeField] private Vector2 center=new (.5f,.5f);
        [SerializeField, Range(0.01f, 5)] private float scale=1;

        private Tween _tween;
        private GameObject _gameObject;
        public void OnEnable(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public void Play()
        {
            var lensDistortion = VolumeSingleton.Instance.TryGetVolumeComponent<LensDistortion>();
            VolumeSingleton.Instance.EnableVolumeComponent(lensDistortion);
            lensDistortion.xMultiplier.value = xMultiplier;
            lensDistortion.yMultiplier.value = yMultiplier;
            lensDistortion.center.value = center;
            lensDistortion.scale.value = scale;
            
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
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),cancellationToken:_gameObject.GetCancellationTokenOnDestroy());
            State = FeedbackState.Running;
            
            _tween = intensity.DoTween(ignoreTimeScale, value=>lensDistortion.intensity.value=value)
                .OnComplete(() =>
                {
                    VolumeSingleton.Instance.DisableVolumeComponent(lensDistortion);
                    State = FeedbackState.Completed;
                });
        }

        public IFeedback Clone()
        {
            var copy = new LensDistortionFX
            {
                IsActive = IsActive,
                timing = timing,
                ignoreTimeScale = ignoreTimeScale,
                intensity = new FloatTweenParameter(intensity),
                xMultiplier = xMultiplier,
                yMultiplier = yMultiplier,
                center = center,
                scale = scale
            };
            return copy;
        }
    }
}